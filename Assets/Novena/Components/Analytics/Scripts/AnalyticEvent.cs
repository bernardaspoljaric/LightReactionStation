using Novena.DAL;
using Novena.Enumerators;
using System;
using UnityEngine;

/// <summary>
/// Analytic event that will be created.
/// If guideId is NULL then Data.Guide will be used!
/// If Data.Guide is NULL then exception will be thrown!
/// </summary>
public class AnalyticEvent
{
  public AnalyticEvent(AnalyticType type, int? guideId = null)
  {
    Initialize(type, null, null, null, guideId);
  }

  public AnalyticEvent(AnalyticType type, int value, int? guideId = null)
  {
    Initialize(type, null, null, value, guideId);
  }

  public AnalyticEvent(AnalyticType type, int themeId, int? value, int? guideId = null)
  {
    Initialize(type, themeId, null, value, guideId);
  }

  public AnalyticEvent(AnalyticType type, int themeId, MediaType mediaId, int? value, int? guideId = null)
  {
    Initialize(type, themeId, mediaId, value, guideId);
  }

  private void Initialize(AnalyticType type, int? themeId, MediaType? mediaId, int? value, int? guideId = null)
  {
    GuideId = GetGuideId(guideId);
    Type = GuideId == 0 ? AnalyticType.none : type;
    ThemeId = themeId;
    MediaType = mediaId;
    Value = value;
    Time = DateTime.Now.ToUniversalTime();
  }

  public AnalyticType Type { get; private set; }
  public int GuideId { get; private set; }
  public int? ThemeId { get; private set; }
  public MediaType? MediaType { get; private set; }
  public int? Value { get; private set; }
  public DateTime Time { get; private set; }

  private int GetGuideId(int? guideId)
  {
    if (guideId != null)
    {
      return (int)guideId;
    }

    if (Data.Guide != null)
    {
      return Data.Guide.Id;
    }

    Debug.LogError("GuideId is NULL and Data.Guide is NULL! Unable to create Event!");
    return 0;
  }
}
