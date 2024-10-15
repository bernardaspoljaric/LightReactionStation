using TMPro;
using UnityEngine;

namespace Novena
{
  public class ScoreController : MonoBehaviour
  {
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;
    private Players.Player _player;

    private void Start()
    {
      _player = Players.GetPlayer(gameObject.name);
      _scoreText.color = Players.GetPlayerColor(_player);
      _scoreText.text = _score.ToString();
      _scoreText.gameObject.SetActive(true);
    }

    public void AddPoint()
    {
      _score++;
      _scoreText.text = _score.ToString();
    }

    public void ResetScore()
    {
      _score = 0;
      _scoreText.text = _score.ToString();
    }

    public int GetScore()
    {
      return _score;
    }
  }
}
