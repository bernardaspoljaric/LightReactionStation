#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Novena.Admin.FileSave.Attribute;
using Novena.DAL.Entity;
using Novena.DAL.Model.Guide;
using Novena.Networking;
using UnityEngine;
using File = System.IO.File;

namespace Novena.Admin.FileSave
{
  public class Files
  {
    /// <summary>
    /// List of all files in guide.
    /// </summary>
    public List<GuideFile> AllFiles { get; private set; }

    /// <summary>
    /// List of files for download.
    /// </summary>
    public List<GuideFile> FilesToDownload { get; private set; }

    /// <summary>
    /// Download size of files to download.
    /// </summary>
    public ulong DownloadSize { get; private set; }

    /// <summary>
    /// Total size of all files
    /// </summary>
    public ulong TotalSize { get; private set; }

    #region PrivateFields

    private Guide _guide;

    #endregion


    public Files(Guide guide)
    {
      _guide = guide;
      AllFiles = new List<GuideFile>();
      FilesToDownload = new List<GuideFile>();
    }

    /// <summary>
    /// Get all files from guide.
    /// </summary>
    /// <returns>List of GuideFile or NULL if nothing found</returns>
    public async UniTask<List<GuideFile>?> GetFilesAsync()
    {
      //Lets get all files
      await GetFiles();

      //Lets get files for download
      await GetFilesForDownload();

      //Finaly return list of all files
      return AllFiles;
    }

    /// <summary>
    /// Get all files in guide.
    /// </summary>
    /// <returns></returns>
    private async UniTask<List<GuideFile>> GetFiles()
    {
      await UniTask.RunOnThreadPool(() =>
      {
        //Lets get files for guide
        SearchForFiles(_guide);
      });

      //Calculate total file size.
      AllFiles.ForEach(f => TotalSize += f.Size);

      return AllFiles;
    }

    /// <summary>
    /// Search for properties with File Attribute in object.
    /// Every file has 3 properties: Path, Timestamp and Size.
    /// Every file is stored in AllFiles list.
    /// </summary>
    /// <param name="obj">Object to search</param>
    private void SearchForFiles(object obj)
    {
      var properties = obj.GetType().GetProperties();
      var filesProperty = obj.GetType().GetProperties()
        .Where(p => System.Attribute.IsDefined(p, typeof(Attribute.File))).ToList();

      if (filesProperty.Any())
      {
        GuideFile guideFile = new GuideFile();

        foreach (var info in filesProperty)
        {
          var atributes = info.GetCustomAttributes(true);

          foreach (var atribute in atributes)
          {
            Attribute.File fileAttr = atribute as Attribute.File;

            if (fileAttr != null)
            {
              switch (fileAttr.Type)
              {
                case FileAttributeType.Path:
                  guideFile.Path = info.GetValue(obj) as string;
                  break;
                case FileAttributeType.Size:
                  guideFile.Size = info.GetValue(obj) is ulong ? (ulong)info.GetValue(obj) : 0;
                  break;
                case FileAttributeType.Timestamp:
                  guideFile.TimeStamp = info.GetValue(obj) as string;
                  break;
              }
            }
          }
        }

        //If object doesn't have actual file
        if (string.IsNullOrEmpty(guideFile.Path) == false && string.IsNullOrEmpty(guideFile.TimeStamp) == false)
        {
          guideFile.Path = guideFile.Path.Replace("~", "");

          //We don't wont to get same file if is allready added
          if (AllFiles.Any(gf => gf.Path == guideFile.Path) == false)
          {
            AllFiles.Add(guideFile);
          }
        }
      }

      foreach (var property in properties)
      {
        if (property.PropertyType.IsArray || property.GetType().IsGenericType && property.GetValue(obj) is IList)
        {
          var values = property.GetValue(obj) as IList;

          if (values == null) continue;

          foreach (var value in values)
          {
            SearchForFiles(value);
          }
        }
      }
    }

    /// <summary>
    /// Filter files to be downloaded.
    /// </summary>
    /// <returns>List of files that not exist in db or they are outdated!</returns>
    private async UniTask<List<GuideFile>> GetFilesForDownload()
    {
      await UniTask.SwitchToThreadPool();

      //Temp list of filtered files to return
      List<GuideFile> tempFiles = new List<GuideFile>();

      DownloadSize = 0;

      var currentLanguageFiles = AllFiles.ToArray();

      using (FilesEntity filesEntity = new())
      {

      //Get all files from db
      var filesFromDb = filesEntity.GetAll();

      //Compare files in db with currentLanguageFiles
      for (int i = 0; i < currentLanguageFiles.Length; i++)
      {
        var file = currentLanguageFiles[i];

        //If file in currentLanguageFiles don't exist or is outdated in db
        var missingFile =
          filesFromDb.FirstOrDefault(fdb => fdb.FilePath == file.Path && fdb.TimeStamp == file.TimeStamp);

        var needsUpdate = filesFromDb.FirstOrDefault(fdb => fdb.FilePath == file.Path && fdb.TimeStamp == file.TimeStamp);

        if (missingFile == null)
        {
          //If is missing add him to temp list
          DownloadSize += file.Size;
          tempFiles.Add(file);
          CleanImageLoaderCacheFile(NCore.Instance.PersistentDataPath + file.Path);
        }
      }

      }

      await UniTask.SwitchToMainThread();

      FilesToDownload = tempFiles;

      return tempFiles;
    }

    /// <summary>
    /// Deletes files that are not used anymore.
    /// <para>
    /// This method iterate through all files that was previously downloaded.
    /// Compares each file with currently downloaded guides.
    /// If file doesnt exist anymore it will be deleted as file and as record in Db.
    /// </para>
    /// </summary>
    public static async UniTaskVoid DeleteUnusedFiles()
    {
      await UniTask.SwitchToThreadPool();

      try
      {
        List<GuideFile> currentFiles = new List<GuideFile>();

        GuidesEntity guidesEntity = new GuidesEntity();

        var guides = guidesEntity.GetAll();

        guidesEntity.Dispose();

        foreach (var localGuide in guides)
        {
          var guide = JsonConvert.DeserializeObject<Guide>(localGuide.Json);

          Files files = new Files(guide);
          var f = await files.GetFilesAsync();

          currentFiles.AddRange(files.AllFiles);
        }

        FilesEntity filesEntity = new();

        var localFiles = filesEntity.GetAll();

        foreach (var localFile in localFiles)
        {
          var filesExist = currentFiles.Where(cf => cf.Path == localFile.FilePath).ToList();

          if (filesExist.Any() == false)
          {
            if (File.Exists(localFile.LocalPath))
            {
              File.Delete(localFile.LocalPath);
              CleanImageLoaderCacheFile(localFile.LocalPath);
            }

            filesEntity.DeleteById(localFile.Id);

            Debug.LogWarning($"DELETED FILE: {localFile.FilePath}");
          }
        }

      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }

      await UniTask.SwitchToMainThread();
    }

    /// <summary>
    /// Delete file from ImageLoaderCache directory.
    /// </summary>
    /// <param name="path"></param>
    private static void CleanImageLoaderCacheFile(string path)
    {
      var dir = Path.GetDirectoryName(path);
      var fileName = Path.GetFileNameWithoutExtension(path);
      var imageLoaderCache = "ImageLoaderCache";
      var imageLoaderCacheDir = Path.Combine(dir, imageLoaderCache);
      if (Directory.Exists(imageLoaderCacheDir))
      {
        var files = Directory.GetFiles(imageLoaderCacheDir, $"{fileName}.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
          File.Delete(file);
        }
      }
    }
  }

  public class GuideFile
  {
    public string Path { get; set; }
    public int LanguageId { get; set; }
    public string TimeStamp { get; set; }
    public ulong Size { get; set; }
  }
}