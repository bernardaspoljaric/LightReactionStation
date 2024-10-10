using UnityEngine;

/// <summary>
/// NVideoPlayer handles both AvProPlayer or VLC player.
/// 
/// Author : GoGs
/// 
/// Version : 1.0.0
/// </summary>

public class NVideoPlayer : MonoBehaviour
{
  [Tooltip("If enabled use VLC components to render video")]
  [SerializeField] private bool _useVlcPlugin;

  [HideInInspector]
  public NVideoPlugin Player;

  #region private fields

  private NVlcPlayer _nVlcPlayer;
  private NAvProPlayer _nAvProPlayer;
  private NVideoPlayerControls _nVideoControls;

  #endregion

  #region Unity 
  private void Awake()
  {
    GetPlayerComponents();
  }

  private void GetPlayerComponents()
  {
    _nVlcPlayer = GetComponentInChildren<NVlcPlayer>(true);
    _nAvProPlayer = GetComponentInChildren<NAvProPlayer>(true);
    _nVideoControls = GetComponentInChildren<NVideoPlayerControls>(true);
  }

  #endregion

  #region Public  

  public void Initialize()
  {
    if (_useVlcPlugin)
    {
      _nVlcPlayer.gameObject.SetActive(true);
      _nAvProPlayer.gameObject.SetActive(false);
      Player = _nVlcPlayer;
    }
    else
    {
      _nVlcPlayer.gameObject.SetActive(false);
      _nAvProPlayer.gameObject.SetActive(true);
      Player = _nAvProPlayer;
    }

    _nVideoControls.gameObject.SetActive(true);
  }

  /// <summary>
  /// Load video
  /// </summary>
  public void LoadVideo(string mediaPath)
  {
    Player.OpenMedia(mediaPath);
  }

  public void ClosePlayer()
  {
    Player.CloseMedia();
    _nVideoControls.gameObject.SetActive(false);
    _nVlcPlayer.gameObject.SetActive(false);
    _nAvProPlayer.gameObject.SetActive(false);
  }

  public void Stop()
  {
    Player.Stop();
  }

  public void Pause()
  {
    Player.Pause();
  }

  public void IsLooping(bool loop)
  {
    Player.IsLooping = loop;
  }

  #endregion


  #region internal

  

  #endregion
}
