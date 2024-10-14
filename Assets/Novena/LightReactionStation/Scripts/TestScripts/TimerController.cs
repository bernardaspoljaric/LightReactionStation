using Novena.Components.Timer;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Novena
{
  public class TimerController : MonoBehaviour
  {
    public static Action OnGameEnded;
    public static Action OnLightTimerEnded;
    [SerializeField] private Timer _gameTimer;
    [SerializeField] private TMP_Text _timerText;

    private int _gameTime;
    private int _lightTime;
    private bool _isLightTimesUp;
    private Coroutine _lightTimerCoroutine;

    private void Awake()
    {
      _gameTimer.OnTimerEnded.RemoveAllListeners();
      _gameTimer.OnTimerEnded.AddListener(OnGameEnd);

      OutputController.OnOutputTimer += StartLightTimer;
    }

    private void Start()
    {
      _gameTime = Settings.Settings.GetValue<int>("GameTime");
      _lightTime = Settings.Settings.GetValue<int>("LightTime");
      SetupTimer();
    }

    private void OnDestroy()
    {
      OutputController.OnOutputTimer -= StartLightTimer;
    }

    /// <summary>
    /// On game start.
    /// </summary>
    public void StartGameTimer()
    {
      _gameTimer.StartTimer();
    }

    /// <summary>
    /// Get information if light timer is out.
    /// </summary>
    /// <returns></returns>
    public bool IsLightTimerEnded()
    {
      return _isLightTimesUp;
    }

    /// <summary>
    /// If light is cliked in right time, stoplight timer.
    /// </summary>
    public void StopLightTimer()
    {
      if(_lightTimerCoroutine != null)
      {
        StopCoroutine(_lightTimerCoroutine);
        OnLightTimerEnded?.Invoke();
      }

      _isLightTimesUp = true;
    }

    /// <summary>
    /// Set game timer.
    /// </summary>
    private void SetupTimer()
    {
      _gameTimer.IsStopwatch = false;
      _gameTimer.TimeToCountdownFrom = _gameTime;
      _gameTimer.FormatString = "mm\\:ss";
      _timerText.text = ConvertTime();
    }

    /// <summary>
    /// On game end.
    /// </summary>
    private void OnGameEnd()
    {
      _gameTimer.StopTimer();
      OnGameEnded?.Invoke();
      Doozy.Engine.GameEventMessage.SendEvent("Back");
    }

    /// <summary>
    /// Convert game time.
    /// </summary>
    /// <returns></returns>
    private string ConvertTime()
    {
      var time = TimeSpan.FromSeconds(_gameTime).ToString(_gameTimer.FormatString);

      return time;
    }

    /// <summary>
    /// Activate light timer.
    /// </summary>
    private void StartLightTimer()
    {
      _lightTimerCoroutine = StartCoroutine(LightTimer());
    }

    private IEnumerator LightTimer()
    {
      _isLightTimesUp = false;
      yield return new WaitForSeconds(_lightTime);
      OnLightTimerEnded?.Invoke();
      _isLightTimesUp = true;
    }
  }
}
