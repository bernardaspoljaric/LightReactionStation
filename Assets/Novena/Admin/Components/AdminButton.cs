using UnityEngine;
using UnityEngine.SceneManagement;

namespace Novena.Admin.Components
{
  public class AdminButton : MonoBehaviour
  {
    public void LoadAdminScene()
    {
      SceneManager.LoadScene(1);
    }
  }
}