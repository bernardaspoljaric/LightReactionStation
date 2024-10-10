using DG.Tweening;
using Scripts.Helpers;
using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Novena.DAL.Model.Guide;
using Novena.Enumerators;
using Novena.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Quiz 
{
	public class Quiz : MonoBehaviour 
	{
		// CUSTOM ANIMACIJE NA BUTTONIMA SE RADE U QUIZANSWERBUTTON.CS GDJE SE POZIVAJU 2. METODE
		#region Public fields for Custom Editor
		public float AnimationSpeed = 1.0f; // brzina fade in fade out!
		public float AnimationDelay = 1.5f; // panel manager animation!

		public CanvasGroup QuizMainContent = null;
		public QuizAnsweredPanelManager AnswerManager = null;
		public TMP_Text QuestionText = null;
		public TMP_Text QuestionCounterText = null;
		public RawImage QuizImage = null;
		public GameObject AnswerButtonPrefab = null;
		public Transform AnswerButtonContainerTop = null;
		public Transform AnswerButtonContainerBottom = null;
		public CanvasGroup FinalPanel = null;

		public TMP_Text CorrectAnswerText = null;
		public TMP_Text WrongAnswerText = null;

		// for Custom editor
		public bool UseButtonIndicators;
		public bool UseNumericalIndicators;
		public bool UseAlphabeticalIndicators;
		public bool UseCustomIndicators;
		public List<string> CustomIndicators = null;

		// prikazuje sadrzaj iz CMS-a te prikazuje error koji se nalazi na odredenoj temi
		public bool UseCMSDebugging;
		#endregion

		#region Private quiz fields
		// private for QUIZ internal variables
		private Theme _theme;
		private List<QuizItem> _listOfQuizItems = new List<QuizItem>();
		private List<GameObject> _listOfAnswerButton = new List<GameObject>();

		private int _currentQuestion = 0;
		private int _numberOfMaximumQuestions = 0;
		// correct and wrong answers counters
		private int _correctAnsweredCounter = 0;
		private int _wrongAnsweredCounter = 0;

		string[] _chosenIndicator = null;
		// ako su potrebni custom indikatori moze se prmjenit sadrzaj za jedan od sljedecih array-eva
		string[] _numericalIndicators = new string[] { "1", "2", "3", "4" };
		string[] _alphabeticalIndicators = new string[] { "A", "B", "C", "D" };

		#endregion

		#region Endpoints
		public Action OnQuizFinished;
		public Action OnCorrectAnswered;
		public Action OnWrongAnswered;
		public Action<int, int> OnQuizFinishedData;
		#endregion

		public void Setup(Theme theme)
		{
			_theme = theme;

			ResetQuiz();
			SetupUIElements();
			SetupQuiz();

			//ispisuje sav sadrzaj quiza u Debug.Log te indicira u kojoj temi je krivo unesen sadržaj
			if (UseCMSDebugging)
			{
				CMSDebugging();
			}
		}

		private void SetupUIElements()
		{
			Debug.Log("UI elements setup");
			//string correctText = MediaHelper.Get.GetMedia(_theme, "CorrectText", MediaType.Text);
			string correctText = _theme.GetMediaByName("CorrectText")?.Text;
			//string wrongText = MediaHelper.Get.GetMedia(_theme, "WrongText", MediaType.Text);
			string wrongText = _theme.GetMediaByName("WrongText")?.Text;

			CorrectAnswerText.text = correctText;
			WrongAnswerText.text = wrongText;
		}

		private void ResetQuiz()
		{
			Resources.UnloadUnusedAssets();
			AnswerManager.HideAllPanels();
			
			QuizImage.DOFade(0.0f, 0.0f);

			QuizMainContent.DOFade(1.0f, 0.0f);
			QuizMainContent.blocksRaycasts = true;

			FinalPanel.DOFade(0.0f, 0.0f);
			FinalPanel.blocksRaycasts = false;			

			_numberOfMaximumQuestions = 0;
			_currentQuestion = -1;

			_wrongAnsweredCounter = 0;
			_correctAnsweredCounter = 0;

			_numberOfMaximumQuestions = _theme.SubThemes.Length;
			QuestionCounterText.text = "0/" + _numberOfMaximumQuestions;
		}

		private void SetupQuiz()
		{
			if (UseNumericalIndicators)
			{
				_chosenIndicator = _numericalIndicators;
			} else if (UseAlphabeticalIndicators)
			{
				_chosenIndicator = _alphabeticalIndicators;
			} else
			{
				_chosenIndicator = CustomIndicators.ToArray();
			}

			FillListWithQuizItems();
			SetAnotherQuestionContent();
		}

		// popunjava listu iz CMS-a
		private void FillListWithQuizItems()
		{
			_listOfQuizItems.Clear();

			var subThemesList = _theme.SubThemes;
			for (int x = 0; x <= subThemesList.Length - 1; x++)
			{
				Theme subTheme = subThemesList[x];

				List<string> answerlist = MediaHelper.Get.GetAllMediaContainingPartOfName(subTheme, "Answer", MediaType.Text);

				if (answerlist != null)
				{
					string question = "";
					List<Photo> tmpList = new List<Photo>();
					string imagePath = "";

					if (ThemeHelper.Check.ContainsMedia(subTheme, "Question"))
					{
						question = MediaHelper.Get.GetMedia(subTheme, "Question", MediaType.Text);
					}
					if (ThemeHelper.Check.ContainsMedia(subTheme, "Image"))
					{
						tmpList = MediaHelper.Get.GetMediaPhotos(subTheme, "Image");
					}
					if (tmpList.Count > 0)
					{
						imagePath = MediaHelper.Get.GetMediaPhotos(subTheme, "Image")[0].Path;
					}

					QuizItem QU = null;
					switch (answerlist.Count)
					{
						case 2:
							QU = new QuizItem(question, imagePath, FindRightAnswerIndicator(subTheme), answerlist[0], answerlist[1]);
							break;

						case 3:
							QU = new QuizItem(question, imagePath, FindRightAnswerIndicator(subTheme), answerlist[0], answerlist[1], answerlist[2]);
							break;

						case 4:
							QU = new QuizItem(question, imagePath, FindRightAnswerIndicator(subTheme), answerlist[0], answerlist[1], answerlist[2], answerlist[3]);
							break;

						default:
							break;
					}
					_listOfQuizItems.Add(QU);
				}
			}
		}

		// postavlja sljedeci content!!!
		private void SetAnotherQuestionContent()
		{
			UnityHelper.DestroyObjects(_listOfAnswerButton);
			_listOfAnswerButton.Clear();

			if ((_currentQuestion+1) >= _numberOfMaximumQuestions)
			{
				QuizFinished();
				return;
			}

			_currentQuestion++;

			QuestionCounterText.text = (_currentQuestion+1) + "/" + _numberOfMaximumQuestions;

			QuestionText.text = _listOfQuizItems[_currentQuestion].Question;

			if (!string.IsNullOrEmpty(_listOfQuizItems[_currentQuestion].ImagePath))
			{
				AssetsFileLoader.LoadTexture2D(_listOfQuizItems[_currentQuestion].ImagePath, QuizImage);

				QuizImage.DOFade(1.0f, AnimationSpeed);
				QuizImage.SetNativeSize();
			}
			else
			{
				QuizImage.DOFade(0.0f, 0.0f);
			}

			// spawn 4 buttons
			if (!string.IsNullOrEmpty(_listOfQuizItems[_currentQuestion].Answer4))
			{
				SpawnButtons(_listOfQuizItems[_currentQuestion], 4);
			}
			// spawn3 buttons
			else if (!string.IsNullOrEmpty(_listOfQuizItems[_currentQuestion].Answer3))
			{
				SpawnButtons(_listOfQuizItems[_currentQuestion], 3);
			}
			//spawn2 buttons
			else
			{
				SpawnButtons(_listOfQuizItems[_currentQuestion], 2);
			}
		}

		// call when quiz is finished!
		private void QuizFinished()
		{
			OnQuizFinished?.Invoke();
			OnQuizFinishedData?.Invoke(_correctAnsweredCounter, _wrongAnsweredCounter);

			FinalPanel.DOFade(1.0f, AnimationSpeed);
			FinalPanel.blocksRaycasts = true;

			Debug.Log("Right answers = " + _correctAnsweredCounter);
			Debug.Log("Wrong answers = " + _wrongAnsweredCounter);
		}

		private void SpawnButtons(QuizItem quizItem, int numberOfButtonsToSpawn)
		{
			List<string> listOfAnswersTMP = new List<string>();
			listOfAnswersTMP.Clear();

			listOfAnswersTMP.Add(quizItem.Answer1);
			listOfAnswersTMP.Add(quizItem.Answer2);
			listOfAnswersTMP.Add(quizItem.Answer3);
			listOfAnswersTMP.Add(quizItem.Answer4);

			int containerSeparator = 0;
			for (int x = 0; x < numberOfButtonsToSpawn; x++)
			{
				// provjera kolko je buttona potrebno za spawn, ako je 4 tada se napravi separate u top i bottom container
				containerSeparator++;
				Transform chosenContainer = null;
				if (numberOfButtonsToSpawn >= 4 && containerSeparator >= 3)
				{
					chosenContainer = AnswerButtonContainerBottom;
				} 
				else
				{
					chosenContainer = AnswerButtonContainerTop;
				}

				GameObject answerButton = Instantiate(AnswerButtonPrefab, chosenContainer);

				QuizAnswerButton mb = answerButton.GetComponent<QuizAnswerButton>();

				if (quizItem.RightAnswerIndicator == (x + 1))
				{
					if (!UseButtonIndicators)
					{
						mb.SetupButton("null", listOfAnswersTMP[x], () => { QuestionAnswered(true); });
					}
					else
					{
						mb.SetupButton(_chosenIndicator[x], listOfAnswersTMP[x], () => { QuestionAnswered(true); });
					}

					mb.RightAnswer = true;

				} else
				{
					if (!UseButtonIndicators)
					{
						mb.SetupButton("null", listOfAnswersTMP[x], () => { QuestionAnswered(false); });
					}
					else
					{
						mb.SetupButton(_chosenIndicator[x], listOfAnswersTMP[x], () => { QuestionAnswered(false); });
					}

					mb.RightAnswer = false;
				}
				_listOfAnswerButton.Add(answerButton);
			}
		}

		// Answer button CLICK trigera sljedecu metodu
		private void QuestionAnswered(bool rightAnswer)
		{
			QuizMainContent.blocksRaycasts = false;

			// provjera koji button je tocan i na njemu se pozove trigger za indikaciju dobrog odgovora
			foreach (GameObject go in _listOfAnswerButton)
			{
				QuizAnswerButton mb = go.GetComponent<QuizAnswerButton>();
				if (mb.RightAnswer == true)
				{
					mb.IndicateRight();
				}
			}

			if (rightAnswer == true)
			{
				AnswerManager.QuestionAnswered(true, AnimationSpeed, AnimationDelay);
				_correctAnsweredCounter++;

				OnCorrectAnswered?.Invoke();
			}
			else
			{
				AnswerManager.QuestionAnswered(false, AnimationSpeed, AnimationDelay);
				_wrongAnsweredCounter++;

				OnWrongAnswered?.Invoke();
			}

			StartCoroutine(SetAnotherQuestionWithDelay(AnimationDelay));
		}

		private IEnumerator SetAnotherQuestionWithDelay(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);

			// TODO - QUIZ potencijalno slozit da se setdelay postavi kao editor parametar!!
			// TODO - QUIZ potencijalno slozit da se blockraycast produzi!!
			QuizMainContent.DOFade(0.0f, AnimationSpeed).SetDelay(0.1f).OnComplete(() => {
				QuizMainContent.DOFade(1.0f, AnimationSpeed).SetDelay(0.1f).OnComplete(()=>
					QuizMainContent.blocksRaycasts = true); // setdelay nakon oncomplete-a!!
				SetAnotherQuestionContent();
			});
		}

		private int FindRightAnswerIndicator(Theme subTheme)
		{
			List<string> answerlist = MediaHelper.Get.GetAllMediaContainingPartOfName(subTheme, "Answer", MediaType.Text);

			for (int x = 0; x < answerlist.Count; x++)
			{
				string checkForRightAnswer = "";
				// TODO - QUIZ napraviti da ne baca warning u slucaju da nema medie u themi
				if (ThemeHelper.Check.ContainsPartOfMediaName(subTheme, "Answer" + (x + 1) + "*"))
				{
					checkForRightAnswer = MediaHelper.Get.GetMedia(subTheme, "Answer" + (x + 1) + "*", MediaType.Text);
				}

				//Debug.Log(checkForRightAnswer);
				if (!string.IsNullOrEmpty(checkForRightAnswer))
				{
					return (x + 1);
				}
			}
			return -1;
		}


		// testna metoda koja ispisuje sadrzaj pitanja i odgovora te sluzio kao debbugiranje ako je sadrzaj u CMS krivo postavljen
		private void CMSDebugging()
		{
			Debug.Log("_________________________QUIZ CONTENT_________________________");
			int themeCounter = 0;
			foreach (QuizItem quizItem in _listOfQuizItems)
			{
				string message = "";
				themeCounter++;

				if (quizItem?.Answer1 == null || quizItem?.Answer2 == null || quizItem?.RightAnswerIndicator == -1 || (quizItem?.Question == null && quizItem?.ImagePath == null))
				{
					Debug.Log("<color=red>Error: </color>There is problem with setup of " + themeCounter + ". subtheme!");
				} else
				{
					message += "[<color=blue>Firt answer =</color> " + quizItem.Answer1 + "] [<color=blue>Second Answer =</color> " + quizItem.Answer2 + "] ";

					if (!string.IsNullOrEmpty(quizItem.Answer3))
					{
						message += " [<color=blue>Third answer =</color> " + quizItem.Answer3 + "] ";
					} else
					{
						message += " [<color=blue>Third Answer = There is no 3.answer</color>] ";
					}

					if (!string.IsNullOrEmpty(quizItem.Answer4))
					{
						message += " [<color=blue>Forth answer =</color> " + quizItem.Answer4 + "] ";
					} else
					{
						message += " [<color=blue>Forth Answer = There is no 4.answer</color>] ";
					}

					if (!string.IsNullOrEmpty(quizItem.Question))
					{
						message += " [<color=blue>Question is =</color> " + quizItem.Question + "] ";
					}

					if (!string.IsNullOrEmpty(quizItem.ImagePath))
					{
						message += " [<color=blue>Image Path =</color> " + quizItem.ImagePath + "] ";
					} else
					{
						message += " [<color=blue>There is no image</color>] ";
					}

					Debug.Log(message);
				}

			}
		}

	}// end of Main class
}