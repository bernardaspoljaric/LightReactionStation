using UnityEngine;

namespace Novena
{
  public class Players
  {
    public enum Player
    {
      Player1 = 1,
      Player2 = 2, 
      Player3 = 3,
      Player4 = 4
    }

    public static Color GetPlayerColor(Player player)
    {
      PlayerColors playerColors = Resources.Load<PlayerColors>("PlayerColor");

      switch (player)
      {
        case Player.Player1:
          return playerColors.Player1;
        case Player.Player2:
          return playerColors.Player2;
        case Player.Player3:
          return playerColors.Player3;
        case Player.Player4:
          return playerColors.Player4;
        default:
          return Color.black;
      }
    }

    public static Player GetPlayer(string playerName)
    {
      switch(playerName)
      {
        case "Player1":
          return Player.Player1;
        case "Player2":
          return Player.Player2;
        case "Player3":
          return Player.Player3;
        case "Player4":
          return Player.Player4;
        default:
          return Player.Player1;
      }
      
    }
  }
}
