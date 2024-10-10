using DG.Tweening;
using UnityEngine;

namespace Scripts.Quiz {
	public class QuizAnsweredPanelManager : MonoBehaviour {

		[SerializeField] private CanvasGroup _rightPanel;
		[SerializeField] private CanvasGroup _wrongPanel;

		// moguce staviti custom animation metode koje animiraju panele po zelji
		public void QuestionAnswered(bool answered, float animationTime, float animationDelay) // TODO - QUIZ dodati animation float za delay izmedu ekrana werong i sljedeceg pitanja
		{
			if (answered)
			{
				_rightPanel.blocksRaycasts = true;
				_rightPanel.DOFade(1.0f, animationTime).SetDelay(animationDelay).OnComplete(() =>
				_rightPanel.DOFade(0.0f, animationTime).SetDelay(animationDelay).OnComplete(()=>
				_rightPanel.blocksRaycasts = false));
			}
			else
			{
				_wrongPanel.blocksRaycasts = true;
				_wrongPanel.DOFade(1.0f, animationTime).SetDelay(animationDelay).OnComplete(() =>
				_wrongPanel.DOFade(0.0f, animationTime).SetDelay(animationDelay).OnComplete(() =>
				_wrongPanel.blocksRaycasts = false));
			}
		}

		public void HideAllPanels()
		{
			_rightPanel.DOFade(0.0f,0.0f);
			_rightPanel.blocksRaycasts = false;
			_wrongPanel.DOFade(0.0f, 0.0f);
			_wrongPanel.blocksRaycasts = false;
		}
	}
}