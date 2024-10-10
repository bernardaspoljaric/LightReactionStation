using Novena.UiUtility.Base;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenViewController : UiController
{
  [SerializeField] NVideoPlayer _nVideoPlayer;
  [SerializeField] NVideoPlayerControls _nVideoPlayerControls;

  [Space(10)]
  [Header("Video render components")]
  [SerializeField] RawImage _vlcRawImage;
  [SerializeField] DisplayUGUI _avProUGUI;

  #region Unity
  public override void Awake()
  {
    InitState();
    base.Awake();
  }

  #endregion


  #region UiView

  public override void OnShowViewStart()
  {
    Initialize();
  }

  public override void OnHideViewStart()
  {
    InitState();
    base.OnHideViewStart();
  }

  #endregion

  #region internal

  private void Initialize()
  {
    ŞetActivePlayer(_nVideoPlayer.Player);
    _nVideoPlayerControls.gameObject.SetActive(true);
  }

  private void ŞetActivePlayer(NVideoPlugin nVideoPlugin)
  {
    if (nVideoPlugin is NVlcPlayer)
    {
      try
      {
        var p = nVideoPlugin as NVlcPlayer;
        _avProUGUI.gameObject.SetActive(false);
        _vlcRawImage.gameObject.SetActive(true);
        _vlcRawImage.texture = p.texture;
        return;
      }
      catch (System.Exception e)
      {
        Debug.LogException(e);
      }
    }

    _avProUGUI.gameObject.SetActive(true);
    _vlcRawImage.texture = null;
    _vlcRawImage.gameObject.SetActive(false);
  }

  private void InitState()
  {
    _avProUGUI.gameObject.SetActive(false);
    _vlcRawImage.texture = null;
    _vlcRawImage.gameObject.SetActive(false);
    _nVideoPlayerControls.gameObject.SetActive(false);
  }

  #endregion


}
