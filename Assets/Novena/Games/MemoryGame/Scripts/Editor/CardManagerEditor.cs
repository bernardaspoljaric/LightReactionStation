//using UnityEngine;
//using UnityEditor;
//using Scripts.Quiz;
//using TMPro;
//using UnityEngine.UI;

//namespace Novena.Games.MemoryGame {
//	[CustomEditor(typeof(CardManager))]
//	public class CardManagerEditor : Editor {

//		private int tab;

//		override public void OnInspectorGUI()
//		{
//			this.serializedObject.Update();
//			var cardManager = target as CardManager;

//			tab = GUILayout.Toolbar(tab, new string[] { "Settings", "Components" });
//			switch (tab)
//			{
//				//GuiLine(2);		
//				case 0:
//					#region Animation settings - Section
//					EditorGUILayout.Space(15);
//					EditorGUILayout.LabelField("Quiz settings: ", EditorStyles.boldLabel);
//					cardManager.AnimationSpeed = EditorGUILayout.FloatField("Fading speed", cardManager.AnimationSpeed);
//					cardManager.AnimationDelay = EditorGUILayout.FloatField("Fading delay speed", cardManager.AnimationDelay);

//					if (cardManager.AnimationDelay <= cardManager.AnimationSpeed)
//					{
//						cardManager.AnimationDelay = cardManager.AnimationSpeed + 0.25f;
//					}

//					GuiLine(1);

//					cardManager.UseButtonIndicators = GUILayout.Toggle(cardManager.UseButtonIndicators, "Use ButtonIndicator");
//					if (cardManager.UseButtonIndicators)
//					{
//						EditorGUILayout.BeginHorizontal();
//						GUILayout.Space(15);
//						EditorGUILayout.BeginVertical();

//						GUILayout.Space(5);
//						cardManager.UseNumericalIndicators = GUILayout.Toggle(cardManager.UseNumericalIndicators, "Use Numerical Indicators");
//						if (cardManager.UseNumericalIndicators)
//						{
//							cardManager.UseAlphabeticalIndicators = false;
//						}

//						cardManager.UseAlphabeticalIndicators = GUILayout.Toggle(cardManager.UseAlphabeticalIndicators, "Use Alphabetical Indicators");
//						if (cardManager.UseAlphabeticalIndicators)
//						{
//							cardManager.UseNumericalIndicators = false;
//						}

//						EditorGUILayout.EndVertical();
//						EditorGUILayout.EndHorizontal();

//						EditorGUILayout.Space(5);
//					}
//					else
//					{
//						cardManager.UseAlphabeticalIndicators = false;
//						cardManager.UseNumericalIndicators = false;
//					}

//					GuiLine(1);

//					cardManager.UseCMSDebugging = GUILayout.Toggle(cardManager.UseCMSDebugging, "Use CMS Debugging");


//					#endregion

//					break;
//				case 1:
//					#region Quiz components - Section
//					EditorGUILayout.Space(15);
//					//EditorGUILayout.LabelField("Canvas groups :", EditorStyles.boldLabel);
//					cardManager.QuizMainContent = (CanvasGroup)EditorGUILayout.ObjectField("Quiz entire Content", cardManager.QuizMainContent, typeof(CanvasGroup), true);

//					cardManager.AnswerManager = (QuizAnsweredPanelManager)EditorGUILayout.ObjectField("Answered panel", cardManager.AnswerManager, typeof(QuizAnsweredPanelManager), true);

//					cardManager.QuestionText = (TMP_Text)EditorGUILayout.ObjectField("Question text", cardManager.QuestionText, typeof(TMP_Text), true);
//					cardManager.QuestionCounterText = (TMP_Text)EditorGUILayout.ObjectField("Question Counter Text", cardManager.QuestionCounterText, typeof(TMP_Text), true);

//					cardManager.QuizImage = (RawImage)EditorGUILayout.ObjectField("Image", cardManager.QuizImage, typeof(RawImage), true);

//					cardManager.AnswerButtonPrefab = (GameObject)EditorGUILayout.ObjectField("Answer button prefab", cardManager.AnswerButtonPrefab, typeof(GameObject), true);

//					cardManager.AnswerButtonContainerTop = (Transform)EditorGUILayout.ObjectField("Answer button container TOP", cardManager.AnswerButtonContainerTop, typeof(Transform), true);
//					cardManager.AnswerButtonContainerBottom = (Transform)EditorGUILayout.ObjectField("Answer button container BOT", cardManager.AnswerButtonContainerBottom, typeof(Transform), true);

//					cardManager.FinalPanel = (CanvasGroup)EditorGUILayout.ObjectField("Final panel", cardManager.FinalPanel, typeof(CanvasGroup), true);
//					EditorGUILayout.Space(5);
//					GuiLine(1);
//					EditorGUILayout.Space(5);
//					cardManager.CorrectAnswerText = (TMP_Text)EditorGUILayout.ObjectField("Correct Text", cardManager.CorrectAnswerText, typeof(TMP_Text), true);
//					cardManager.WrongAnswerText = (TMP_Text)EditorGUILayout.ObjectField("Wrong Text", cardManager.WrongAnswerText, typeof(TMP_Text), true);

//					#endregion

//					break;

//			}

//			// method to draw horizontal line in editor depends on size parameter
//			void GuiLine(int i_height = 1)
//			{
//				Rect rect = EditorGUILayout.GetControlRect(false, i_height);
//				rect.height = i_height;
//				EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
//			}
//		}
//	}
//}
