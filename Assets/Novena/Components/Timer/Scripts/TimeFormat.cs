using System;

namespace Novena.Components.Timer {
	[Serializable]
	public enum TimeFormat {
		MinutesAndSeconds,
		OnlySeconds,
		SecondsWithDecimals,
		MinutesAndSecondsWithDecimals,
		Custom
	}
}
