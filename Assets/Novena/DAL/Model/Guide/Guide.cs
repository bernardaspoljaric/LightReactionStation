using Novena.Admin.FileSave.Attribute;

namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class Guide
  {
    public int Id{ get; set; } 
    public string Name{ get; set; } 
    public TranslatedContent[] TranslatedContents{ get; set; } 
    public Map[] Maps{ get; set; } 
  }
  
  
  [System.Serializable]
  public struct Map
  {
    public int Id{ get; set; } 
    public string Name{ get; set; } 
    
    [Admin.FileSave.Attribute.File(FileAttributeType.Path)]
    public string ImagePath{ get; set; } 
    [Admin.FileSave.Attribute.File(FileAttributeType.Timestamp)]
    public string ImageTimestamp{ get; set; }
    [Admin.FileSave.Attribute.File(FileAttributeType.Size)]
    public int ImageSize{ get; set; }
  }
}