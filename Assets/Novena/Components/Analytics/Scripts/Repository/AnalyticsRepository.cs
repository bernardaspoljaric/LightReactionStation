using Cysharp.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AnalyticsRepository
{
  private const string SEND_ANALYTICS_API = "http://n3guide.novena.agency/sys/api/analytics/batch-add.aspx";
  private const string SEND_ANALYTICS_API_TEST = "https://cf6036d4-6f6a-4366-ad1a-180d1deaa0c7.mock.pstmn.io";

  AnalyticsLocalRepository _localRepository;

  public AnalyticsRepository()
  {
    _localRepository = new AnalyticsLocalRepository();
  }

  public void Insert(AnalyticEvent e)
  {
    _localRepository.Insert(e);
  }

  public async UniTask<bool> Send()
  {
    bool output = false;

    var events = _localRepository.GetAll();

    if (events.Count <= 0) return false;

#if UNITY_EDITOR
        Debug.Log("AnalyticsRepository => Send: Events counts: " + events.Count);
#endif

    AnalyticsEventsModel analyticsEventsModel = new AnalyticsEventsModel(events);
    string json = analyticsEventsModel.ToJson();

#if UNITY_EDITOR
    Debug.Log("AnalyticsRepository => Send: " + json);
#endif

    var bytes = Encoding.UTF8.GetBytes(json);

    WWWForm form = new WWWForm();
    form.AddField("mac", SystemInfo.deviceUniqueIdentifier);
    form.AddBinaryData("json", bytes);

    var request = UnityWebRequest.Post(SEND_ANALYTICS_API, form);

    request.timeout = 3;

    try
    {
      var response = await request.SendWebRequest();
      output = response.result == UnityWebRequest.Result.Success;

      if (output)
      {
        //Lets delete all that we have sent
        _localRepository.DeleteAll();
      }
    }
    catch (UnityWebRequestException e)
    {
      output = false;

      Debug.LogException(e);
    }

    return output;
  }

  public void Dispose()
  {
    _localRepository.Dispose();
  }
}
