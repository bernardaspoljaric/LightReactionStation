using System;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
  public class HomeController : MonoBehaviour
  {
    public static Action OnStartClicked;
    [SerializeField] private Button _startButton;
    [SerializeField] private CanvasGroup _startContainerCG;
    [SerializeField] private CanvasGroup _udpContainerCG;

    private void Awake()
    {
      GameManager.OnErrorHappened += ActivateStartButton;
      InputController.OnUdpInput += SetForUdp;
      InputController.OnKeyboardInput += SetStart;
      InputController.OnTouchInput += SetStart;
    }
    private void Start()
    {
      _startButton.onClick.RemoveAllListeners();
      _startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
    }

    private void OnDestroy()
    {
      GameManager.OnErrorHappened -= ActivateStartButton;
      InputController.OnUdpInput -= SetForUdp;
      InputController.OnKeyboardInput -= SetStart;
      InputController.OnTouchInput -= SetStart;
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

    private void SetForUdp()
    {
      _startContainerCG.alpha = 0;
      _startContainerCG.interactable = false;
      _startContainerCG.blocksRaycasts = false;

      _udpContainerCG.alpha = 1;
    }

    private void SetStart()
    {
      _startContainerCG.alpha = 1;
      _startContainerCG.interactable = true;
      _startContainerCG.blocksRaycasts = true;

      _udpContainerCG.alpha = 0;
    }
  }
}
