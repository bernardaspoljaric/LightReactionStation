using DG.Tweening;
using System;
using UnityEngine;

namespace Novena
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;
    public static Action<bool> OnErrorHappened;

    [SerializeField] private TimerController _timerController;

    private int _numberOfPlayers;
    private int _levelDifficulty;
    
    private int _lightTime;
    

    private void Awake()
    {
      if (Instance == null)
        Instance = this;

      // test listener to start button clicked
      HomeController.OnStartClicked += StartGame;

    }
    private void Start()
    {
      ReadSettings();
    }

    private void OnDestroy()
    {
      HomeController.OnStartClicked += StartGame;
    }

    public void ErrorHappened()
    {
      OnErrorHappened?.Invoke(true);
    }

    private void ReadSettings()
    {
      _numberOfPlayers = Settings.Settings.GetValue<int>("PlayerNumber");
      _levelDifficulty = Settings.Settings.GetValue<int>("LevelDifficulty");
      _lightTime = Settings.Settings.GetValue<int>("LightTime");
      
    }

    private void StartGame()
    {
      // test on camputer screen
      Doozy.Engine.GameEventMessage.SendEvent("StartGame");
      DOVirtual.DelayedCall(0.5f, () => { _timerController.StartGameTimer(); });
    }
  }
}
