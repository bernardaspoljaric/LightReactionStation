using System.IO;
using UnityEngine;

namespace Novena.Networking
{
  public static class Api
  {
	/// <summary>
    /// https://n3guide.novena.agency
    /// </summary>
	  public const string NOVENA_HOST = 
      "http://n3guide.novena.agency";
	  
    /// <summary>
    /// http://n3guide.novena.agency/sys/api/guide/json.ashx?downloadCode=
    /// </summary>
    public const string DOWNLOAD_GUIDE_JSON = 
      "http://n3guide.novena.agency/sys/api/guide/json.ashx?downloadCode=";
    
    public const string GET_AVAILABLE_GUIDES = "http://n3guide.novena.agency/sys/api/user/availableGuides.aspx";
    
    public const string ApiGetGuideId =
      "http://n3guide.novena.agency/sys/api/guide/getGuideIdByDownloadCode.aspx?downloadCode=";
    public const string ApiPartialUpdate = 
      "http://n3guide.novena.agency/sys/api/guide/downloadPartial.aspx?downloadCode=";


    public static string GetGuidePath()
    {
      string path = "";
      string persistenDataPath = Application.persistentDataPath;
      
      path = Path.Combine(persistenDataPath);
      
      return path;
    }
  }
}