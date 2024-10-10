using Novena.DAL;
using Novena.UiUtility.Base;
using UnityEngine;

namespace Assets.N3Guide.Base.Controllers
{
  public class VideoTemplate : UiController
  {
    [Header("Components")]
    [SerializeField] private NVideoPlayer _nVideoPlayer;

    public override void OnShowViewStart()
    {
      SetVideo();
    }

    public override void OnHideViewStart()
    {
      _nVideoPlayer.ClosePlayer();
    }

    private void SetVideo()
    {
      var videoTheme = Data.TranslatedContent.GetThemeByName("Tema 2");
      var videoMedia = videoTheme.GetMediaByName("Video");
      var videoPath = videoMedia?.FullPath;

      if (string.IsNullOrEmpty(videoPath)) return;
      _nVideoPlayer.Initialize();
      _nVideoPlayer.LoadVideo(videoPath);
      _nVideoPlayer.IsLooping(true);
    }
  }
}