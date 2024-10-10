using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayPause : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] private Image _playIcon = null;
  [SerializeField] private Image _pauseIcon = null;

  private Button _button = null;
  private NVideoPlugin _nVideoPlugin = null;

  private void Awake()
  {
    _button = GetComponent<Button>();
    _button.onClick.AddListener(OnButtonClick);

    TogglePlayPauseButtonIcon(false);
  }

  private void OnEnable()
  {
    _nVideoPlugin = GetComponentInParent<NVideoPlayerControls>().NVideoPlayer.Player;
    _nVideoPlugin.OnStarted += OnVideoStarted;
    _nVideoPlugin.OnPaused += OnVideoPaused;
    _nVideoPlugin.OnEnded += OnVideoEnded;
    CheckVideoState();
  }

  private void CheckVideoState()
  {
    if (_nVideoPlugin.IsPlaying)
    {
      TogglePlayPauseButtonIcon(true);
    } else
    {
      TogglePlayPauseButtonIcon(false);
    }    
  }

  private void OnVideoEnded()
  {
    TogglePlayPauseButtonIcon(false);
  }

  private void OnVideoPaused()
  {
    TogglePlayPauseButtonIcon(false);
  }

  private void OnVideoStarted()
  {
    TogglePlayPauseButtonIcon(true);
  }

  private void OnButtonClick()
  {
    if (_nVideoPlugin.IsPlaying)
    {
      _nVideoPlugin.Pause();
    }
    else
    {
      _nVideoPlugin.Play();
    }
  }

  private void TogglePlayPauseButtonIcon(bool isPlay)
  {
    try
    {
      if (isPlay)
      {
        _playIcon.enabled = false;
        _pauseIcon.enabled = true;
        return;
      }

      _playIcon.enabled = true;
      _pauseIcon.enabled = false;
    }
    catch (System.Exception e)
    {
        Debug.LogException(e);
    }
  }
}
