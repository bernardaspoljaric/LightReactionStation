using System;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class HomeController : MonoBehaviour
  {
    public static Action OnStartClicked;
    [SerializeField] private Button _startButton;

    private void Awake()
    {
      GameManager.OnErrorHappened += ActivateStartButton;
    }
    private void Start()
    {
      _startButton.onClick.RemoveAllListeners();
      _startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
    }

    private void ActivateStartButton(bool isDeactivate)
    {
      if (isDeactivate)
      {
        _startButton.interactable = false;
        _startButton.GetComponent<Image>().color = Color.gray;
      }
      else
      {
        _startButton.interactable = true;
        _startButton.GetComponent<Image>().color = Color.green;
      }
    }
  }
}
