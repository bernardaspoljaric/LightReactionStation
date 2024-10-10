using System.Collections.Generic;
using UnityEngine;

namespace Novena.Games.MemoryGame {
	public class TextureController {
		private List<Texture2D> _textureList = new List<Texture2D>();
		private Texture2D _cardBackTexture;


		public Texture2D CardBackTexture { get { return _cardBackTexture; } }

		public TextureController(List<Texture2D> textureList, Texture2D cardBackTexture)
		{
			_textureList = textureList;
			_cardBackTexture = cardBackTexture;
		}

		public List<Card> GetUniqueCardList()
		{
			List<Card> cards = new List<Card>();

			for (int i = 0; i < _textureList.Count; i++)
			{
				Card card = new Card(i, _textureList[i]);
				cards.Add(card);
			}
			return cards;
		}
	}
}
