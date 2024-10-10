using System;
using UnityEngine;

public abstract class NVideoPlugin : MonoBehaviour
{
  #region Events
  public abstract event Action OnStarted;
  public abstract event Action OnEnded;
  public abstract event Action OnPaused;
  #endregion

  #region Properties

  public abstract bool IsPlaying { get; }

  public abstract long Duration { get; }

  public abstract long Time { get; }

  public abstract bool IsLooping { get; set; }

  #endregion


  #region Functions

  public abstract void OpenMedia(string path, bool autoPLay = true);

  /// <summary>
  /// Stop video and dispose player.
  /// </summary>
  public abstract void CloseMedia();

  public abstract void Play();
  public abstract void Pause();
  public abstract void Stop();
  public abstract void SetTime(long time);

  /// <summary>
  /// Set volume of video player.
  /// From 0 - 1.
  /// </summary>
  /// <param name="volumeLevel">0-1</param>
  public abstract void SetVolume(float volumeLevel);

  #endregion
}

