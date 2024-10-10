using Scripts.Quiz;
using UnityEngine;

public class QuizEventHelper : MonoBehaviour
{
  public Quiz quiz;

	private void Awake()
	{
		quiz.OnCorrectAnswered += QuizCorrectAnswered;
		quiz.OnWrongAnswered += QuizWrongAnswered;
		quiz.OnQuizFinished += QuizFinished;
		quiz.OnQuizFinishedData += QuizFinishedInfo;
	}


	private void QuizCorrectAnswered()
	{
		Debug.Log("Endpoint -> correct answered!!");
	}

	private void QuizWrongAnswered()
	{
		Debug.Log("Endpoint -> wrong answered!!");
	}

	private void QuizFinishedInfo(int correctAnswers, int WrongAnswers)
	{
		Debug.Log("Endpoint -> correct = " + correctAnswers + " , wrong = " + WrongAnswers);
	}

	private void QuizFinished()
	{
		Debug.Log("Endpoint -> Quiz finished!!");
	}
}