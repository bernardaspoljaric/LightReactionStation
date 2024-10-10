using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Novena.Components.AvProVideoPlayer
{
  public class AvProVideoPlayer : MonoBehaviour
  {
    [SerializeField] private MediaPlayer _mediaPlayer;

    /// <summary>
    /// Open media.
    /// </summary>
    /// <param name="path"></param>
    public void LoadVideo(string path)
    {
      MediaPath mediaPath = new MediaPath(path, MediaPathType.AbsolutePathOrURL);
      _mediaPlayer.OpenMedia(mediaPath, false);
    }
    
    /// <summary>
    /// Unload media from media player.
    /// </summary>
    public void ResetPlayer()
    {
      _mediaPlayer.CloseMedia();
    }
  }
}