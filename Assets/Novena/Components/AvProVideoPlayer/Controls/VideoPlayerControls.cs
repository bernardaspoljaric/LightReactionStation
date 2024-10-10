using System;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Novena.Components.AvProVideoPlayer.Controls
{
  public class VideoPlayerControls : MonoBehaviour
  {
    public Action OnVideoStarted;
    public Action OnVideoEnded;
    public Action OnVideoPaused;
    public Action OnVideoReadyToPlay;
    
    [Header("Components")]
    [SerializeField] public MediaPlayer MediaPlayer = null;
    [SerializeField] private CanvasGroup _controlsGroup;
    [SerializeField] float _userInactiveDuration = 1.5f;

    private ButtonPlayPauseControl _buttonPlayPause = null;
    
    /// <summary>
    /// Helper variable to handle pause state of mediaPlayer
    /// </summary>
    private bool _isPaused = true;
    
    private void Awake()
    {
      _buttonPlayPause = GetComponentInChildren<ButtonPlayPauseControl>();
      _buttonPlayPause.OnClick += OnButtonPlayPauseClick;
      
      SubscribeMediaPlayerEvents();
    }

    private void OnButtonPlayPauseClick()
    {
      if (MediaPlayer.Control.IsPlaying())
      {
        MediaPlayer.Pause(); 
        return;
      }

      //If video is ended and play button is pressed we have to rewind to beginning
      if (MediaPlayer.Control.IsFinished())
      {
        MediaPlayer.Control.Rewind();
      }
      
      MediaPlayer.Play();
    }

    private void SubscribeMediaPlayerEvents()
    {
      MediaPlayer.Events.AddListener((mediaPlayer, eventType, error) =>
      {
        switch (eventType)
        {
          case MediaPlayerEvent.EventType.FinishedPlaying :
            VideoEnded();
            break;
          case MediaPlayerEvent.EventType.Started :
            VideoStarted();
            break;
          case MediaPlayerEvent.EventType.ReadyToPlay :
            VideoReadyToPlay();
            break;
        }
      });
    }

    private void VideoEnded()
    {
      //We have to invoke stop which will invoke pause state of media player
      //It seems like bug that when event FinishedPlaying is invoked state
      //MediaPlayer.Control.IsPlaying() its still true
      MediaPlayer.Control.Stop();
      OnVideoEnded?.Invoke();
    }
    
    private void VideoStarted()
    {
      //Debug.Log("VideoStarted");
      OnVideoStarted?.Invoke();
    }

    private void VideoReadyToPlay()
    {
      //Debug.Log("VideoReadyToPlay");
      OnVideoReadyToPlay?.Invoke();
    }

    #region Handle controls visiblity

    private bool CanHideControls()
    {
      bool result = true;
      if (Input.mousePresent)
      {
        // Check whether the mouse cursor is over the controls, in which case we can't hide the UI
        RectTransform rect = _controlsGroup.GetComponent<RectTransform>();
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out canvasPos);

        Rect rr = RectTransformUtility.PixelAdjustRect(rect, null);
        result = !rr.Contains(canvasPos);
      }

      return result;
    }

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

    private void ShowControls(bool isShow)
    {
      _controlsGroup.DOFade(isShow ? 1 : 0, 0.1f);
      _controlsGroup.blocksRaycasts = isShow;
    }
    
    #endregion
    
    private void Update()
    {
      if(MediaPlayer == null) return;
      if (MediaPlayer.Info == null) return;
      if (MediaPlayer.Info.HasVideo() == false) return;

      UpdateControlsVisibility();

      if (MediaPlayer.Control.IsPaused())
      {
        if (_isPaused) return;
        _isPaused = true;
        OnVideoPaused?.Invoke();
      }
      else
      {
        _isPaused = false;
      }
    }
  }
  
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