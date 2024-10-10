using Novena.Components.LanguageSwitch;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageSwitcher))]
public class LanguageSwitcherEditor : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    var languageSwitcher = (LanguageSwitcher) target;
   
  }
}
