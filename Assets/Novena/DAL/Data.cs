using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Novena.DAL.Model.Guide;
using Novena.Domain.Entities;
using UnityEngine;

namespace Novena.DAL
{
  /// <summary>
  /// Utility class for storing objects in use (Guide, Theme...)
  /// </summary>
  public static class Data
  {
    #region Events

    /// <summary>
    /// Invoked when translated content is changed!
    /// </summary>
    public static Action OnTranslatedContentUpdated;

    #endregion
    
    /// <summary>
    /// List of all downloaded guides.
    /// </summary>
    public static List<Guide> Guides { get; private set; }

    /// <summary>
    /// Current active guide.
    /// </summary>
    public static Guide Guide { get; set; }


    /// <summary>
    /// Current translated content (language).
    /// <para>
    /// Use this to store and get current selected language.
    /// </para>
    /// <example>
    /// When user click language button store that in here.
    /// </example>
    /// </summary>
    public static TranslatedContent TranslatedContent
    {
      get { return s_translatedContent; }
      set
      {
        if (s_translatedContent != null)
        {
          if (value.Id != s_translatedContent.Id)
          {
            s_translatedContent = value;
            SetCurrentThemeBySwitchCode();
            OnTranslatedContentUpdated?.Invoke();
          }
        }
        else
        {
          s_translatedContent = value;
          SetCurrentThemeBySwitchCode();
          OnTranslatedContentUpdated?.Invoke();
        }
        s_translatedContent = value;
      }
    }

    /// <summary>
    /// Current theme.
    /// <para>
    /// Use this to store and get current theme.
    /// </para>
    /// <example>
    /// When user click theme in list of themes store that in here.
    /// </example>
    /// </summary>
    public static Theme Theme { get; set; }

    
    /// <summary>
    /// Load content from DB. If there is any guide sets first as default guide.
    /// </summary>
    /// <param name="guides">List of guides from DB</param>
    public static void LoadContent(List<LocalGuide> guides)
    {
      Guides = new List<Guide>();
      
      foreach (var localGuide in guides)
      {
        try
        {
          var jsonData = localGuide.Json.Replace("~/files", "/files");
          var guide = JsonConvert.DeserializeObject<Guide>(jsonData);
        
          Guides.Add(guide);
        }
        catch (Exception e)
        {
          Debug.LogException(e);
        }
      }

      if (Guides.Any())
      {
        //Set default guide
        Guide = Guides[0];
        //Set default language
        TranslatedContent = Guide.TranslatedContents[0];
      }
    }


    #region Private fields

    private static TranslatedContent s_translatedContent;

    #endregion

    #region Private methods

    /// <summary>
    /// Search's for equivalent theme by LanguageSwitchCode and set it as current theme.
    /// </summary>
    /// <remarks>
    /// If theme is not found it will be set to NULL!!
    /// </remarks>
    private static void SetCurrentThemeBySwitchCode()
    {
      if (Theme == null)
      {
        //Debug.LogWarning("Current theme is not set!");
        return;
      }

      if (Theme.LanguageSwitchCode <= 0)
      {
        //Debug.LogWarning(Theme.Name + " does not have Language switch code!");
        return;
      }
      
      var newTheme = s_translatedContent.GetThemeByLanguageSwitchCode(Theme.LanguageSwitchCode);

      if (newTheme != null)
      {
        Theme = newTheme;
      }
      else
      {
        Debug.LogWarning("Unable to find theme to do switch: " + Theme.Name + "in " + s_translatedContent.LanguageEnglishName);
      }
    }

    #endregion
  }
}