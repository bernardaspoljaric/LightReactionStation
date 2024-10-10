using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnalyticsEventsModel
{
  public List<AnalyticEventModel> AnalyticsData { get; set; }

  public AnalyticsEventsModel(List<AnalyticEventModel> e)
  {
    AnalyticsData = e;
  }

  /// <summary>
  /// Converts the object to a JSON string.
  /// </summary>
  /// <returns>Json string or empty string if object is empty!</returns>
  public string ToJson()
  {
    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
    jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

    return JsonConvert.SerializeObject(this, Formatting.None, jsonSerializerSettings);
  }
}

[Serializable]
public class AnalyticEventModel
{
  public int Id { get; private set; }
  public int guideId { get; private set; }
  public string timeUTC { get; private set; }
  public int eventTypeId { get; private set; }
  public int? value { get; private set; }
  public string deviceUID { get; private set; }
  public int? themeId { get; private set; }
  public int? mediaId { get; private set; }


  public AnalyticEventModel(int id, int guideId, string time, int type, int? value, int? themeId, int? mediaId)
  {
    this.Id = id;
    this.guideId = guideId;
    this.timeUTC = time;
    this.eventTypeId = type;
    this.value = value;
    this.deviceUID = SystemInfo.deviceUniqueIdentifier;
    this.themeId = themeId;
    this.mediaId = mediaId;
  }
}
