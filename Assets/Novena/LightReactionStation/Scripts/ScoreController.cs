using TMPro;
using UnityEngine;

namespace Novena
{
  public class ScoreController : MonoBehaviour
  {
    [SerializeField] private TMP_Text[] _scoreText;

    private int _score = 0;
    private Players.Player _player;

    private void Start()
    {
      _player = Players.GetPlayer(gameObject.name);

      for (int i = 0; i < _scoreText.Length; i++)
      {
        _scoreText[i].color = Players.GetPlayerColor(_player);
        _scoreText[i].text = _score.ToString();
        _scoreText[i].gameObject.SetActive(true);
      }

    }

    public void AddPoint()
    {
      _score++;

      for (int i = 0; i < _scoreText.Length; i++)
      {
        _scoreText[i].text = _score.ToString();
      }
    }

    public void ResetScore()
    {
      _score = 0;

      for (int i = 0; i < _scoreText.Length; i++)
      {
        _scoreText[i].text = _score.ToString();
      }
    }

    public int GetScore()
    {
      return _score;
    }
  }
}
