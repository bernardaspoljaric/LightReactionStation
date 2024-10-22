using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class LightController : MonoBehaviour
  {
    public static Action<LightController> OnButtonClick;
    private CanvasGroup _keyCG;
    private Image _lightImage;
    private Button _lightButton;
    private int _lightIndex;
    private bool _isActive;
    private Coroutine _lightTimerCoroutine;
    private int _lightTime;
    private Players.Player _player;

    private void Awake()
    {
      _keyCG = GetComponentInChildren<CanvasGroup>();
      OutputController.OnSignalSendOption += ChangeLightColor;
      InputController.OnKeyboardInput += ShowKeyboardKey;
    }

    private void Start()
    {
      _lightImage = GetComponent<Image>();
      _lightButton = GetComponent<Button>();
      _lightTime = Settings.Settings.GetValue<int>("LightTime");

      SetLightIndex();

      _lightButton.onClick.RemoveAllListeners();
      _lightButton.onClick.AddListener(OnButtonCliked);
    }

    private void OnDestroy()
    {
      OutputController.OnSignalSendOption -= ChangeLightColor;
      InputController.OnKeyboardInput -= ShowKeyboardKey;
    }

    public bool IsActiveLight()
    {
      return _isActive;
    }

    public Players.Player GetLightsPlayer()
    {
      return _player;
    }

    public void StopLightTimer()
    {
      StopCoroutine(_lightTimerCoroutine);
      ChangeLightColor(Color.white, _lightIndex, _player);
    }

    /// <summary>
    /// Change light color if light index is correct.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="lightIndex"></param>
    private void ChangeLightColor(Color color, int lightIndex, Players.Player player)
    {
      if (lightIndex == _lightIndex)
      {
        if (color != Color.white)
        {
          _player = player;
          _isActive = true;
          StartLightTimer();
        }
        else
          _isActive = false;
        
        _lightImage.color = color;
      }
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
      yield return new WaitForSeconds(_lightTime);
      ChangeLightColor(Color.white, _lightIndex, _player);
    }

    /// <summary>
    /// Show text helpers on lights.
    /// </summary>
    private void ShowKeyboardKey()
    {
      _keyCG.alpha = 1.0f;
    }

    /// <summary>
    /// Set this light's index.
    /// </summary>
    private void SetLightIndex()
    {
      var lightName = gameObject.name.Split("_");
      _lightIndex = Int32.Parse(lightName[1]);
    }

    private void OnButtonCliked()
    {
      if (!_isActive) return;

      OnButtonClick?.Invoke(this);
    }
  }
}
