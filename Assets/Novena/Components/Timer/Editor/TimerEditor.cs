using Novena.Components.Timer;
using UnityEditor;
using UnityEngine;

namespace Novena.Components.Timer {
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Timer))]
	public class TimerEditor : Editor {

		private Timer _timer;
		private int _tab;

		private void OnEnable()
		{
			EditorApplication.update += UpdateLabelDuringPlayMode;
		}

		private void OnDisable()
		{
			EditorApplication.update -= UpdateLabelDuringPlayMode;
		}

		private void UpdateLabelDuringPlayMode()
		{
			if (EditorApplication.isPlaying)
			{
				Repaint();
			}
		}


		public override void OnInspectorGUI()
		{

			_timer = (Timer)target;
			serializedObject.Update();

			_tab = GUILayout.Toolbar(_tab, new string[] { "Settings", "Events", "Controls" });

			switch (_tab)
			{
				case 0:

					EditorGUILayout.Space(7.5f);
					int popupIndex = _timer.IsStopwatch ? 1 : 0;
					popupIndex = EditorGUILayout.Popup("Type: ", popupIndex, new[] { "Timer", "Stopwatch" }, (GUIStyle)new(EditorStyles.popup) { alignment = TextAnchor.MiddleCenter });
					_timer.IsStopwatch = popupIndex == 1;
					EditorGUILayout.Space(7.5f);
					_timer.TimeFormat = (TimeFormat)EditorGUILayout.EnumPopup("Time Format: ", _timer.TimeFormat, (GUIStyle)new(EditorStyles.popup) { alignment = TextAnchor.MiddleCenter });

					EditorGUILayout.Space(15);


					if (!_timer.IsStopwatch)
					{
						EditorGUILayout.Space(5);
						GUILayout.BeginHorizontal();
						_timer.TimeToCountdownFrom = EditorGUILayout.FloatField("Countdown from: ", _timer.TimeToCountdownFrom);
						EditorGUILayout.LabelField(" seconds.");
						GUILayout.EndHorizontal();
					}
					if (_timer.TimeFormat == TimeFormat.SecondsWithDecimals || _timer.TimeFormat == TimeFormat.MinutesAndSecondsWithDecimals)
					{
						EditorGUILayout.Space(5);
						_timer.NumberOfDecimals = EditorGUILayout.IntSlider("Number Of Decimals: ", _timer.NumberOfDecimals, 1, 5);
					}

					if (_timer.TimeFormat == TimeFormat.Custom)
					{
						EditorGUILayout.Space(5);
						_timer.FormatString = EditorGUILayout.TextField("Custom Format String: ", _timer.FormatString);
					}
					EditorGUILayout.Space(5);
					GuiLine();
					EditorGUILayout.Space(5);
					EditorGUILayout.LabelField("Readonly Properties:", (GUIStyle)new(EditorStyles.whiteLabel) { alignment = TextAnchor.MiddleCenter });
					EditorGUILayout.Space(5);

					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Current Time: ");
					EditorGUILayout.LabelField(_timer.CurrentTime.ToString());
					GUILayout.EndHorizontal();
					EditorGUILayout.Space(5);
					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Current Formated Time: ");
					EditorGUILayout.LabelField(_timer.FormatedTime);
					GUILayout.EndHorizontal();
					EditorGUILayout.Space(5);
					EditorGUILayout.Toggle("Is Paused?", _timer.IsPaused);



					EditorGUILayout.Space(7.5f);
					EditorGUILayout.LabelField("");

					GuiLine();
					EditorGUILayout.Space(5);
					EditorGUILayout.LabelField("Controlls:", (GUIStyle)new(EditorStyles.whiteLabel) { alignment = TextAnchor.MiddleCenter });

					if (GUILayout.Button("Start Timer"))
					{
						_timer.StartTimer();
					}
					if (GUILayout.Button("Pause"))
					{
						_timer.PauseTimer();
					}

					if (GUILayout.Button("Resume"))
					{
						_timer.ResumeTimer();
					}

					if (GUILayout.Button("Stop"))
					{
						_timer.StopTimer();
					}
					break;

				case 1:


					EditorGUILayout.Space(7.5f);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnTimerStarted"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFormatedTimeChanged"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnTimerEnded"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnTimerPaused"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnTimerStopped"));


					EditorGUILayout.Space(7.5f);
					break;

			}



			EditorUtility.SetDirty(target);
			serializedObject.ApplyModifiedProperties();
		}

		private void GuiLine(int height = 1)
		{
			Rect rect = EditorGUILayout.GetControlRect(false, height);
			rect.height = height;
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
		}
	}
}