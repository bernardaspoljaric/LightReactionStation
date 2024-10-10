using Novena.DAL;
using TMPro;
using UnityEngine;

public class CmsTextTable : MonoBehaviour {
	private TMP_Text _text;

	[SerializeField] private bool _isThemeOrSubthemeName;
	[SerializeField] private int _themeLanguageSwitchCode;
	[SerializeField] private int _subthemeLanguageSwitchCode;
	[SerializeField] private string _textMedia;

	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
    Data.OnTranslatedContentUpdated += SetText;
	}

	private void SetText()
	{
		var translatedContent = Data.TranslatedContent;
		if (_isThemeOrSubthemeName)
		{
			if (_themeLanguageSwitchCode != 0)
			{
				_text.text = translatedContent.GetThemeByLanguageSwitchCode(_themeLanguageSwitchCode).Name;
			}
			if (_subthemeLanguageSwitchCode!=0)
			{
				_text.text = translatedContent.GetThemeByLanguageSwitchCode(_themeLanguageSwitchCode).GetSubThemeByLanguageSwitchCode(_subthemeLanguageSwitchCode).Name;

			}
			return;
		}

		if (_themeLanguageSwitchCode == 0 || string.IsNullOrEmpty(_textMedia)) return;
		var theme = translatedContent.GetThemeByLanguageSwitchCode(_themeLanguageSwitchCode);
		if (_subthemeLanguageSwitchCode != 0)
		{
			theme = theme.GetSubThemeByLanguageSwitchCode(_subthemeLanguageSwitchCode);
		}

		if (theme.GetMediaByName(_textMedia) != null)
		{
			_text.text = theme.GetMediaByName(_textMedia).Text;
		}
	}
}
