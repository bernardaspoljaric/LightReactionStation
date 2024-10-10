using System;
using UnityEngine;

namespace Novena.Components.Timer {
	public class FormatTime {
		public static string SetFormatedTime(float time, int intTime, TimeFormat timeFormat, int numberOfDecimals, string customFormat)
		{
			string formatedTime = "";

			switch (timeFormat)
			{
				case TimeFormat.MinutesAndSeconds:

					formatedTime = (intTime / 60).ToString("D2") + ":" + (intTime % 60).ToString("D2");

					break;

				case TimeFormat.OnlySeconds:

					formatedTime = intTime.ToString();

					break;

				case TimeFormat.SecondsWithDecimals:

					formatedTime = Math.Round(time, numberOfDecimals).ToString($"F{numberOfDecimals}");

					break;

				case TimeFormat.MinutesAndSecondsWithDecimals:

					formatedTime = (intTime / 60).ToString("D2") + ":" + (intTime % 60).ToString("D2") + "." + Mathf.FloorToInt(time % 1 * Mathf.Pow(10, numberOfDecimals)).ToString($"D{numberOfDecimals}");

					break;

				case TimeFormat.Custom:

					try
					{
						formatedTime = time.ToString($@"{customFormat}");
					}
					catch (FormatException ex)
					{
						Debug.LogError(ex);
						return time.ToString();
					}

					break;
			}

			return formatedTime;
		}
	}
}