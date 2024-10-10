using Novena.Components.LanguageSwitch;
using Novena.DAL;
using Novena.UiUtility.Base;
using TMPro;
using UnityEngine;

namespace Assets.N3Guide.Base.Controllers
{
  public class TextTemplate : UiController
  {
    [Header("Components")]
    [SerializeField] private LanguageSwitcher _lngSwitcher;
    [SerializeField] private TMP_Text _contentTMPText;
    
    public override void OnShowViewStart()
    {
      //We need to setup language switcher component on every on every view where it has been used
      _lngSwitcher.Setup();
      SetContent();
    }

    private void SetContent()
    {
      _contentTMPText.text = Data.Theme?.GetMediaByName("Text")?.Text ?? "No text media!";
    }
  }
}