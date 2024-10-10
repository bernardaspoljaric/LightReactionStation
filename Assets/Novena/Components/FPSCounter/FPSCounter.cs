using UnityEngine;

/// <summary>
/// To display use
/// Key combination Left Ctrl and F
/// </summary>
public class FPSCounter : MonoBehaviour
{
  private bool _isRunning = false;
  private GUIStyle _style = new GUIStyle();

  #region counter fileds

  private int _lastFrameIndex;
  private float[] _frameDeltaTimeArray;

  #endregion

  #region unity

  private void Awake()
  {
    _style.fontSize = 58;
    _style.fontStyle = FontStyle.Bold;
    _style.normal.textColor = Color.red;

    _frameDeltaTimeArray = new float[50];
  }

  void Update()
  {
    if (IsKeyCombinationPressed())
    {
      if (_isRunning)
      {
        _isRunning = false;
        return;
      }

      _isRunning = true;
    }

    if (_isRunning)
    {
      _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
      _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
    }
  }

  private void OnGUI()
  {
    if (_isRunning)
    {
      GUI.Label(new Rect(0, 0, 100, 50), CalculateFPS().ToString() + " FPS", _style);
    }
  }

  #endregion

  #region internal

  private float CalculateFPS()
  {
    float total = 0f;

    foreach (var deltaTime in _frameDeltaTimeArray)
    {
      total += deltaTime;
    }

    return _frameDeltaTimeArray.Length / total;
  }

  /// <summary>
  /// Key combination Left Ctrl and F
  /// </summary>
  /// <returns></returns>
  private bool IsKeyCombinationPressed()
  {
    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F))
    {
      return true;
    }

    return false;
  }

  #endregion


}
