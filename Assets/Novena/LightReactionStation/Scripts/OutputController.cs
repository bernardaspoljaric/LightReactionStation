using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novena
{
  public class OutputController : MonoBehaviour
  {
    public static Action<Color, int> OnSignalSend;
    public static Action<string> OnUdpSignalSend;
    public static Action OnOutputTimer;

    // only for computer test
    [SerializeField] private RectTransform _gameBodyRT;
    
    private float _signalTimePeriod;
    private List<Light> _lightList;
    private Coroutine _outputCoroutine;

    private int _playerNumber;
    private int _roundCount = 1;
    private int _activeLight;

    private void Awake()
    {
      TimerController.OnLightTimerEnded += OnSignalEnd;
    }

    private void Start()
    {
      GetLights();

      _playerNumber = GameManager.Instance.GetPlayerNumber();
      _signalTimePeriod = GameManager.Instance.GetSignalTimePeriod();
    }

    private void OnDestroy()
    {
      TimerController.OnLightTimerEnded -= OnSignalEnd;
    }

    /// <summary>
    /// Starts game depending on input option.
    /// </summary>
    public void StartGame()
    {
        SendSignal();
    }

    /// <summary>
    /// Stops game's coroutine.
    /// </summary>
    public void StopGame()
    {
      StopCoroutine(_outputCoroutine);
    }

    /// <summary>
    /// Get currently active button.
    /// </summary>
    /// <returns></returns>
    public int GetActiveLight()
    {
      return _activeLight;
    }

    /// <summary>
    /// Get all test lights created in Unity editor/Game/Body.
    /// </summary>
    private void GetLights()
    {
      _lightList = new List<Light>();
      var childNumber = _gameBodyRT.childCount;

      for (int i = 0; i < childNumber; i++)
      {
        _lightList.Add(_gameBodyRT.GetChild(i).GetComponent<Light>());
      }
    }

    /// <summary>
    /// Send siganl for light activation via coroutine.
    /// </summary>
    private async void SendSignal()
    {
      await UniTask.Delay(200);
      _outputCoroutine = StartCoroutine(SignalTime());
    }

    /// <summary>
    /// Signal coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SignalTime()
    {
      var timePeriod = GameManager.Instance.GetSignalTimePeriod();
      while (true)
      {
        yield return new WaitForSeconds(_signalTimePeriod);

        if (GameManager.Instance.GetDifficluty() > 0)
          //_signalTimePeriod = UnityEngine.Random.Range(1f, timePeriod);

        _activeLight = UnityEngine.Random.Range(0, _lightList.Count);
        if (_playerNumber != 1)
        {
          SetActivePlayer();
          _roundCount++;
          if (_roundCount > _playerNumber)
            _roundCount = 1;
        }
        else
        {
          if (GameManager.Instance.GetInputOption() == 2)
            OnUdpSignalSend?.Invoke(_activeLight + "_Player1" + "_Light");
          else
            OnSignalSend?.Invoke(Players.GetPlayerColor(Players.Player.Player1), _activeLight);
        }

        OnOutputTimer?.Invoke();
      }    
    }

    /// <summary>
    /// Turn off active light.
    /// </summary>
    private void OnSignalEnd()
    {
      if (GameManager.Instance.GetInputOption() != 2)
        OnSignalSend?.Invoke(Color.white, _activeLight);
      else
        OnUdpSignalSend?.Invoke(_activeLight + "_white" + "_Light");
    }

    /// <summary>
    /// If there is more than one player - send signal depending on active player.
    /// </summary>
    private void SetActivePlayer()
    {
      var inputOption = GameManager.Instance.GetInputOption();
      switch (_roundCount) 
      {
        case 1:
          if(inputOption != 2)
            OnSignalSend?.Invoke(Players.GetPlayerColor(Players.Player.Player1), _activeLight);
          else
            OnUdpSignalSend?.Invoke(_activeLight + "_Player1" + "_Light");

          GameManager.Instance.SetActivePlayer(0);
          break;
        case 2:
          if (inputOption != 2)
            OnSignalSend?.Invoke(Players.GetPlayerColor(Players.Player.Player2), _activeLight);
          else
            OnUdpSignalSend?.Invoke(_activeLight + "_Player2" + "_Light");

          GameManager.Instance.SetActivePlayer(1);
          break;
        case 3:
          if (inputOption != 2)
            OnSignalSend?.Invoke(Players.GetPlayerColor(Players.Player.Player3), _activeLight);
          else
            OnUdpSignalSend?.Invoke(_activeLight + "_Player3" + "_Light");

          GameManager.Instance.SetActivePlayer(2);
          break;
        case 4:
          if (inputOption != 2)
            OnSignalSend?.Invoke(Players.GetPlayerColor(Players.Player.Player4), _activeLight);
          else
            OnUdpSignalSend?.Invoke(_activeLight + "_Player4" + "_Light");

          GameManager.Instance.SetActivePlayer(3);
          break;
      }
    }
  }
}
