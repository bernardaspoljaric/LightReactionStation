using System;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class Light : MonoBehaviour
  {
    public static Action OnButtonClick;
    private CanvasGroup _keyCG;
    private Image _lightImage;
    private Button _lightButton;
    private int _lightIndex;

    private void Awake()
    {
      _keyCG = GetComponentInChildren<CanvasGroup>();
      OutputController.OnSignalSend += ChangeLightColor;
      InputController.OnKeyboardInput += ShowKeyboardKey;
    }

    private void Start()
    {   
      _lightImage = GetComponent<Image>();
      _lightButton = GetComponent<Button>();

      SetLightIndex();

      _lightButton.onClick.RemoveAllListeners();
      _lightButton.onClick.AddListener(OnButtonCliked);
    }

    private void OnDestroy()
    {
      OutputController.OnSignalSend -= ChangeLightColor;
      InputController.OnKeyboardInput -= ShowKeyboardKey;
    }

    /// <summary>
    /// Change light color if light index is correct.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="lightIndex"></param>
    private void ChangeLightColor(Color color, int lightIndex)
    {
      if(lightIndex == _lightIndex)
        _lightImage.color = color;
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
      if (GameManager.Instance.GetActiveLight() != _lightIndex) return;

      OnButtonClick?.Invoke();
    }
  }
}
