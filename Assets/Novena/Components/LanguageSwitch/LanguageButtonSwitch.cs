using Doozy.Engine.UI;
using Novena.Components.LanguageSwitch.Base;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Novena.Components.LanguageSwitch
{
  [RequireComponent(typeof(UIToggle))]
  public class LanguageButtonSwitch : LanguageButton
  {
    [Header("Components")]
    [Space(20)]
    [SerializeField] private Image border;
    [SerializeField] private Color borderColorActive;
    [SerializeField] private Color borderColorInactive;
    [Space(20)]
    [SerializeField] private Image background;
    [SerializeField] private Color backgroundColorActive;
    [SerializeField] private Color backgroundColorInactive;
    [Space(20)]
    [SerializeField] private TMP_Text label;
    [SerializeField] private Color labelColorActive;
    [SerializeField] private Color labelColorInactive;


    private UIToggle _toggle;
    
    private void Awake()
    {
      _toggle = gameObject.GetComponent<UIToggle>();
    }

    public override void Setup(TranslatedContent translatedContent)
    {
      base.Setup(translatedContent);
      _toggle.Toggle.group = gameObject.transform.parent.GetComponent<ToggleGroup>();
      label.text = translatedContent.LanguageName;

      SetState(Data.TranslatedContent.LanguageId == TranslatedContent.LanguageId);
    }

    private void SetState(bool state)
    {
      //Change colors
      if (border != null)
      {
        border.color = state ? borderColorActive : borderColorInactive;
      }
      
      if (background != null)
      {
        background.color = state ? backgroundColorActive : backgroundColorInactive;
      }
      
      if (label != null)
      {
        label.color = state ? labelColorActive : labelColorInactive;
      }
      
      _toggle.IsOn = state;
    }

    public void OnButton_Click(bool state) 
    {
      base.OnButtonClick();
      SetState(state);
    }
  }
}