using System;
using UnityEngine;

namespace Novena
{
  public class InputController : MonoBehaviour
  {
    public static Action OnKeyboardInput;

    // only for test, because we have three input options
    public static Action OnUdpInput;
    public static Action OnTouchInput;

    [SerializeField] private TouchController _touchController;
    [SerializeField] private KeyboardController _keyboardController;
    [SerializeField] private UdpController _udpController;

    private int _input;
    private void Awake()
    {
      _input = Settings.Settings.GetValue<int>("Input");
    }

    private void Start()
    {
      SetInput();
    }

    /// <summary>
    /// Get choosen input.
    /// </summary>
    /// <returns></returns>
    public int GetInput()
    {
      return _input;
    }

    /// <summary>
    /// Set game for choosen input.
    /// </summary>
    private void SetInput()
    {
      if (_input == 0)
      {
        _touchController.enabled = true;
        _keyboardController.enabled = false;
        _udpController.enabled = false;

        OnTouchInput?.Invoke();
      }
      else if(_input == 1)
      {
        _touchController.enabled = false;
        _keyboardController.enabled = true;
        _udpController.enabled = false;

        OnKeyboardInput?.Invoke();
      }
      else if (_input == 2)
      {
        _touchController.enabled = false;
        _keyboardController.enabled = false;
        _udpController.enabled = true;

        OnUdpInput?.Invoke();
      }
      else
      {
        Debug.Log("Input doesn't exist.");
        GameManager.Instance.ErrorHappened();
      }

    }
  }
}
