using Cysharp.Threading.Tasks;
using Novena.Utility.Interface;
using UnityEngine;

//Author: GoGs
public class AnalyticsManager : MonoBehaviour, IInitialize
{
  [Tooltip("Enable only for test purposes!")]
  [SerializeField]
  private bool EnableInEditor = false;

  private static Analytics Analytics;
  private static bool isInitialized = false;
  private void Awake()
  {
    //Check if we already have AnalyticsManager.
    if (Analytics == null)
    {
      new AnalyticsHelper(EnableInEditor);
      Analytics = new Analytics();
      DontDestroyOnLoad(this);

      //Lets send all analytics that we have before we start.
      //In case that app was crashed or closed before we could send analytics on quit.
      Send();

      return;
    }

    //We don't need more than one AnalyticsManager.
    Destroy(gameObject);
  }

  //This is called from InitController after guide is loaded.
  public void Initialize()
  {
    //If isInitialized is false that means that this is first time that we are loading guide.
    //This will indicate that app is turned on and not scene loaded when we return from any other scene.
    if (isInitialized == false)
    {
      //Create an Analytic event that app is turned on.
      //We can call this only when guide id is known in Data.Guide!
      Analytics.Create(new AnalyticEvent(AnalyticType.appTurnedOn));
      Debug.Log("AnalyticsManager => Initialize: appTurnedOn");
      isInitialized = true;
      return;
    }
  }

  /// <summary>
  /// Send all analytics that we have.
  /// </summary>
  public void Send()
  {
    Analytics.Send().Forget();
  }

  private async void OnApplicationQuit()
  {
    Analytics.Create(new AnalyticEvent(AnalyticType.appTurnedOff));
    //Lets send all analytics that we have before we quit.
    await Analytics.Send();
    Analytics.Dispose();
  }
}
