using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novena
{
  [CreateAssetMenu(fileName = "PlayerColor", menuName = "ScriptableObjects/PlayerColors", order = 1)]
  public class PlayerColors : ScriptableObject
  {
    public Color Player1 = Color.yellow;
    public Color Player2 = Color.blue;
    public Color Player3 = Color.red;
    public Color Player4 = Color.green;
  }
}
