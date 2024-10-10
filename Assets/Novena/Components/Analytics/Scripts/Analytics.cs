using Cysharp.Threading.Tasks;

public class Analytics
{
  /// <summary>
  /// Singleton instance.
  /// </summary>
  private static Analytics _instance;

  private AnalyticsRepository _repository;

  private bool _enabled;

  public Analytics()
  {
    _enabled = AnalyticsHelper.CanUseAnalytics();

    if (_instance == null)
    {
      _instance = this;
    }

    if (_enabled == false) return;

    _repository = new AnalyticsRepository();
  }

  /// <summary>
  /// Creates an Analytic event.
  /// </summary>
  /// <param name="e">The Analytic event that contains data <see cref="AnalyticEvent"/></param>
  /// <example>
  /// Example usage:
  /// <code>
  /// Analytics.Create(new AnalyticEvent(AnalyticType.langOpen, "1"));
  /// </code>
  /// </example>
  public static void Create(AnalyticEvent e)
  {
    if (_instance._enabled == false) return;
    if (e.Type == AnalyticType.none)
    {
      return;
    }
    _instance._repository.Insert(e);
  }

  public async UniTask Send()
  {
    if (_instance._enabled == false) return;
    await _repository.Send();
  }

  public void Dispose()
  {
    if (_instance._enabled == false) return;
    _instance = null;
    _repository.Dispose();
  }
}
