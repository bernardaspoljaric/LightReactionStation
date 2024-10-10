using DG.Tweening;
using Novena.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novena.Games.MemoryGame {
	public class CardController : MonoBehaviour {

		public Action<CardController> OnCardClicked;

		private RawImage _rawImage;
		private RectTransform _rt;
		private Button _button;
		private Card _card;
		private Texture2D _cardBackTexture;
		private TMP_Text _text;
		private CanvasGroup _canvasGroup;

		public int CardID { get; private set; }
		[SerializeField] private float _animationSpeed;

		private void Awake()
		{
			_rt = GetComponent<RectTransform>();
			_rawImage = GetComponentInChildren<RawImage>();
			_button = GetComponentInChildren<Button>();
			_text = GetComponentInChildren<TMP_Text>();
			_canvasGroup = GetComponent<CanvasGroup>();

			SubscribeEvents();
		}

		private void SubscribeEvents()
		{
			_button.onClick.AddListener(() => {
				_button.interactable = false;

				TurnOver();
				OnCardClicked?.Invoke(this);
			});
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveAllListeners();
		}

		public void Setup(Card card, Texture2D cardBackTexture)
		{
			_card = card;
			CardID = _card.ID;
			_cardBackTexture = cardBackTexture;
			_text.text = card.Name;

			_rawImage.texture = cardBackTexture;
		}

		public void FadeImage(Action onComplete = null)
		{
			CanvasGroupHelper.FadeCanvasGroup(_canvasGroup, false, _animationSpeed, 0, onComplete);
		}

		public void TurnOver()
		{
			AnimationController animationController = new AnimationController(_animationSpeed);

			if (_rawImage.texture == _card.Texture)
			{
				Turn(animationController, _rt, false, () => {
					_button.interactable = true;
				});
			}
			else
			{
				Turn(animationController, _rt, true);
			}
		}

		private void Turn(AnimationController animationController, RectTransform rt, bool front, TweenCallback onComplete = null)
		{
			animationController.TurnCardHalfwayX(rt, false, () => {
				_rawImage.texture = front ? _card.Texture : _cardBackTexture;
				animationController.TurnCardHalfwayX(rt, true, onComplete);
				animationController.ShowText(_text, front);
			});
		}


	}
}

