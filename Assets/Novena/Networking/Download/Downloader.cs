using System;
using System.IO;
using System.Text;
using System.Threading;
using CI.HttpClient;
using Cysharp.Threading.Tasks;
using Novena.Controllers;
using Novena.Exceptions;
using UnityEngine;
using UnityEngine.Networking;

namespace Novena.Networking.Download
{
  public static class Downloader
  {
    public static Action OnDownloadComplete;
    
    public static void DownloadGuide()
    {
      LoadingController.Instance.SetStatus("Downloading");
      Download();
    }

    /// <summary>
    /// Download and save file to location provided.
    /// </summary>
    /// <param name="url">Url to file</param>
    /// <param name="saveFilePath">Where to save file</param>
    /// <param name="token">Cancellation token</param>
    /// <param name="downloadProgressAction">Action to return download progress</param>
    /// <returns>true if download success</returns>
    /// <exception cref="NetworkException">If any error in download</exception>
    public static async UniTask<bool> DownloadFile(string url, string saveFilePath, CancellationToken token, Action<float> downloadProgressAction)
    {
      var request = new UnityWebRequest(url);
      var downloadHandler = new DownloadHandlerFile(saveFilePath);

      request.downloadHandler = downloadHandler;
      
      //This is part of UniTask to get download progress
      var progress = Progress.Create<float>(x =>
      {
        downloadProgressAction(x);
      });
      
      try
      {
        var response = await request.SendWebRequest().ToUniTask(progress, PlayerLoopTiming.Update, token);

        if (response.result == UnityWebRequest.Result.Success)
        {
          return true;
        }
        
        throw new NetworkException(message: response.ToString());
      }
      catch (Exception e)
      {
        throw new NetworkException(message: e.Message);
      }
    }
 
    /// <summary>
    /// Start file download
    /// </summary>
    private static void Download()
    {
      //Get current data.json
      string timeStampJson = Novena.Networking.Download.PartialUpdate.PartialUpdate.GetJson();
      
      //Save current data.json to FileState key
      PlayerPrefs.SetString($"FileState", timeStampJson);
      
      byte[] data = Encoding.UTF8.GetBytes(timeStampJson);
      
      HttpClient httpClient = new HttpClient();
      
      var multiform = new MultipartFormDataContent();
      var fileContent = new ByteArrayContent(data, "application/json");
      multiform.Add(fileContent, "json", "file.json");

      string downloadPath = Api.GetGuidePath() + ".zip";
      
      var fs = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
      string url = Api.ApiPartialUpdate + DataLoading.DataLoading.DownloadCode;

      httpClient.Post(new Uri(url), multiform, HttpCompletionOption.StreamResponseContent,
        r =>
        {
          if (r.Exception != null)
          {
            Debug.LogError(r.Exception.Message);
            httpClient.Abort();
            //Navigator.To("Admin");
          }

          if (r.IsSuccessStatusCode == false)
          {
            Debug.LogError(r.ReasonPhrase + " : " + r.ReadAsString());
            httpClient.Abort();
            //Navigator.To("Admin");
          }
          else
          {
            var stream = r.ReadAsStream();
            stream.CopyTo(fs);
        
            LoadingController.Instance.UpdateLoadingState(r.PercentageComplete, r.TotalContentRead, r.ContentLength);
        
            if (r.ContentLength == r.TotalContentRead)
            {
              fs.Dispose();
              OnDownloadComplete?.Invoke();
            }
          }
        });
    }
  }
}