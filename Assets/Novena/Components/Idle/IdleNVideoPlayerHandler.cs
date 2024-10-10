using Novena.Components.Idle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
   * This component checks is NVideoPlayer playing.
   * If it is than reset's idle timer in another words
   * doesn't go to idle if any player is playing.
   */

[RequireComponent(typeof(IdleController))]
public class IdleNVideoPlayerHandler : MonoBehaviour
{
  /// <summary>
  /// All media players in scene,
  /// </summary>
  private List<NVideoPlugin> _mediaPlayers = new List<NVideoPlugin>();

  private float _checkTime = 1; //one second

  #region Unity

  // Start is called before the first frame update
  void Awake()
  {
    GetMediaPlayers();
  }

  // Update is called once per frame
  void Update()
  {
    //Check every _checkTime not every frame
    if (_checkTime <= 0)
    {
      CheckIsPlaying();
      _checkTime = 1;
    }

    _checkTime -= Time.deltaTime;
  }

  #endregion

  #region Internal

  /// <summary>
  /// Get all media players in scene
  /// </summary>
  private void GetMediaPlayers()
  {
    _mediaPlayers = FindObjectsOfType<NVideoPlugin>(true).ToList();
  }

  /// <summary>
  /// Iterate through each active player and if it is playing reset idle timer.
  /// </summary>
  private void CheckIsPlaying()
  {
    for (int i = 0; i < _mediaPlayers.Count; i++)
    {
      var player = _mediaPlayers[i];

      if (player.IsPlaying)
      {
        IdleController.Instance.ResetIdleTimer();
      }
    }
  }

  #endregion


}
