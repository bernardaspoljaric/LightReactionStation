using Doozy.Engine.UI;
using Novena.Components.LanguageSwitch.Base;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LanguageButtonSingle : LanguageButton
{
  [Header("Components")] 
  [SerializeField] private TMP_Text label;

  private UIButton _uiButton;
  private List<TranslatedContent> _translatedContents;

  private void Awake()
  {
    _uiButton = gameObject.GetComponent<UIButton>();
  }

  public override void Setup(TranslatedContent translatedContent)
  {
    _translatedContents = Data.Guide.TranslatedContents.ToList();

    base.Setup(_translatedContents[0]);
    label.text = GetNextTranslatedContent().LanguageName;
  }

  private void SetState()
  {
    base.TranslatedContent = GetNextTranslatedContent();
    label.text = base.TranslatedContent.LanguageName;
  }

  private TranslatedContent GetNextTranslatedContent()
  {
    //Get index of current translated content in Data.Guide.TranslatedContents
    var currentIndex = _translatedContents.IndexOf(base.TranslatedContent);

    //Get next in _translatedContents check is not bigger than _translatedContents.Count
    var nextIndex = currentIndex + 1 < _translatedContents.Count ? currentIndex + 1 : 0;

    return _translatedContents[nextIndex];
  }

  public void OnButton_Click()
  {
    SetState();
    base.OnButtonClick();
  }
}
