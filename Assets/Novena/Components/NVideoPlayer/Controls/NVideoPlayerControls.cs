using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class NVideoPlayerControls : MonoBehaviour
{
  [SerializeField] float _userInactiveDuration = 1.5f;
  public NVideoPlayer NVideoPlayer = null;

  #region private fields

  private CanvasGroup _canvasGroup;

  #endregion

  private void Awake()
  {
    GetComponents();
  }

  private void Update()
  {
    UpdateControlsVisibility();
  }

  private void GetComponents()
  {
    _canvasGroup = GetComponent<CanvasGroup>();
  }

  #region controlls visibility   

  private void UpdateControlsVisibility()
  {
    if (UserInteraction.IsUserInputThisFrame() || !CanHideControls())
    {
      UserInteraction.InactiveTime = 0f;
      ShowControls(true);
    }
    else
    {
      UserInteraction.InactiveTime += Time.unscaledDeltaTime;
      if (UserInteraction.InactiveTime >= _userInactiveDuration)
      {
        ShowControls(false);
      }
      else
      {
        ShowControls(true);
      }
    }
  }
  private bool CanHideControls()
  {
    bool result = true;
    if (Input.mousePresent)
    {
      // Check whether the mouse cursor is over the controls, in which case we can't hide the UI
      RectTransform rect = _canvasGroup.GetComponent<RectTransform>();
      Vector2 canvasPos;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out canvasPos);

      Rect rr = RectTransformUtility.PixelAdjustRect(rect, null);
      result = !rr.Contains(canvasPos);
    }

    return result;
  }

  private void ShowControls(bool isShow)
  {
    _canvasGroup.DOFade(isShow ? 1f : 0, 0.1f);
    _canvasGroup.blocksRaycasts = isShow;
  }

  #endregion

  struct UserInteraction
  {
    public static float InactiveTime;
    private static Vector3 _previousMousePos;
    private static int _lastInputFrame;

    public static bool IsUserInputThisFrame()
    {
      if (Time.frameCount == _lastInputFrame)
      {
        return true;
      }

      bool touchInput = (Input.touchSupported && Input.touchCount > 0);
      bool mouseInput = (Input.mousePresent && (Input.mousePosition != _previousMousePos ||
                                                Input.mouseScrollDelta != Vector2.zero || Input.GetMouseButton(0)));

      if (touchInput || mouseInput)
      {
        _previousMousePos = Input.mousePosition;
        _lastInputFrame = Time.frameCount;
        return true;
      }

      return false;
    }
  }
}



