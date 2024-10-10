using System;
using UnityEngine;

namespace Novena
{
  public class KeyboardController : MonoBehaviour
  {
    public static Action<int> OnKeyClicked;

    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Keypad1))
      {
        OnKeyClicked?.Invoke(1);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad2))
      {
        OnKeyClicked?.Invoke(2);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad3))
      {
        OnKeyClicked?.Invoke(3);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad4))
      {
        OnKeyClicked?.Invoke(4);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad5))
      {
        OnKeyClicked?.Invoke(5);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad6))
      {
        OnKeyClicked?.Invoke(6);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad7))
      {
        OnKeyClicked?.Invoke(7);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad8))
      {
        OnKeyClicked?.Invoke(8);
      }
      else if (Input.GetKeyDown(KeyCode.Keypad9))
      {
        OnKeyClicked?.Invoke(9);
      }

    }
  }
}
