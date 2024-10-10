using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Novena.Games.MemoryGame {
	public class TextController {
		public List<string> CardTexts { get; set; }

		public TextController(List<string> cardTexts, List<Card> cards)
		{
			CardTexts = cardTexts;

			SetCardTexts(cards);
		}

		private void SetCardTexts(List<Card> cards)
		{
			for (int i = 0; i < cards.Count; i++)
			{
				cards[i].Name = CardTexts[i];
			}
		}
	}
}
