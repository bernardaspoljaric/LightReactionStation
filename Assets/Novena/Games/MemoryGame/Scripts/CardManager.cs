using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;

namespace Novena.Games.MemoryGame {

	public enum MatchedCards {
		Destroy,
		Stay,
		Fade
	}

	public class CardManager : MonoBehaviour {

		public Action OnGameOver;
		public UnityEvent<bool> OnTwoCardsTurned;

		[SerializeField] private List<GameObject> _cardPrefabs;
		[SerializeField] private RectTransform _grid;
		[SerializeField] private MatchedCards _matchedCards;
		[SerializeField] private float _timeToStayFaceUp;

		private List<GameObject> _listOfActiveCardGameObjects = new List<GameObject>();
		private List<CardController> _turnedCards = new List<CardController>();

		public void InstantiateCards(Vector2 gridSize, List<Card> cards, Texture2D cardBackTexture)
		{
			if (!CheckPrefabs(cards)) return;
			int gridSurface = (int)(gridSize.x * gridSize.y);
			if (!CheckGrid(gridSurface)) return;

			for (int i = 0; i < gridSurface; i++)
			{
				//This is because of pairing of the cards. You want every other to be different not every one.
				int j = i / 2;
				//This makes it so if there is less textures than the grid surface, it doesen't break, but uses the same ones again.
				j %= cards.Count;

				var cardGameObject = Instantiate(_cardPrefabs[cards[j].PrefabNumber], _grid);
				_listOfActiveCardGameObjects.Add(cardGameObject);

				var cardController = cardGameObject.GetComponent<CardController>();

				cardController.Setup(cards[j], cardBackTexture);
				cardController.OnCardClicked += (cC) => OnCardClicked(cC).Forget();
			}

			ShuffleCards();
		}

		private async UniTask OnCardClicked(CardController cardController)
		{
			AddCardToListOfTurnedCards(cardController);

			if (_turnedCards.Count == 2)
			{
				_listOfActiveCardGameObjects.ForEach((card) => card.GetComponent<Button>().interactable = false);

				if (_turnedCards[0].CardID == _turnedCards[1].CardID)
				{
					await TwoCardsTurned(_turnedCards, true);

					_turnedCards.ForEach((cC) => {
						_listOfActiveCardGameObjects.Remove(cC.gameObject);
					});

					CheckIfGameIsOver();
				}
				else
				{
					await TwoCardsTurned(_turnedCards, false);
				}

				_listOfActiveCardGameObjects.ForEach((card) => card.GetComponent<Button>().interactable = true);
				_turnedCards.Clear();
			}
		}

		private void AddCardToListOfTurnedCards(CardController cardController)
		{
			if (_turnedCards.Count < 2 && !_turnedCards.Any((c) => c == cardController))
			{
				_turnedCards.Add(cardController);
			}
		}

		private async UniTask TwoCardsTurned(List<CardController> turnedCards, bool areSame)
		{
			//needs to be used because delayed call fires after the list is cleared
			var temp = new List<CardController>(turnedCards);

			await UniTask.Delay((int)(_timeToStayFaceUp * 1000));

			if (!areSame)
			{
				temp.ForEach((tc) => tc.TurnOver());
			}
			else
			{
				switch (_matchedCards)
				{
					case MatchedCards.Destroy:
						temp.ForEach((tc) => tc.FadeImage(() => {
							Destroy(tc.gameObject);
						}));
						break;
					case MatchedCards.Fade:
						temp.ForEach((tc) => tc.FadeImage());
						break;
					default:
						break;
				}
			}

			OnTwoCardsTurned?.Invoke(areSame);
		}

		private void CheckIfGameIsOver()
		{
			if (_listOfActiveCardGameObjects.Count > 0) return;

			DOVirtual.DelayedCall(_timeToStayFaceUp, () => OnGameOver?.Invoke());
		}

		public void Reset()
		{
			foreach (var cardGO in _listOfActiveCardGameObjects)
			{
				Destroy(cardGO);
			}

			_listOfActiveCardGameObjects.Clear();
		}
		private bool CheckPrefabs(List<Card> cards)
		{
			int max = 0;
			for (int i = 0; i < cards.Count; i++)
			{
				max = Mathf.Max(cards[i].PrefabNumber, max);
			}
			if (max == _cardPrefabs.Count - 1) return true;
			else
			{
				Debug.LogError("Prefab number has to be the same as prefabs referenced!");
				return false;
			}
		}

		public void ShuffleCards()
		{
			List<int> indexes = new List<int>();
			List<Transform> items = new List<Transform>();
			for (int i = 0; i < _grid.childCount; ++i)
			{
				indexes.Add(i);
				items.Add(_grid.GetChild(i));
			}

			foreach (var item in items)
			{
				item.SetSiblingIndex(indexes[Random.Range(0, indexes.Count)]);
			}
		}

		private bool CheckGrid(int gridSurface)
		{
			bool isValid = true;

			if (gridSurface < 4)
			{
				Debug.LogError("Grid is too small lmao what r u a baby?!");
				isValid = false;
			}
			if (gridSurface % 2 != 0)
			{
				Debug.LogError("Grid is not even!");
				isValid = false;
			}
			return isValid;
		}
	}
}
