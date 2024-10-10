#nullable enable
using System.Collections.Generic;
using System.Linq;
using Novena.Admin.FileSave.Attribute;

namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class TranslatedContent
  {
    public int Id{ get; set; } 
    public int LanguageId{ get; set; } 
    public int Rank{ get; set; } 
    public string ContentTitle{ get; set; } 
    public string LanguageName{ get; set; } 
    public string LanguageEnglishName{ get; set; } 
    
    [Admin.FileSave.Attribute.File(FileAttributeType.Path)]
    public string LanguageThumbnailPath{ get; set; }
    
    [Admin.FileSave.Attribute.File(FileAttributeType.Timestamp)]
    public string LanguageThumbnailTimestamp{ get; set; }

    [Admin.FileSave.Attribute.File(FileAttributeType.Size)]
    public ulong LanguageThumbnailSize{ get; set; }
    
    public Theme[] Themes{ get; set; } 
    public TagCategorie[] TagCategories{ get; set; } 

    #region Helper methods

    /// <summary>
    /// Get theme by name.
    /// </summary>
    /// <param name="name">Theme name</param>
    /// <returns>Theme if found or null</returns>
    public Theme? GetThemeByName(string name)
    {
      Theme? output = null;

      output = Themes?.FirstOrDefault(theme => theme.Name == name);

      return output;
    }

    /// <summary>
    /// Get theme by label.
    /// </summary>
    /// <param name="label">Label name</param>
    /// <returns>Theme if found or null</returns>
    public Theme? GetThemeByLabel(string label)
    {
      Theme? output = null;
      
      output = Themes?.FirstOrDefault(theme => theme.Label == label);
      
      return output;
    }

    /// <summary>
    /// Get list of theme that have no tag or excluded tag name.
    /// </summary>
    /// <param name="excludeTagName"></param>
    /// <returns></returns>
    public List<Theme> GetThemesExcludeByTag(string excludeTagName)
    {
      List<Theme> output = new List<Theme>();

      output = Themes?.Where(t => t.Tags == null || t.Tags.Any(tag => tag.Title != excludeTagName)).ToList();

      return output;
    }

    /// <summary>
    /// Get tag category by name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns>TagCategorie or NULL if nothing found!</returns>
    public TagCategorie? GetTagCategoryByName(string categoryName)
    {
      return TagCategories?.FirstOrDefault(cat => cat.Title == categoryName);
    }

    /// <summary>
    /// Search for Theme by <b>code</b>.
    /// </summary>
    /// <param name="code">Language switch code</param>
    /// <returns>Theme. <b>NULL</b> if nothing found!!</returns>
    public Theme? GetThemeByLanguageSwitchCode(int code)
    {
      Theme? output = null;

      output = Themes?.FirstOrDefault(theme => theme.LanguageSwitchCode == code);
      
      return output;
    }

    #endregion
  }
}