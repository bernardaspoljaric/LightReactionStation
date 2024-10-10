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
    [SerializeField] private OutputController _outputController;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InputController _inputController;
    [SerializeField] private LevelController _levelController;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;

      // test listener to start button clicked
      HomeController.OnStartClicked += StartGame;
      Light.OnButtonClick += OnLightClicked;

      TimerController.OnGameEnded += GameEnd;

    }

    private void OnDestroy()
    {
      HomeController.OnStartClicked -= StartGame;
      Light.OnButtonClick += OnLightClicked;

      TimerController.OnGameEnded -= GameEnd;
    }

    /// <summary>
    /// If there is some problem with game configuration send event for error and disable start game.
    /// </summary>
    public void ErrorHappened()
    {
      OnErrorHappened?.Invoke(true);
    }

    /// <summary>
    /// Get choosen input option - touch/click, keyboard or udp.
    /// </summary>
    /// <returns></returns>
    public int GetInputOption()
    {
      return _inputController.GetInput();
    }

    /// <summary>
    /// Get how many players are playing.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerNumber()
    {
      return _playerController.GetPlayerNumber();
    }

    /// <summary>
    /// Get level difficulty - easy, medium, hard.
    /// </summary>
    /// <returns></returns>
    public int GetDifficluty()
    {
      return _levelController.GetLevelDifficulty();
    }

    /// <summary>
    /// Geet currently active light.
    /// </summary>
    /// <returns></returns>
    public int GetActiveLight()
    {
      return _outputController.GetActiveLight();
    }

    /// <summary>
    /// Set currently active player.
    /// </summary>
    /// <param name="index"></param>
    public void SetActivePlayer(int index)
    {
      _playerController.SetActivePlayer(index);
    }

    public int GetSignalTimePeriod()
    {
      return _levelController.GetSignalTimePeriod();
    }

    /// <summary>
    /// When user clicks on light.
    /// </summary>
    private void OnLightClicked()
    {
      var player = _playerController.GetActivePlayer();
      if (!_timerController.IsLightTimerEnded())
      {
        player.GetComponent<ScoreController>().AddPoint();
        _timerController.StopLightTimer();
      }
    }

    private void StartGame()
    {
      // test on camputer screen
      Doozy.Engine.GameEventMessage.SendEvent("StartGame");

      _playerController.ResetPlayers();
      DOVirtual.DelayedCall(0.5f, () => { _timerController.StartGameTimer(); });
      _outputController.StartGame();
    }

    private void GameEnd()
    {
      _outputController.StopGame();
      // test on camputer screen
      Doozy.Engine.GameEventMessage.SendEvent("Back");

      //TODO: show last game result
    }
  }
}
