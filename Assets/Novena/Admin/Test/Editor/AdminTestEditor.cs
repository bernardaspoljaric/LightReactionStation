using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdminTest))]
public class AdminTestEditor : Editor
{
  string imagePath = "";
  int orgSizeX = 0;
  int orgSizeY = 0;
  int desiredX = 0;
  int desiredY = 0;

  public override void OnInspectorGUI()
  {

    AdminTest adminTest = (AdminTest)target;

    DrawDefaultInspector();

    EditorGUILayout.HelpBox("Admin tests", MessageType.Info);

    imagePath = GUILayout.TextField(imagePath, 200);

    
    if (GUILayout.Button("Clean ImageLoader edited photos"))
    {
      adminTest.CleanImageLoaderEditedPhotos(imagePath);
    }

    orgSizeX = Convert.ToInt32(GUILayout.TextField(orgSizeX.ToString(), 20));
    orgSizeY = Convert.ToInt32(GUILayout.TextField(orgSizeY.ToString(), 20));
    desiredX = Convert.ToInt32(GUILayout.TextField(desiredX.ToString(), 20));
    desiredY = Convert.ToInt32(GUILayout.TextField(desiredY.ToString(), 20));

    var res = new FitSize(0, 0);

    if (GUILayout.Button("Calculate fit dimension"))
    {
      res = adminTest.GetFitSize(orgSizeX, orgSizeY, desiredX, desiredY);
      Debug.Log($"{res.x} : {res.y}");
    }
  }
}
