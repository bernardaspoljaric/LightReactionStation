using Cysharp.Threading.Tasks;
using Novena.Exceptions;
using Novena.Networking;
using UnityEngine.Networking;

namespace Novena.DAL
{
  public class Repository
  {
    /// <summary>
    /// Send request to Download guide json with download code API.
    /// </summary>
    /// <param name="downloadCode"></param>
    /// <returns>Guide json string</returns>
    public async UniTask<string> GetGuideJson(string downloadCode)
    {
      var request = UnityWebRequest.Get(Api.DOWNLOAD_GUIDE_JSON + downloadCode);
      
      try
      {
        var response = await request.SendWebRequest();

        if (response.result == UnityWebRequest.Result.Success)
        {
          return response.downloadHandler.text;
        }

        throw new NetworkException(message: response.downloadHandler.text);
      }
      catch (UnityWebRequestException e)
      {
        throw new NetworkException(message: e.Message);
      }
    }
  }
}