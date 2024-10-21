using System;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class LightTest : MonoBehaviour
  {
    public static Action OnButtonClick;

    [SerializeField] private GameTestController _gameTestController;
    private Image _lightImage;
    private Button _lightButton;
    private int _lightIndex;

    private void Start()
    {
      _lightImage = GetComponent<Image>();
      _lightButton = GetComponent<Button>();
      SetLightIndex();
      _lightButton.onClick.RemoveAllListeners();
      _lightButton.onClick.AddListener(OnButtonCliked);
    }
    public void ChangeLightColor(Color color)
    {
        _lightImage.color = color;
    }

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
