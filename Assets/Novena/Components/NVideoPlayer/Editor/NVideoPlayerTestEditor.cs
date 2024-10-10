using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NVideoPlayerTest))]
public class NVideoPlayerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    if (GUILayout.Button("Calculate time"))
    {
      GetTimeString(4863600);
    }
  }

  private string GetTimeString(long timeMilliseconds)
  {
    float totalSeconds = timeMilliseconds / 1000;
    float totalMilliseconds = timeMilliseconds;
    int hours = Mathf.FloorToInt(totalMilliseconds / 3600000);
    float usedSeconds = hours * 60f * 60f;

    int minutes = Mathf.FloorToInt((totalSeconds - usedSeconds) / 60f);
    usedSeconds += minutes * 60f;

    int seconds = Mathf.FloorToInt(totalSeconds - usedSeconds);

    string result;
    if (hours <= 0)
    {    
      result = string.Format("{0:00}:{1:00}", minutes, seconds);     
    }
    else
    {     
      result = string.Format("{2}:{0:00}:{1:00}", minutes, seconds, hours);     
    }

    return result;
  }
}
