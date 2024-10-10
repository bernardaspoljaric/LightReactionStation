using System;
using UnityEngine;
using UnityEngine.Events;

namespace Novena.Components.Timer {
	[Serializable]
	public class Timer : MonoBehaviour {

		#region Events

		/// <summary>
		/// When Timer is manually Stopped.
		/// </summary>
		public UnityEvent OnTimerStopped;
		/// <summary>
		/// Every Frame after Current and Formated Time is updated.
		/// </summary>
		public UnityEvent OnTimeUpdated;
		/// <summary>
		/// When Timer countdown reaches 0.
		/// </summary>
		public UnityEvent OnTimerEnded;
		/// <summary>
		/// Frame when Timer Update is turned on.
		/// </summary>
		public UnityEvent OnTimerStarted;
		/// <summary>
		/// When Timer is Paused
		/// </summary>
		public UnityEvent OnTimerPaused;
		/// <summary>
		/// When Timer is Resumed
		/// </summary>
		public UnityEvent OnTimerResumed;

		#endregion

		#region Public Fields

		/// <summary>
		/// Is Time Counting from 0?
		/// </summary>
		[SerializeField] public bool IsStopwatch { get; set; }
		/// <summary>
		/// Way of formating time. Can be custom set.
		/// </summary>
		[Obsolete]
		[SerializeField] public TimeFormat TimeFormat { get; set; }
		/// <summary>
		/// Number of decimals if time format has decimals in it. DOES NOT apply to Custom formating.
		/// </summary>
		[Obsolete]
		[SerializeField] public int NumberOfDecimals { get; set; }
		/// <summary>
		/// If IsStopwatch is false, time from which timer countsdown.
		/// </summary>
		[SerializeField] public float TimeToCountdownFrom { get; set; }
		/// <summary>
		/// String used to format the time.
		/// </summary>
		[SerializeField] public string FormatString { get; set; }
		/// <summary>
		/// Is the timer paused?
		/// </summary>
		[SerializeField] public bool IsPaused { get; private set; }
		/// <summary>
		/// Timers time as float.
		/// </summary>
		[SerializeField] public float CurrentTime { get; private set; }
		/// <summary>
		/// Timers time as formated string.
		/// </summary>
		[SerializeField]
		public string FormatedTime
		{
			get
			{
				return _formatedTime;
			}
		}

		#endregion

		#region Private Fields

		private bool _isTimerActive;
		private string _formatedTime;
		private int _frameCount = 0;
		private const int _framesToWait = 15;

		#endregion

		#region Public Functions

		/// <summary>
		/// Resets the timer and Starts it.
		/// </summary>
		public void StartTimer()
		{
			IsPaused = false;
			_isTimerActive = false;

			CurrentTime = 0;

			if (!IsStopwatch)
				CurrentTime = TimeToCountdownFrom;

			_isTimerActive = true;

			OnTimerStarted?.Invoke();
		}

		/// <summary>
		/// Stops the timer and resets Current Time.
		/// </summary>
		public void StopTimer()
		{
			IsPaused = false;
			_isTimerActive = false;
			OnTimerStopped?.Invoke();

			CurrentTime = 0;
			if (!IsStopwatch)
				CurrentTime = TimeToCountdownFrom;
		}

		/// <summary>
		/// Pause the timer.
		/// </summary>
		public void PauseTimer()
		{
			IsPaused = true;
			OnTimerPaused?.Invoke();
		}

		/// <summary>
		/// Resume the timer.
		/// </summary>
		public void ResumeTimer()
		{
			IsPaused = false;
			OnTimerResumed?.Invoke();
		}

		#endregion

		#region Private Functions

		private void Update()
		{
			if (!_isTimerActive) return;
			if (IsPaused) return;

			_frameCount++;

			if (IsStopwatch)
			{
				CurrentTime += Time.deltaTime;


				if (_frameCount > _framesToWait)
				{
					_formatedTime = TimeSpan.FromSeconds(CurrentTime).ToString(FormatString);
					_frameCount = 0;
				}
			}
			else
			{
				CurrentTime -= Time.deltaTime;


				if (_frameCount > _framesToWait)
				{
					_formatedTime = TimeSpan.FromSeconds(CurrentTime).ToString(FormatString);
					_frameCount = 0;
				}
				if (CurrentTime <= 0)
				{
					_isTimerActive = false;
					OnTimerEnded?.Invoke();
				}
			}
		}

		#endregion
	}
}