using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Doozy.Engine.Progress;
using Novena.Admin.FileSave;
using Novena.Components.Dialog;
using Novena.Components.ProgressIndicator;
using Novena.DAL;
using Novena.DAL.Entity;
using Novena.DAL.Model;
using Novena.Networking;
using Novena.Networking.Download;
using Novena.UiUtility.Base;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novena.Admin.Pages.Download
{
  public class DownloadView : UiController
  {
    [Header("Download info")] 
    [SerializeField] private TMP_Text _guideName;
    [SerializeField] private TMP_Text _downloadSize;
    [SerializeField] private TMP_Text _fileCount;

    [Header("Downloader components")] 
    [SerializeField] private CanvasGroup _progressorContainer;
    [SerializeField] private TMP_Text _totalSizeTmp;
    [SerializeField] private TMP_Text _currentDownloadTmp;
    [SerializeField] private Progressor _progressor;
    [SerializeField] private TMP_Text _statusInfoTMPText;
    
    [Header("UI components")] 
    [SerializeField] private InfiniteProgressIndicator _infiniteProgressIndicator;
    
    #region Private fields

    private CancellationTokenSource _cancelSource;

    private float _currentFileSize = 0;
    private double _curentlyDownloaded = 0;

    private float _currentProgress;
    private float _oldProgress;

    #endregion

    public override void OnShowViewFinished()
    {
      _cancelSource = new CancellationTokenSource();
      _progressorContainer.alpha = 0;
      _infiniteProgressIndicator.Show();
      _init();
      _startDownloadProcess().Forget();
      base.OnShowViewFinished();
    }

    public override void OnHideViewFinished()
    {
      _cancelSource.Cancel();
      base.OnHideViewFinished();
    }

    private void _init()
    {
      _statusInfoTMPText.text = "";
      _currentFileSize = 0;
      _curentlyDownloaded = 0;
    }

    private async UniTaskVoid _startDownloadProcess()
    {
      try
      {
        var guideInfos = await _getGuidesInfo();
        var isComplete = _downloadFiles(guideInfos);
      }
      catch (Exception e)
      {
        Dialog dialog = new Dialog("ERROR", e.Message);
        dialog.Show();
      }
    }

    private async UniTask<List<GuideInfo>> _getGuidesInfo()
    {
      List<GuideInfo> guideInfos = new List<GuideInfo>();

      if (DownloadData.Instance.DownloadCodes.Any())
      {
        Repository repository = new Repository();

        foreach (var downloadCode in DownloadData.Instance.DownloadCodes)
        {
          try
          {
            SetInfoMessage($"GETTING GUIDE JSON: {downloadCode.Code}");
            var json = await repository.GetGuideJson(downloadCode.Code);

            SetInfoMessage($"GETTING GUIDE INFO: {downloadCode.Code}");
            GuideInfo guideInfo = new GuideInfo(json);
            var f = await guideInfo.GetData();
            guideInfos.Add(guideInfo);
          }
          catch (Exception e)
          {
            throw new Exception(e.Message);
          }
        }
      }
      else
      {
        throw new Exception("NO DOWNLOAD CODES!! PLEASE INSERT DOWNLOAD CODES!");
      }
      return guideInfos;
    }

    private async UniTask<bool> _downloadFiles(List<GuideInfo> guideInfos)
    {
     
      var token = _cancelSource.Token;

      FilesEntity filesEntity = new FilesEntity();
      GuidesEntity guidesEntity = new GuidesEntity();

      foreach (var guideInfo in guideInfos)
      {
        if (guideInfo.Files.FilesToDownload.Count <= 0)
        {
          SetInfoMessage($"GUIDE UP TO DATE:{guideInfo.Guide.Name}");
          //Lets save our guide to db!
          guidesEntity.InsertUpdate(guideInfo);
          await UniTask.Delay(2000);
          continue;
        }

        //Set progressor max value based on total download size!
        SetProgressorValue(0, guideInfo.Files.DownloadSize);

        SetTotalDownloadSize(guideInfo.Files.DownloadSize);
        SetDownloadData(guideInfo);
        SetInfoMessage($"DOWNLOAD FILES:{guideInfo.Guide.Name}");
        
        _infiniteProgressIndicator.Hide();
        _progressorContainer.alpha = 1;

        bool isFail = false;

        for (int i = 0; i < guideInfo.Files.FilesToDownload.Count; i++)
        {
          var fileToDownload = guideInfo.Files.FilesToDownload[i];

          var url = Api.NOVENA_HOST + fileToDownload.Path;
          var savePath = Application.persistentDataPath + fileToDownload.Path;

          _currentFileSize = fileToDownload.Size;

          try
          {
            SetInfoMessage($"CURRENT FILE:{fileToDownload.Path}");
            await Downloader.DownloadFile(url, savePath, token, CurrentFileProgress);

            File file = new File();
            file.FilePath = fileToDownload.Path;
            file.LocalPath = savePath;
            file.TimeStamp = fileToDownload.TimeStamp;
            file.GuideId = guideInfo.Guide.Id;

            var f = filesEntity.GetByFilePath(fileToDownload.Path);

            //If file exist just update time stamp.
            if (f != null)
            {
              filesEntity.Update(file);
            }
            else
            {
              //Store to db
              filesEntity.Insert(file);
            }
          }
          catch (Exception e)
          {
            Debug.LogException(e);
            _oldProgress = 0;
            Dialog dialog = new Dialog("ERROR", e.Message);
            dialog.Show();
            throw new Exception($"{guideInfo.Guide.Name} {e.Message}");
          }

          //Reset progress
          _oldProgress = 0;
        }
        
        _curentlyDownloaded = 0;
        //Lets save our guide to db!
        guidesEntity = new GuidesEntity();
        guidesEntity.InsertUpdate(guideInfo);
      }
      
      SetInfoMessage("DOWNLOAD COMPLETE");
      
      Files.DeleteUnusedFiles().Forget();
      
      await UniTask.Delay(TimeSpan.FromSeconds(3));

      filesEntity.Dispose();
      guidesEntity.Dispose();
      
      //After download completed!
      SceneManager.LoadScene(0);

      return true;
    }

    /// <summary>
    /// Update progress for each file download
    /// </summary>
    /// <param name="progress"></param>
    private void CurrentFileProgress(float progress)
    {
      progress *= 100;
      _currentProgress = progress - _oldProgress;
      _oldProgress = progress;

      _curentlyDownloaded += (_currentFileSize * _currentProgress) / 100;

      SetProgressorValue((float)_curentlyDownloaded);
      SetCurrentDownloadSize((float)_curentlyDownloaded);
    }

    /// <summary>
    /// Set progress of download in percentage
    /// </summary>
    /// <param name="value">Current download progress</param>
    /// <param name="maxValue">Max value for percentage calculation</param>
    private void SetProgressorValue(float value, float maxValue = 0)
    {
      _progressor.SetValue(value);

      if (maxValue > 0)
      {
        //This is to set how progressor calculates percentage
        _progressor.SetMax(maxValue);
      }
    }

    /// <summary>
    /// Set TMP of current download progress in MB.
    /// </summary>
    /// <param name="value"></param>
    private void SetCurrentDownloadSize(float value)
    {
      _currentDownloadTmp.text = ToMegabytesFormat(value);
    }

    /// <summary>
    /// Set's text to total size in MB
    /// </summary>
    private void SetTotalDownloadSize(float value)
    {
      _totalSizeTmp.text = ToMegabytesFormat(value);
    }

    private void SetDownloadData(GuideInfo guideInfo)
    {
      _guideName.text = guideInfo.Guide.Name;
      /*_downloadSize.text = ToMegabytesFormat(guideInfo.Files.DownloadSize);
      _fileCount.text = guideInfo.Files.FilesToDownload.Count.ToString();*/
    }

    /// <summary>
    /// Convert number to string. Display value in Megabytes.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string ToMegabytesFormat(float value)
    {
      return ((value / 1024) / 1024).ToString("0.##") + " MB";
    }

    private void SetInfoMessage(string message)
    {
      _statusInfoTMPText.text = message;
    }
  }
}