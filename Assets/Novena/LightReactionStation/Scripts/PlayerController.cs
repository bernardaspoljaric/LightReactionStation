using UnityEngine;

namespace Novena
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] GameObject[] _players;
    private int _playerNumber;

    private void Start()
    {
      _playerNumber = Settings.Settings.GetValue<int>("PlayerNumber");
      if( _playerNumber < 1)
      {
        Debug.Log("Game can't have 0 player, automatic set to 1.");
        _playerNumber = 1;
      }
      else if( _playerNumber > 4)
      {
        Debug.Log("Game can't have more than 4 players, automatic set to 4.");
        _playerNumber = 4;
      }

      SetPlayers();
    }

    private void SetPlayers()
    {
      for (int i = 0; i < _playerNumber; i++)
      {
        _players[i].SetActive(true);
      }
    }
  }
}
