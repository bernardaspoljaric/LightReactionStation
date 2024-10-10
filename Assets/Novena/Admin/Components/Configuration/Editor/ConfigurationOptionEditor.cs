using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Novena
{
    [CustomEditor(typeof(Option))]
    public class ConfigurationOptionEditor : Editor
    {
         public VisualTreeAsset m_InspectorXML;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI.
            VisualElement myInspector = new VisualElement();       
            
            // Load from default reference.
            m_InspectorXML.CloneTree(myInspector);

             // Get the serialized object (the target of this custom editor)
         

           
            // Return the finished Inspector UI.
            return myInspector;
        }
    }
}
