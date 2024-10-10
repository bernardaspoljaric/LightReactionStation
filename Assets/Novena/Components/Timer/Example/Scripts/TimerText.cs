using Novena.Components.Timer;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerText : MonoBehaviour {
	private TMP_Text _text;
	[SerializeField] private Timer _timer;
	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (_timer == null) return;
		_text.SetText(_timer.FormatedTime);
		
	}
}
