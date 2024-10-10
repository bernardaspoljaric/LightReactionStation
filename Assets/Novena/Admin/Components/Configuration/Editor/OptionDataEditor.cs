using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Novena
{
    [CustomEditor(typeof(OptionData))]
    public class OptionDataEditor : Editor
    {
        // Temporary field to store the new option input
        private string newOption = "";
        private string value = "";
        private int selectedOptionIndex = 0;

        public override void OnInspectorGUI()
        {
            // Get a reference to the target script
            OptionData myComponent = (OptionData)target;

            value = myComponent.Value;

            // Convert the options array to a List for easier manipulation
            List<string> optionsList = new List<string>(myComponent.Values);

            myComponent.Name = EditorGUILayout.TextField("Name", myComponent.Name);

            myComponent.Value = EditorGUILayout.TextField("Value", value);

            myComponent.Description = EditorGUILayout.TextField("Description", myComponent.Description);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Options");

            EditorGUILayout.Space();

            // Create a dropdown (popup) in the Inspector
            selectedOptionIndex = EditorGUILayout.Popup(
                "Selected Value",
                selectedOptionIndex,
                myComponent.Values,
                EditorStyles.popup
            );

            if (myComponent.Values.Length > 0)
            {
                myComponent.Value = myComponent.Values[selectedOptionIndex];

                EditorGUILayout.Space();

                // Section for editing or removing existing options
                EditorGUILayout.LabelField("Edit or Remove Options");
                for (int i = 0; i < optionsList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    // Text field to edit the current option
                    optionsList[i] = EditorGUILayout.TextField(optionsList[i]);

                    // Button to remove the option
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        optionsList.RemoveAt(i);
                        break; // Break to avoid modifying the list while iterating
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();

            // Input field for adding a new option
            newOption = EditorGUILayout.TextField("New Option", newOption);

            // Button to add the new option to the list
            if (GUILayout.Button("Add Option"))
            {
                if (!string.IsNullOrEmpty(newOption))
                {
                    // Add the new option to the list
                    optionsList.Add(newOption);

                    // Update the component's options array
                    myComponent.Values = optionsList.ToArray();

                    // Clear the input field after adding
                    newOption = "";

                    // Save the changes
                    EditorUtility.SetDirty(target);
                }
            }

            // Apply changes back to the component's options array
            if (GUI.changed)
            {
                myComponent.Values = optionsList.ToArray();
                EditorUtility.SetDirty(target);
            }

            /* // Draw the default inspector for other fields
            DrawDefaultInspector(); */
        }
    }
}
