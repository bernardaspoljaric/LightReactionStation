using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Novena
{
    [CustomEditor(typeof(ConfigurationData))]
    public class ConfigurationDataEditor : Editor
    {
        public VisualTreeAsset m_InspectorXML;

        private Category _selectedCategory;

        private List<Option> _options = new List<Option>();

        private Dictionary<string, List<Option>> _categoryOptions =
            new Dictionary<string, List<Option>>();

        private SerializedProperty optionsProperty;

        private void OnEnable()
        {
            optionsProperty = serializedObject.FindProperty("m_Options");
        }

        private void GetCategoryOptions(ConfigurationData configurationData)
        {
            _categoryOptions.Clear();
            foreach (Category category in configurationData.m_Categories)
            {
                List<Option> optionsForCategory = configurationData.m_Options
                    .Where(x => x.m_CategoryID == category.m_ID)
                    .ToList();

                if (optionsForCategory.Count == 0)
                {
                    optionsForCategory = new List<Option>();
                }

                _categoryOptions.Add(category.m_Name, optionsForCategory);
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            _options.Clear();
            // Get a reference to the target script
            ConfigurationData myComponent = (ConfigurationData)target;

            GetCategoryOptions(myComponent);

            _selectedCategory = myComponent.m_Categories.FirstOrDefault();

            // Create a new VisualElement to be the root of our Inspector UI.
            VisualElement myInspector = new VisualElement();

            // Load from default reference.
            m_InspectorXML.CloneTree(myInspector);

            //find element from myInspector
            ListView optionList = myInspector.Q<ListView>("OptionList");

            DropdownField categoryDropdown = myInspector.Q<DropdownField>("CategoryDropDown");
            categoryDropdown.choices = myComponent.m_Categories.Select(x => x.m_Name).ToList();
            categoryDropdown.value = _selectedCategory?.m_Name ?? null;
            categoryDropdown.RegisterValueChangedCallback(
                (evt) =>
                {
                    /* _selectedCategory = myComponent.m_Categories.FirstOrDefault(
                        x => x.m_Name == evt.newValue
                    );
                    _options = _categoryOptions[_selectedCategory.m_Name];
                    optionList.itemsSource = _options;

                    optionList.Rebuild();
                    Debug.Log(
                        "Category changed to "
                            + evt.newValue
                            + " with "
                            + _options.Count
                            + " options"
                    ); */
                }
            );

            try
            {
                _options = myComponent.m_Options; //_categoryOptions[_selectedCategory.m_Name];
            }
            catch (System.Exception) { }

            optionList.itemsSource = _options; //myComponent.m_Options;

            optionList.itemsAdded += (items) =>
            {
                Debug.Log(
                    "Item added"
                        + items.Count()
                        + optionList.itemsSource.Count
                        + " "
                        + _options.Count
                );

               /*  //Remove the last item from the list
                _options.RemoveAt(_options.Count - 1);

                var option = new Option();
                option.m_CategoryID = _selectedCategory.m_ID;
                option.m_ValueType = ValueType.String;

                _options.Add(option);

                //save all lists from the dictionary to the options list

                myComponent.m_Options.Clear();

                foreach (var categoryOptions in _categoryOptions.Values)
                {
                    myComponent.m_Options.AddRange(categoryOptions);
                } */

                //myComponent.m_Options = _categoryOptions.Values.SelectMany(x => x).ToList();
            };

            optionList.itemsRemoved += (items) =>
            {
               /*  myComponent.m_Options.Clear();

                foreach (var categoryOptions in _categoryOptions.Values)
                {
                    myComponent.m_Options.AddRange(categoryOptions);
                } */
            };

            optionList.makeItem = () =>
            {
                VisualElement categoryItem = new VisualElement();

                categoryItem.style.marginBottom = 5;

                VisualElement optionItem = new VisualElement();

                optionItem.AddToClassList("option-item");

                Label nameField = new Label();
                nameField.name = "Name";
                nameField.AddToClassList("option-label");
                optionItem.Add(nameField);

                // Value
                Label valueField = new Label();
                valueField.name = "Value";
                valueField.AddToClassList("option-value");
                optionItem.Add(valueField);

                //add button for editing
                Button editButton = new Button();
                editButton.name = "Edit";
                editButton.text = "Edit";

                optionItem.Add(editButton);

                categoryItem.Add(optionItem);

                return categoryItem;
            };

            optionList.bindItem = (element, i) =>
            {
                VisualElement item = element as VisualElement;

                try
                {
                    Label nameField = item.Q<Label>("Name");
                    nameField.text = _options[i]?.m_Name ?? "OptionName";
                    Label valueField = item.Q<Label>("Value");
                    valueField.text = _options[i]?.m_Value ?? "OptionValue";

                    Button editButton = item.Q<Button>("Edit");
                    editButton.clicked += () =>
                    {
                        //open a new window for editing and pass the option
                        OptionItemWindowEditor.ShowExample(_options[i], myComponent);
                    };
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }
            };

            // Return the finished Inspector UI.
            return myInspector;
        }
    }
}
