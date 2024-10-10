using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class TouchController : MonoBehaviour
  {
    [SerializeField] private RectTransform _gameBodyRT;
    private List<Button> _lightButtons;
    private void OnEnable()
    {
      GetLightButtons();
      EnableTouchButtons(true);
    }

    private void OnDisable()
    {
      EnableTouchButtons(false);
    }

    private void GetLightButtons()
    {
      _lightButtons = new List<Button>();
      for (int i = 0; i < _gameBodyRT.childCount; i++)
      {
        _lightButtons.Add(_gameBodyRT.GetChild(i).GetComponent<Button>());
      }
    }

    private void EnableTouchButtons(bool isEnabled)
    {
      for (int i = 0; i < _lightButtons.Count; i++)
      {
        _lightButtons[i].interactable = isEnabled;
      }
    }
  }
}
