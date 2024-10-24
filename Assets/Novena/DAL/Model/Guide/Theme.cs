﻿#nullable enable
using System.Linq;
using Novena.Admin.FileSave.Attribute;
using Novena.Enumerators;

namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class Theme
  {
    public int Id{ get; set; } 
    public string Name{ get; set; } 
    public int Rank{ get; set; } 
    public float Longitude{ get; set; } 
    public float Latitude{ get; set; } 
    public float PositionX{ get; set; } 
    public float PositionY{ get; set; } 
    [Admin.FileSave.Attribute.File(FileAttributeType.Path)]
    public string ImagePath { get; set; }
    
    [Admin.FileSave.Attribute.File(FileAttributeType.Timestamp)]
    public string ImageTimestamp { get; set; }
    
    [Admin.FileSave.Attribute.File(FileAttributeType.Size)]
    public ulong ImageSize { get; set; }
    public string Label{ get; set; } 
    public int BeaconId{ get; set; } 
    public int MapId{ get; set; } 
    public int LanguageSwitchCode{ get; set; } 
    public Tag[] Tags{ get; set; } 
    public SubTheme[] SubThemes{ get; set; } 
    public Media[] Media{ get; set; } 

    /// <summary>
    /// This is thumbnail of theme
    /// </summary>
    public Image Image => GetImage();

    /// <summary>
    /// Creates Image object
    /// </summary>
    /// <returns>Image</returns>
    private Image GetImage()
    {
      return new Image { Path = ImagePath, TimeStamp = ImageTimestamp };
    }

    #region Helper methods

    public SubTheme? GetSubThemeByName(string name)
    {
      if (SubThemes.Any() == false) return null;

      return SubThemes.FirstOrDefault(sb => sb.Name == name);
    }

    /// <summary>
    /// Returns first media of requested type.
    /// </summary>
    /// <returns>Media or null if nothing found</returns>
    public Media? GetMediaByType(MediaType type)
    {
      Media? media = Media?.FirstOrDefault(m => m.MediaTypeId == (int)type);

      return media;
    }

    /// <summary>
    /// Gets media by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Media or null if nothing found</returns>
    public Media? GetMediaByName(string name)
    {
      Media? media = Media?.FirstOrDefault(m => m.Name == name);
      
      return media;
    }

    /// <summary>
    /// Get theme tag by category name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns>Tag or NULL if nothing found!</returns>
    public Tag? GetThemeTagByCategoryName(string categoryName)
    {
      TagCategorie? tagCategorie = Data.TranslatedContent.GetTagCategoryByName(categoryName);

      if (tagCategorie == null) return null;

      Tag? tag = Tags.FirstOrDefault(tag => tag.TagCategoryId == tagCategorie.Id);

      return tag;
    }

		public SubTheme? GetSubThemeByLanguageSwitchCode(int languageSwitchCode)
		{
			if (SubThemes.Any() == false) return null;

			return SubThemes.FirstOrDefault(sb => sb.LanguageSwitchCode == languageSwitchCode);
		}

		#endregion
	}
  
  public class Image
  {
    public string? Path;
    public string? TimeStamp;
  }

  [System.Serializable]
  public class SubTheme : Theme
  {
    
  }
}