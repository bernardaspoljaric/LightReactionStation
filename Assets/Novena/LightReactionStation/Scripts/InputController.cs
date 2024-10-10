using UnityEngine;

namespace Novena
{
  public class InputController : MonoBehaviour
  {
    [SerializeField] private TouchController _touchController;
    [SerializeField] private KeyboardController _keyboardController;
    [SerializeField] private UdpController _udpController;

    private int _input;
    private void Start()
    {
      _input = Settings.Settings.GetValue<int>("Input");
      SetInput();
    }

    private void SetInput()
    {
      if (_input == 0)
      {
        _touchController.enabled = true;
        _keyboardController.enabled = false;
        _keyboardController.enabled = false;
      }
      else if(_input == 1)
      {
        _touchController.enabled = false;
        _keyboardController.enabled = true;
        _keyboardController.enabled = false;
      }
      else if (_input == 2)
      {
        _touchController.enabled = false;
        _keyboardController.enabled = false;
        _keyboardController.enabled = true;
      }
      else
      {
        Debug.Log("Input doesn't exist.");
        GameManager.Instance.ErrorHappened();
      }

    }
  }
}
