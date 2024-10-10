using RenderHeads.Media.AVProVideo;
using System;

public class NAvProPlayer : NVideoPlugin
{
  #region private fields

  private MediaPlayer _mediaPlayer;
  private bool _isLooping;

  #endregion

  public override bool IsPlaying { get => _mediaPlayer?.Control?.IsPlaying() ?? false; }

  public override long Duration { get => (long)(_mediaPlayer.Info.GetDuration() * 1000); }

  public override long Time { get => (long)(_mediaPlayer.Control.GetCurrentTime() * 1000); }

  public override bool IsLooping { get => _mediaPlayer.Control.IsLooping(); set => _mediaPlayer.Control.SetLooping(value); }

  public override event Action OnStarted;
  public override event Action OnEnded;
  public override event Action OnPaused;

  #region unity

  private void OnEnable()
  {
    Init();
  }

  #endregion

  #region internal

  private void Init()
  {
    _mediaPlayer = GetComponentInChildren<MediaPlayer>(true);
    SubscribeEvents();
  }

  private void SubscribeEvents()
  {
    _mediaPlayer.Events.AddListener((mediaPlayer, eventType, error) =>
    {
      switch (eventType)
      {
        case MediaPlayerEvent.EventType.FinishedPlaying:
          VideoEnded();
          break;
        case MediaPlayerEvent.EventType.Started:
          VideoStarted();
          break;
      }
    });
  }

  void VideoEnded()
  {
    OnEnded?.Invoke();
  }

  void VideoStarted()
  {
    OnStarted?.Invoke();
  }

  private void SetLooping(bool looping)
  {
    _isLooping = looping;
    _mediaPlayer.Control.SetLooping(looping);
  }

  #endregion


  #region public

  public override void CloseMedia()
  {
    _mediaPlayer.CloseMedia();
  }

  public override void OpenMedia(string path, bool autoPlay)
  {
    MediaPath mediaPath = new MediaPath(path, MediaPathType.AbsolutePathOrURL);
    _mediaPlayer.OpenMedia(mediaPath, autoPlay);
  }

  public override void Pause()
  {
    _mediaPlayer.Pause();
    OnPaused?.Invoke();
  }

  public override void Play()
  {
    _mediaPlayer.Play();
  }

  public override void SetTime(long time)
  {
    float t = time / 1000;
    _mediaPlayer.Control.Seek(t);
  }

  public override void Stop()
  {
    _mediaPlayer.Stop();
  }

  public override void SetVolume(float volumeLevel)
  {
    if (volumeLevel > 1) volumeLevel = 1;
    
    _mediaPlayer?.Control?.SetVolume(volumeLevel);
  }
  #endregion
}
