#nullable enable
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using TMPro;
using UnityEngine;

namespace Assets.N3Guide.Base.Scripts
{
  public class MenuButton : MonoBehaviour
  {
    [Header("Components")] 
    [SerializeField] private TMP_Text _label = null;
    [SerializeField] private TMP_Text _title = null;

    public void SetButton(Theme theme)
    {
      _label.text = theme.Label;
      _title.text = theme.Name;

      UIButton btn = gameObject.GetComponent<UIButton>();
      btn.OnClick.OnTrigger.Event.AddListener(() =>
      {
        Tag? themeTag = theme.GetThemeTagByCategoryName("TEMPLATE");
        Doozy.Engine.GameEventMessage.SendEvent(themeTag?.Title);
        Data.Theme = theme;
      });
    }
  }
}