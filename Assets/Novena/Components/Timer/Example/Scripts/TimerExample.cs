using Novena.Components.Timer;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerExample : MonoBehaviour {
	private TMP_Text _text;
	[SerializeField] private Timer _timer;
	[SerializeField] private TMP_Dropdown _timeFormat;
	[SerializeField] private TMP_InputField _timeToCountdownFrom;
	[SerializeField] private TMP_InputField _customFormatString;
	[SerializeField] private Slider _numberOfDecimals;
	[SerializeField] private Toggle _isStopwatch;

	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
		_timer.IsStopwatch = true;
		_isStopwatch.onValueChanged.RemoveAllListeners();
		_isStopwatch.onValueChanged.AddListener((isOn) => {

			_timer.IsStopwatch = isOn;
		});
		PopulateDropdown();
		_timer.TimeFormat = TimeFormat.MinutesAndSeconds;
		_timer.NumberOfDecimals = 1;
		_timeFormat.onValueChanged.RemoveAllListeners();
		_timeFormat.onValueChanged.AddListener((index) => {
			_timer.TimeFormat = (TimeFormat)index;
		});
		_timer.FormatString = _customFormatString.text;
		_numberOfDecimals.onValueChanged.RemoveAllListeners();
		_numberOfDecimals.onValueChanged.AddListener((value) => {

			_timer.NumberOfDecimals = (int)value;
		});

		_timeToCountdownFrom.onValueChanged.RemoveAllListeners();
		_timeToCountdownFrom.onValueChanged.AddListener((text) => {

			_timer.TimeToCountdownFrom = float.Parse(text);
		});
	}



	public void StartTimer()
	{
		_timer.StartTimer();
	}
	public void ResumeTimer()
	{
		_timer.ResumeTimer();
	}
	public void PauseTimer()
	{
		_timer.PauseTimer();
	}
	public void StopTimer()
	{
		_timer.StopTimer();
	}


	private void PopulateDropdown()
	{
		_timeFormat.ClearOptions();

		string[] enumNames = Enum.GetNames(typeof(TimeFormat));

		List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
		foreach (string enumName in enumNames)
		{
			options.Add(new TMP_Dropdown.OptionData(enumName));
		}

		_timeFormat.AddOptions(options);
	}

	private void Update()
	{
		_text.text = _timer.FormatedTime;
	}
}
