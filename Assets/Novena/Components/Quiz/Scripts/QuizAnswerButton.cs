using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Quiz {
	public class QuizAnswerButton : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private TMP_Text _answerText;
		[SerializeField] private TMP_Text _indicatorLetterText;
		[Space(10)]
		[Header("Helpers")]
		[SerializeField] private TMP_Text _textHelper;
		[SerializeField] private Image _fillImageHelper;
		[Header("Settings")]
		[SerializeField] private float _animationSpeed = 0.25f;

		private bool _rightAnswer = false;
		public bool RightAnswer { get => _rightAnswer; set => _rightAnswer = value; }

		public void SetupButton(string indicatorForText, string answerText, Action onClick)
		{
			_answerText.text = answerText;

			if (indicatorForText == "null")
			{
				_indicatorLetterText.text = "";
			}
			else
			{
				_indicatorLetterText.text = indicatorForText;
			}

			Button btn = gameObject.GetComponent<Button>();
			btn.onClick.AddListener(() => { onClick(); IndicateWrong(); });
		}

		// TODO - QUIZ change naming
		// CALLED RIGHT ANSWERED BUTTON IF ITS WRONG ANSWERED
		// OnWrongButtonClickTriggerRightButton
		public void IndicateRight()
		{
			_textHelper.GetComponent<CanvasGroup>().DOFade(1.0f, (_animationSpeed / 0.5f));
			_fillImageHelper.color = Color.green;
		}

		private void IndicateWrong()
		{
			// kada je dobro odgovoren tada ne prikaze Texthelper!!!!!
			_textHelper.enabled = false;

			if (!_rightAnswer)
			{
				_fillImageHelper.color = Color.red;
			}
		}

	}
}