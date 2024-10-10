using Novena.Enumerators;
using UnityEngine;

public class AnalyticsExample : MonoBehaviour
{
  public void OnButtonCreate_Click()
  {
    //For test purposes we will use guideId 2222.
    //If guideId is NULL it will be set from Data.Guide.
    //If Data.Guide is NULL exception will be thrown!
    //value is id of language that user has selected.
    Analytics.Create(new AnalyticEvent(type: AnalyticType.langOpen, value: 1, guideId: 2222));

    //Example of creating AnalyticEvent with themeId for themeOpen event.
    Analytics.Create(new AnalyticEvent(type: AnalyticType.themeOpen, themeId: 10, value: null));

    Analytics.Create(new AnalyticEvent(type: AnalyticType.themeMediaOpen, themeId: 10, mediaId: MediaType.Audio, value: null));

    //Analytics.Create(new AnalyticEvent(type: AnalyticType.themeMediaOpen, themeId: 10, mediaId: MediaType.Video, value: null, guideId: 2222));

    //In this example we will use value for audioListen event.
    //Value is percentage of audio that user has listened.
    //Analytics.Create(new AnalyticEvent(type: AnalyticType.audioListen, themeId: 10, mediaId: MediaType.Audio, value: 5, guideId: 2222));
  }
}
