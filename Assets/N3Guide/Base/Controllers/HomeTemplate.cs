using System;
using System.Collections.Generic;
using Assets.N3Guide.Base.Scripts;
using Novena.Components.LanguageSwitch;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.Helpers;
using Novena.UiUtility.Base;
using UnityEngine;

namespace Assets.N3Guide.Base.Controllers
{
  public class HomeTemplate : UiController
  {
    [Header("Components")]
    [SerializeField] private LanguageSwitcher _languageSwitcher;
    
    [Header("MainMenu components")]
    [SerializeField] private Transform _mainMenuContainer;
    [SerializeField] private MenuButton _mainMenuBtnPrefab;

    #region Private fields

    private List<MenuButton> _menuButtonList;

    #endregion

    public override void Awake()
    {
      base.Awake();
      _menuButtonList = new List<MenuButton>();
    }

    public override void OnShowViewStart()
    {
      SetData();
      base.OnShowViewStart();
    }
    
    private void SetData()
    {
      _languageSwitcher.Setup();
      GenerateMainMenu();
    }
    
    private void GenerateMainMenu()
    {
      UnityHelper.DestroyObjects(_menuButtonList);

      try
      {
        var themeList = Data.TranslatedContent.GetThemesExcludeByTag("SYSTEM");

        for (int i = 0; i < themeList.Count; i++)
        {
          Theme theme = themeList[i];

          MenuButton mb = Instantiate(_mainMenuBtnPrefab, _mainMenuContainer);
          mb.SetButton(theme);
          mb.gameObject.SetActive(true);
        
          _menuButtonList.Add(mb);
        }
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }
  }
}