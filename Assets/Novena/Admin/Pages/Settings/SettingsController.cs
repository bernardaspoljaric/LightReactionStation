using System.Collections.Generic;
using Novena.Helpers;
using Novena.Settings;
using Novena.UiUtility.Base;
using UnityEngine;

namespace Novena.Admin.Pages.Settings
{
  public class SettingsController : UiController
  {
    [SerializeField] private SettingsItem _settingsItem;
    [SerializeField] private Transform _itemsContainer;

    //private SettingsManager _settingsManager;
    private List<SettingsItem> _settingsItems = new ();
    private List<SettingData> _settingItems = new ();
    
    public override void OnShowViewStart()
    {
      Setup();
    }

    private void Setup()
    {
      _loadSettings();
      GenerateSettingsItems();
    }

    private void _loadSettings()
    {
      _settingItems = SettingsUtility.Load();
    }

    private void GenerateSettingsItems()
    {
      UnityHelper.DestroyObjects(_settingsItems);
      //var settingItems = _settingsManager.SettingItems;

      foreach (var settingItem in _settingItems)
      {
        var si = Instantiate(_settingsItem, _itemsContainer);
        si.Setup(settingItem);
        _settingsItems.Add(si);
      }
    }

    public void OnButtonSave_Click()
    {
      foreach (var settingsItem in _settingsItems)
      {
        settingsItem.SaveChanges();
      }
      
      SettingsUtility.Save(_settingItems);
      //_settingsManager.SaveSettings();
      Debug.Log("SETTINGS SAVED");
    }
  }
}