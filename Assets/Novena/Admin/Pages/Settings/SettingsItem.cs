using Novena.Settings;
using TMPro;
using UnityEngine;

namespace Novena.Admin.Pages.Settings
{
  public class SettingsItem : MonoBehaviour
  {
    [SerializeField] private TMP_Text _labelTmp;
    [SerializeField] private TMP_InputField _valueInputField;
    [SerializeField] private TMP_Text _descriptionTmp;

    public SettingData SettingData { get; private set; }
    
    public void Setup(SettingData settingData)
    {
      SettingData = settingData;
      _labelTmp.text = settingData.Name + ":";
      _descriptionTmp.text = settingData.Description;
      _valueInputField.text = settingData.Value;
    }

    public void SaveChanges()
    {
      SettingData.Value = _valueInputField.text;
    }
    
    
  }
}