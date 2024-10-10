using Novena.Admin;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DownloadData)), CanEditMultipleObjects]
public class DownloadDataEditor : Editor
{
  bool hasChanged = false;
  public override void OnInspectorGUI()
  {
    hasChanged = false;
    EditorGUILayout.LabelField("Download codes", GUILayout.Width(100));
    EditorGUILayout.Space();
    DrawItems();
  }

  private void DrawItems()
  {
    DownloadData downloadData = (DownloadData)target;

    foreach (DownloadCode downloadCode in downloadData.DownloadCodes)
    {
      EditorGUI.BeginChangeCheck();

      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.LabelField("Code:", GUILayout.Width(50));
      downloadCode.Code = EditorGUILayout.TextField(downloadCode.Code, GUILayout.Width(100));

      EditorGUILayout.LabelField("Guide name:", GUILayout.Width(80));
      downloadCode.Name = EditorGUILayout.TextField(downloadCode.Name, GUILayout.Width(200));

      EditorGUILayout.EndHorizontal();

      if (EditorGUI.EndChangeCheck())
      {
        hasChanged = true;
      }
    }

    EditorGUILayout.Space();

    // Allow the user to add a new item to the list
    if (GUILayout.Button("Add Code"))
    {
      downloadData.DownloadCodes.Add(new DownloadCode());
    }

    // Allow the user to remove an item from the list
    if (downloadData.DownloadCodes.Count > 0 && GUILayout.Button("Remove Code"))
    {
      downloadData.DownloadCodes.RemoveAt(downloadData.DownloadCodes.Count - 1);
    }

    if (hasChanged)
    {
      // Apply any changes to the serialized object
      serializedObject.ApplyModifiedProperties();
      EditorUtility.SetDirty(target);
    }
  }
}
