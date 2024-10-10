using System.Linq;
using Doozy.Engine;
using Novena.DAL;
using Novena.DAL.Entity;
using Novena.UiUtility.Base;
using Novena.Utility.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novena.Controllers
{
  public class InitController : UiController
  {
    public override void OnShowViewFinished()
    {
      CheckGuidesState();
      base.OnShowViewFinished();
    }
    
    /// <summary>
    /// Checks for downloaded guides.
    /// </summary>
    private void CheckGuidesState()
    {
      GuidesEntity guidesEntity = new GuidesEntity();

      var localGuides = guidesEntity.GetAll();

      guidesEntity.Dispose();

      if (localGuides.Count <= 0)
      {
        SceneManager.LoadScene(1);
        return;
      }

      //Lets load our guides
      Data.LoadContent(localGuides);

      GameEventMessage.SendEvent("GoToHome");

      Initialize();
    }

    private void Initialize()
    {
      var inits = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IInitialize>();

      foreach (var init in inits)
      {
        init.Initialize();
      }
    }
  }
}