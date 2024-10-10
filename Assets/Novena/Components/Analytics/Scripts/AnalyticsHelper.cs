using UnityEngine;

public class AnalyticsHelper
{
  private static bool _enabledInEditor = false;
  public AnalyticsHelper(bool enabledInEditor)
  {
    _enabledInEditor = enabledInEditor;

#if UNITY_EDITOR
    if (_enabledInEditor)
    {
      Debug.LogWarning("Analytics enabled in editor!");
    }
#endif
  }

  /// <summary>
  /// Checks if we can use analytics.
  /// In editor we can use analytics only if we have enabled it in AnalyticsManager.
  /// In build we can use analytics only if we are not in debug build (Build Settings => Development Build checkmark set to false).
  /// </summary>
  /// <returns></returns>
  public static bool CanUseAnalytics()
  {
#if UNITY_EDITOR
    if (_enabledInEditor)
    {
      return true;
    }

    return false;
#else
    //If we are in debug build, we don't want to send analytics
    if (Debug.isDebugBuild == true) return false;
    return true;
#endif
  }
}
