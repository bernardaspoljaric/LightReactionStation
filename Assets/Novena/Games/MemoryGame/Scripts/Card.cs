using UnityEngine;

namespace Novena.Games.MemoryGame {
	public class Card {
		public int ID { get; set; }
		public Texture2D Texture { get; set; }
		public string Name { get; set; }
		public int PrefabNumber { get; set; }

		public Card(int id, Texture2D texture)
		{
			ID = id;
			Texture = texture;
			PrefabNumber = 0;
			Name = "";
		}
	}
}
