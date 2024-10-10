using Cysharp.Threading.Tasks;
using DG.Tweening;
using Novena.Helpers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Novena.Games.MemoryGame {
	public class GameManager : MonoBehaviour {
		public UnityEvent OnGameOver;

		private TimeController _timeController;
		private ScoreController _scoreController;
		private TextController _textController;
		private AudioController _audioController;
		private TextureController _textureController;
		private CardManager _cardsManager;
		private GridLayoutGroup _gridLayoutGroup;

		[SerializeField] private Vector2 _gridSize;

		//this needs to go thru CMS
		[SerializeField] private List<Texture2D> _textureList;
		[SerializeField] private Texture2D _cardBackTexture;
		[SerializeField] private List<string> _cardTexts;
		[SerializeField] private List<int> _cardPrefabNumbers;


		private void Awake()
		{
			_gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
			_cardsManager = GetComponent<CardManager>();

			_gridLayoutGroup.constraintCount = (int)_gridSize.x;

			SubscribeEvents();
		}

		private void SubscribeEvents()
		{
			_cardsManager.OnGameOver += GameOver;
		}

		public void Setup()
		{
			Reset();

			_textureController = new TextureController(_textureList, _cardBackTexture);
			var uniqueCardList = _textureController.GetUniqueCardList();

			if (_cardPrefabNumbers.Count > 0)
			{
				for (int i = 0; i < uniqueCardList.Count; i++)
				{
					uniqueCardList[i].PrefabNumber = _cardPrefabNumbers[i];
				}
			}

			if (_cardTexts.Count == uniqueCardList.Count)
			{
				_textController = new TextController(_cardTexts, uniqueCardList);
			}

			uniqueCardList.ShuffleList();

			if (uniqueCardList.Count > 0)
				_cardsManager.InstantiateCards(_gridSize, uniqueCardList, _textureController.CardBackTexture);

			//otherwise cards cant change their sizedelta, grid layout is just for init positions
			TurnOffGridLayoutGroup().Forget();
		}

		private async UniTaskVoid TurnOffGridLayoutGroup()
		{
			await UniTask.WaitForEndOfFrame();
			_gridLayoutGroup.enabled = false;
		}

		public void SetTimer()
		{
			print("TODO: Timer");

		}

		public void SetScore()
		{

			print("TODO: Score");
		}

		public void SetupAudio(AudioClip cardTurn, AudioClip correct, AudioClip wrong, AudioClip won, AudioClip backgroundMusic = null, AudioClip lost = null)
		{
			print("TODO: Audio");
		}

		private void GameOver()
		{
			OnGameOver?.Invoke();
		}

		private void Reset()
		{
			_gridLayoutGroup.enabled = true;
			_cardsManager.Reset();

			//idk if i need to actually handle those since they arent mono (does unloadunusedassets handle those???)
			//_textureController.Reset();
			//_timeController.Reset();
			//_scoreController.Reset();
		}

		/*
		 * gettextures foreach texture->card->List<Card>
		 * shuffle list<card>
		 * setupcards instantiate amout of grid desire must be %2==0 
		 * create list<go(cardC->Card->Id,tex)>
		 * shuffle list<go>
		 * setupsounds
		 * 
		 * 
		 * mybe animation of creating cards(JUICE)
		 * setup all texts
		 * 
		 * start timer
		 * start counting score
		 * 
		 * onclik/tač turnover
		 * if two turned do le thing correct vanish em, wrong turn em back
		 * when last two pairs vanish engame/score thingie with option to reset
		*/
	}
}