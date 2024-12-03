using Novena.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Novena.Players;

namespace Novena
{
  public class GameTestController : MonoBehaviour
  {
    public static Action<string> OnStart;
    [SerializeField] private RectTransform _gameBodyRT;
    [SerializeField] private CanvasGroup _startCG;
    [SerializeField] private Button _startButton;
    private List<LightTest> _lightList;
    private int _activeLight;

    private void Awake()
    {

      UdpTestController.OnLightColorChange += ChangeLightColor;
      UdpTestController.OnGameEnd += OnGameEnd;

      _startButton.onClick.RemoveAllListeners();
      _startButton.onClick.AddListener(OnStartClicked);
    }
    private void Start()
    {
      GetLights();
    }

    private void OnDestroy()
    {
      UdpTestController.OnLightColorChange -= ChangeLightColor;
      UdpTestController.OnGameEnd -= OnGameEnd;
    }

    public int GetActiveLight()
    {
      return _activeLight;
    }

    private void GetLights()
    {
      _lightList = new List<LightTest>();
      var childNumber = _gameBodyRT.childCount;

      for (int i = 0; i < childNumber; i++)
      {
        _lightList.Add(_gameBodyRT.GetChild(i).GetComponent<LightTest>());
      }
    }

    private void ChangeLightColor(string udpCode)
    {
      var codeSplit = udpCode.Split('_');
      int lightIndex = Int32.Parse(codeSplit[0]);
      Color color = Color.white;

      if (udpCode.Contains("Player"))
      {
        Player currentPlayer = Players.GetPlayer(codeSplit[1]);
        color = Players.GetPlayerColor(currentPlayer);
      }

      _lightList[lightIndex].ChangeLightColor(color);
      _activeLight = lightIndex;
    }

    private void ShowStart(bool show)
    {
      CanvasGroupHelper.FadeCanvasGroup(_startCG, show, 0.4f);
    }

    private void OnStartClicked()
    {
      ShowStart(false);
      OnStart?.Invoke("start");
    }

    private void OnGameEnd()
    {
      ShowStart(true);

      for (int i = 0; i < _lightList.Count; i++)
      {
        _lightList[i].ChangeLightColor(Color.white);
      }

      _activeLight = -1;
    }
  }
}
