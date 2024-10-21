using UnityEngine;

namespace Novena
{
  public class LevelController : MonoBehaviour
  {
    private int _levelDifficulty;
    private float _signalTimePeriod;

    private void Awake()
    {
      _levelDifficulty = Settings.Settings.GetValue<int>("LevelDifficulty");
    }
    private void Start()
    {
      CheckLevelDifficulty();
      SetSignalTimePeriod();
    }

    /// <summary>
    /// Get current level difficulty.
    /// </summary>
    /// <returns></returns>
    public int GetLevelDifficulty()
    {
      return _levelDifficulty;
    }

    /// <summary>
    /// Get current signal time period.
    /// </summary>
    /// <returns></returns>
    public float GetSignalTimePeriod()
    {
      return _signalTimePeriod;
    }

    /// <summary>
    /// Check saved level difficulty -- 0-easy, 1-medium, 2- hard; if wrong value is entered do automatic set.
    /// </summary>
    private void CheckLevelDifficulty()
    {
      if(_levelDifficulty < 0)
        _levelDifficulty = 0;
      else if(_levelDifficulty > 2)
        _levelDifficulty = 2;
    }

    /// <summary>
    /// Harcoded signal periods - how much time should pass between light activation.
    /// </summary>
    private void SetSignalTimePeriod()
    {
      if (_levelDifficulty == 0)
        _signalTimePeriod = 7f;
      else if (_levelDifficulty == 1)
        _signalTimePeriod = 5f;
      else
        _signalTimePeriod = 3f;
    }
  }
}
