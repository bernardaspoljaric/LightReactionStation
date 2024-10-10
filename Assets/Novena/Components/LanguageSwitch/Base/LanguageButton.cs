using Novena.DAL;
using Novena.DAL.Model.Guide;
using UnityEngine;

namespace Novena.Components.LanguageSwitch.Base
{
  /// <summary>
  /// Abstract class for creating language button.
  /// </summary>
  public abstract class LanguageButton : MonoBehaviour
  {
    [HideInInspector] public TranslatedContent TranslatedContent;

    public virtual void Setup(TranslatedContent translatedContent)
    {
      TranslatedContent = translatedContent;
    }

    public virtual void OnButtonClick()
    {
      ChangeLanguage();
    }

    private void ChangeLanguage()
    {
      if (Data.TranslatedContent.Id != TranslatedContent.Id)
      {
        Data.TranslatedContent = TranslatedContent;

        if (LanguageSwitcherUtility.UseLanguageSwitchNode)
        {
          LanguageSwitcherUtility.GoToLanguageNode();
        }

        Analytics.Create(new AnalyticEvent(AnalyticType.langOpen, TranslatedContent.LanguageId));
      }
    }
  }
}