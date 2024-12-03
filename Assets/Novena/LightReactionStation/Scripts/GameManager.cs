using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Novena
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;
    public static Action<bool> OnErrorHappened;
    public static Action<string> OnGameEnd;
    public bool IsOtherOption;

    [SerializeField] private TMP_Text _resultText;

    [SerializeField] private TimerController _timerController;
    [SerializeField] private OutputController _outputController;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InputController _inputController;
    [SerializeField] private LevelController _levelController;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;

      Settings.Settings.LoadSettings();

      HomeController.OnStartClicked += StartGame;

      if (IsOtherOption)
      {
        LightController.OnButtonClick += OnLightClickedOption;
        KeyboardController.OnKeyClicked += OnKeyboardClickOption;
      }
      else
      {
        Light.OnButtonClick += OnLightClicked;
        KeyboardController.OnKeyClicked += OnKeyboardClick;
      }
      
      TimerController.OnGameEnded += GameEnd;   
    }

    private void OnDestroy()
    {
      HomeController.OnStartClicked -= StartGame;
      Light.OnButtonClick -= OnLightClicked;
      LightController.OnButtonClick -= OnLightClickedOption;

      TimerController.OnGameEnded -= GameEnd;
      KeyboardController.OnKeyClicked -= OnKeyboardClick;
      KeyboardController.OnKeyClicked -= OnKeyboardClickOption;
    }

    #region Public helper methods
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

    public float GetSignalTimePeriod()
    {
      return _levelController.GetSignalTimePeriod();
    }

    #endregion

    /// <summary>
    /// Starts a game depending on choosen input.
    /// </summary>
    public void StartGame()
    {
      // test on camputer screen
      if (_inputController.GetInput() != 2)
        Doozy.Engine.GameEventMessage.SendEvent("StartGame");

      _playerController.ResetPlayers();
      DOVirtual.DelayedCall(0.5f, () => { _timerController.StartGameTimer(); });
      _outputController.StartGame();
    }

    #region Original game logic - using Light.cs
    /// <summary>
    /// When user clicks on light.
    /// </summary>
    public void OnLightClicked()
    {
      var player = _playerController.GetActivePlayer();
      if (!_timerController.IsLightTimerEnded())
      {
        player.GetComponent<ScoreController>().AddPoint();
        _timerController.StopLightTimer();
      }
      else
      {
        _timerController.StopLightTimer();
      }
    }

    /// <summary>
    /// When user clicks key on keyboard check if right key is clicked and add points.
    /// </summary>
    /// <param name="lightReferece"></param>
    private void OnKeyboardClick(int lightReferece)
    {
      var player = _playerController.GetActivePlayer();
      if (lightReferece == GetActiveLight())
      {
        if (!_timerController.IsLightTimerEnded())
        {
          player.GetComponent<ScoreController>().AddPoint();
          _timerController.StopLightTimer();
        }
      }
      else
      {
        _timerController.StopLightTimer();
      }
    }
    #endregion

    #region Option game logic - using LightController.cs
    /// <summary>
    /// When user clicks on light - using LightController.
    /// </summary>
    public void OnLightClickedOption(LightController lightController)
    {
      if (lightController.IsActiveLight())
      {
        var playerName = lightController.GetLightsPlayer();
        var playerIndex = Int32.Parse(playerName.ToString().Substring(playerName.ToString().Length - 1));
        var player = _playerController.GetPlayer(playerIndex - 1);
        player.GetComponent<ScoreController>().AddPoint();
      }

      lightController.StopLightTimer();
    }

    /// <summary>
    /// When user clicks on keyboard check if light is active and set points to player - using LightController.cs.
    /// </summary>
    /// <param name="lightReferece"></param>
    private void OnKeyboardClickOption(int lightReferece)
    {
      var lightController = GameObject.Find("Light_" + lightReferece).GetComponent<LightController>();

      if (lightController.IsActiveLight())
      {
        var playerName = lightController.GetLightsPlayer();
        var playerIndex = Int32.Parse(playerName.ToString().Substring(playerName.ToString().Length - 1));
        var player = _playerController.GetPlayer(playerIndex - 1);
        player.GetComponent<ScoreController>().AddPoint();
      }

      lightController.StopLightTimer();
    }
    #endregion

    private void GameEnd()
    {
      _outputController.StopGame();

      // test on camputer screen
      if (_inputController.GetInput() != 2)
      {
        Doozy.Engine.GameEventMessage.SendEvent("Back");
        ShowLastGameResult();
      }
      else
      {
        OnGameEnd?.Invoke("end");
      }
    }

    private void ShowLastGameResult()
    {
      var playerNumber = GetPlayerNumber();
      string text = "";

      for (int i = 0; i < playerNumber; i++)
      {
        var player = _playerController.GetPlayer(i);
        text += "Player" + " " + i.ToString() + ": " + player.GetComponent<ScoreController>().GetScore() + "\n";
      }

      _resultText.text = text;
    }
  }
}
