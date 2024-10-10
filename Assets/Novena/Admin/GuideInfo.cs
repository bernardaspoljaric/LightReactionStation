using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Novena.Admin.FileSave;
using Novena.DAL.Entity;
using Novena.DAL.Model.Guide;
using UnityEngine;

namespace Novena.Admin
{
  public class GuideInfo
  {
    public Guide Guide { get; set; }
    public string GuideJson { get; set; }
    public Files Files { get; set; }
    public int IsActive { get; set; }
    public bool IsDownloadRequired { get; set; }
    public bool IsNewGuide { get; set; }
    
    
    public GuideInfo(string guideJson)
    {
      var guide = JsonConvert.DeserializeObject<Guide>(guideJson);
      this.GuideJson = guideJson;
      this.Guide = guide;
    }

    public async UniTask<bool> GetData()
    {
      try
      {
        Files files = new Files(Guide);
        var f = await files.GetFilesAsync();
        
        bool active = true;
        bool downloadRequired = false;
        //Is guide new on device
        bool isNew = true;

        GuidesEntity guidesEntity = new();

        var localGuide = guidesEntity.GetGuideById(Guide.Id);

        guidesEntity.Dispose();

        if (localGuide != null)
        {
          isNew = false;
          
          //We have guide on device
          //Let's compare it's json with existing json
          downloadRequired = !String.Equals(GuideJson, localGuide.Json);
          active = localGuide.Active;
        }
        
        if (files.FilesToDownload.Any())
        {
          downloadRequired = true;
        }

        this.Files = files;
        this.IsActive = active ? 1:0;
        this.IsDownloadRequired = downloadRequired;
        this.IsNewGuide = isNew;

        return true;
      }
      catch (Exception e)
      {
        Debug.LogException(e);
        return false;
      }
    }
  }
}