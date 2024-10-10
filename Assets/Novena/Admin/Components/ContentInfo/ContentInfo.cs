using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Novena.Components.Dialog;
using Novena.DAL.Entity;
using Novena.DAL.Model.Guide;
using TMPro;
using UnityEngine;

namespace Novena.Admin.Components.ContentInfo
{
  public class ContentInfo : MonoBehaviour
  {
    [SerializeField] private TMP_Text _contentInfoTmp;

    private void OnEnable()
    {
      GetContentInfo();
    }

    void GetContentInfo()
    {
      GuidesEntity guidesEntity = new GuidesEntity();
      var guides = guidesEntity.GetAll();
      guidesEntity.Dispose();

      if (guides.Count <= 0)
      {
        _contentInfoTmp.text = "NO CONTENT AVAILABLE.";
        return;
      }

      StringBuilder stringBuilder = new StringBuilder();

      stringBuilder.AppendLine("DOWNLOADED CONTENT: ");
      stringBuilder.AppendLine();

      foreach (var guide in guides)
      {
        try
        {
          var g = JsonConvert.DeserializeObject<Guide>(guide.Json);
          stringBuilder.AppendLine($"{g.Name}");
        }
        catch (Exception e)
        {
          Debug.LogException(e);
        }
      }

      _contentInfoTmp.text = stringBuilder.ToString();
    }

    /// <summary>
    /// Delete all content!
    /// </summary>
    public void DeleteContent()
    {
      DialogButton dialogButton = new DialogButton();
      dialogButton.Label = "YES!";
      dialogButton.Action = _delete;
      
      Dialog dialog = new Dialog("DELETE ALL CONTENT!", "ARE YOU SURE?", new List<DialogButton>{dialogButton});
    }

    private void _delete()
    {
      using (GuidesEntity guidesEntity = new GuidesEntity())
      {
        guidesEntity.DeleteAll();
      }

      using (FilesEntity filesEntity = new FilesEntity())
      {
        filesEntity.DeleteAll();
      }      

      try
      {
        string directoryPath = Application.persistentDataPath + "/files";
        Directory.Delete(directoryPath, true);
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }

      GetContentInfo();
    }
  }
}