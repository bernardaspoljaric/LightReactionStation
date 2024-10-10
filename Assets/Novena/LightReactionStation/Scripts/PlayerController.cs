using UnityEngine;

namespace Novena
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] GameObject[] _players;
    private int _playerNumber;
    private int _activePlayer;

    private void Start()
    {
      _playerNumber = Settings.Settings.GetValue<int>("PlayerNumber");
      CheckPlayerNumber();

      if (_playerNumber == 1)
        _activePlayer = 0;

      SetPlayers();
    }

    /// <summary>
    /// Get how many players is game for.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerNumber()
    {
      return _playerNumber;
    }

    /// <summary>
    /// Set currently active player.
    /// </summary>
    /// <param name="index"></param>
    public void SetActivePlayer(int index)
    {
      _activePlayer = index;
    }

    /// <summary>
    /// Get currently active player.
    /// </summary>
    /// <returns></returns>
    public GameObject GetActivePlayer()
    {
      return _players[_activePlayer];
    }

    /// <summary>
    /// Reset player's previous achievments.
    /// </summary>
    public void ResetPlayers()
    {
      for (int i = 0; i < _playerNumber; i++)
      {
        _players[i].GetComponent<ScoreController>().ResetScore();
      }
    }

    /// <summary>
    /// Check player number, if doesn't match settings instructions do automatic set.
    /// </summary>
    private void CheckPlayerNumber()
    {
      if (_playerNumber < 1)
      {
        Debug.Log("Game can't have 0 player, automatic set to 1.");
        _playerNumber = 1;
      }
      else if (_playerNumber > 4)
      {
        Debug.Log("Game can't have more than 4 players, automatic set to 4.");
        _playerNumber = 4;
      }
    }

    /// <summary>
    /// Activate players.
    /// </summary>
    private void SetPlayers()
    {
      for (int i = 0; i < _playerNumber; i++)
      {
        _players[i].SetActive(true);
      }
    }
  }
}
