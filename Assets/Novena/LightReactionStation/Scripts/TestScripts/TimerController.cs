using Novena.Components.Timer;
using System;
using TMPro;
using UnityEngine;

namespace Novena
{
  public class TimerController : MonoBehaviour
  {
    public static Action OnGameEnded;
    [SerializeField] private Timer _timer;
    [SerializeField] private TMP_Text _timerText;

    private int _gameTime;

    private void Awake()
    {
      _timer.OnTimerEnded.RemoveAllListeners();
      _timer.OnTimerEnded.AddListener(OnGameEnd);
    }

    private void Start()
    {
      _gameTime = Settings.Settings.GetValue<int>("GameTime");
      SetupTimer();
    }

    public void StartGameTimer()
    {
      _timer.StartTimer();
    }

    private void OnGameEnd()
    {
      _timer.StopTimer();
      OnGameEnded?.Invoke();
      Doozy.Engine.GameEventMessage.SendEvent("Back");
    }

    private void SetupTimer()
    {
      _timer.IsStopwatch = false;
      _timer.TimeToCountdownFrom = _gameTime;
      _timer.FormatString = "mm\\:ss";
      _timerText.text = ConvertTime();
    }

    private string ConvertTime()
    {
      var time = TimeSpan.FromSeconds(_gameTime).ToString(_timer.FormatString);

      return time;
    }
  }
}
