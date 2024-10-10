using Novena.Utility.Application;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novena.Admin
{
  public class AdminUtility : MonoBehaviour
  {
    public void ShutDown()
    {
      CloseApp();
    }

    public void Restart()
    {
      ApplicationManager.RestartApplication();
    }

    public void ExitAdmin()
    {
      SceneManager.LoadScene(0);
    }

    private void CloseApp()
    {
      Application.Quit();
    }
  }
}