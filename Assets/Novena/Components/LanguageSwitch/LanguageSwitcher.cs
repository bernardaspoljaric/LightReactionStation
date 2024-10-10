using System;
using System.Collections.Generic;
using Novena.Components.LanguageSwitch.Base;
using Novena.DAL;
using Novena.Helpers;
using UnityEngine;

namespace Novena.Components.LanguageSwitch
{
  public class LanguageSwitcher : MonoBehaviour
  {
    [SerializeField] private LanguageButton _languageButton;
    [SerializeField] private Transform _buttonContainer;
    [Space(10)]
    [Tooltip("Should doozoy jump to Languge switch node on each change?")]
    [SerializeField] public bool UseLanguageSwitchNode = false;

    #region Private fields

    private List<LanguageButton> _languageButtons = new List<LanguageButton>();
    private static LanguageSwitcherUtility _languageSwitcherUtility;

    #endregion

    private void Awake()
    {
      if (_languageSwitcherUtility == null)
      {
        _languageSwitcherUtility = new LanguageSwitcherUtility(UseLanguageSwitchNode);
      }
    }

    public void Setup()
    {
      GenerateButtons();
    }

    /// <summary>
    /// Iterate through TranslatedContents and instantiate language button.
    /// </summary>
    private void GenerateButtons()
    {
      UnityHelper.DestroyObjects(_languageButtons);

      //add item to Data.Guide.TranslatedContents
      //var testLangList = new List<TranslatedContent>();
      //testLangList.Add(new TranslatedContent()
      //{
      //  LanguageId = 1,
      //  LanguageName = "Hrvatski",
      //  Id = 1
      //});

      //testLangList.Add(new TranslatedContent()
      //{
      //  LanguageId = 2,
      //  LanguageName = "English",
      //  Id = 2
      //}); 

      if (_languageButton.GetType() == typeof(LanguageButtonSingle))
      {
        var languageButton = Instantiate(_languageButton, _buttonContainer);
        languageButton.Setup(Data.Guide.TranslatedContents[0]);
        languageButton.gameObject.SetActive(true);

        _languageButtons.Add(languageButton);
        return;
      }

      try
      {
        foreach (var lang in Data.Guide.TranslatedContents)
        {
          var languageButton = Instantiate(_languageButton, _buttonContainer);
          languageButton.Setup(lang);
          languageButton.gameObject.SetActive(true);

          _languageButtons.Add(languageButton);
        }
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }
  }
}