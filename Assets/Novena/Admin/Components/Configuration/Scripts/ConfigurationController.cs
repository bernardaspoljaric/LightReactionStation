using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Novena
{
    public class ConfigurationController : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset _optionListItemTemplate;
        private ConfigurationData _configurationData;
        private Category _selectedCategory;
        private List<Option> _options = new List<Option>();

        #region Ui Elements

        private VisualElement _root;
        private Label _labelCategoryTitle;
        private VisualElement _menuContainer;
        private ScrollView _optionsList;

        #endregion

        void Awake()
        {
            _configurationData = NConfiguration.Instance.ConfigurationData;
            _root = GetComponent<UIDocument>().rootVisualElement;
            Initialize();
        }

        private void Initialize()
        {
            SetUiElements();
        }

        private void SetUiElements()
        {
            //get element from uxml
            _labelCategoryTitle = _root.Q<Label>("LabelCategoryTitle");
            _labelCategoryTitle.text = _configurationData.m_Categories[0].m_Name;

            SetupMenu();
            SetupOptions();
        }

        private void SetupMenu()
        {
            _menuContainer = _root.Q<VisualElement>("MenuContainer");
            _menuContainer.Clear();

            _selectedCategory = _configurationData.m_Categories[0];

            foreach (var category in _configurationData.m_Categories)
            {
                var btn = new Button();
                btn.text = category.m_Name;
                btn.AddToClassList("button");

                if (_selectedCategory == category)
                {
                    btn.AddToClassList("active");
                }

                btn.clicked += () =>
                {
                    _labelCategoryTitle.text = category.m_Name;
                    _selectedCategory = category;

                    //remove active class from other buttons
                    foreach (var child in _menuContainer.Children())
                    {
                        if (child != btn)
                        {
                            child.RemoveFromClassList("active");
                        }
                    }

                    //add btn style class .active
                    btn.AddToClassList("active");

                    SetupOptions();
                };
                _menuContainer.Add(btn);
            }
        }

        private void SetupOptions()
        {
            _optionsList = _root.Q<ScrollView>("OptionsList");
            _optionsList.Clear();

            _options = _configurationData.m_Options.FindAll(
                x => x.m_CategoryID == _selectedCategory.m_ID
            );

            for (int i = 0; i < _options.Count; i++)
            {
                var item = _optionListItemTemplate.CloneTree();
                item.name = "Option" + _options[i].m_Name;

                DropdownField dropdown = item.Q<DropdownField>("DropdownValue");
                dropdown.style.display = DisplayStyle.None;

                if (_options[i].m_Values != null && _options[i].m_Values.Count > 0)
                {
                    dropdown.style.display = DisplayStyle.Flex;
                    dropdown.choices = _options[i].m_Values;
                    dropdown.value = _options[i].m_Value;
                    dropdown.RegisterValueChangedCallback(
                        (evt) =>
                        {
                            _options[i].m_Value = ConfigValueUtil.SetValue(evt.newValue);
                        }
                    );
                }

                Toggle toggle = item.Q<Toggle>("ToggleValue");
                toggle.style.display = DisplayStyle.None;

                Slider slider = item.Q<Slider>("SliderValue");
                slider.style.display = DisplayStyle.None;

                TextField valueField = item.Q<TextField>("TextValue");
                valueField.style.display = DisplayStyle.None;

                Vector2Field vector2Field = item.Q<Vector2Field>("Vector2Value");
                vector2Field.style.display = DisplayStyle.None;

                Vector3Field vector3Field = item.Q<Vector3Field>("Vector3Value");
                vector3Field.style.display = DisplayStyle.None;

                switch (_options[i].m_ValueType)
                {
                    case ValueType.Bool:
                        toggle.style.display = DisplayStyle.Flex;
                        toggle.value = bool.Parse(_options[i].m_Value);
                        toggle.RegisterValueChangedCallback(
                            (evt) =>
                            {
                                _options[i].m_Value = ConfigValueUtil.SetValue(evt.newValue);
                            }
                        );
                        break;
                    case ValueType.Range:
                        slider.style.display = DisplayStyle.Flex;
                        var range = ConfigValueUtil.GetValue<Vector3>(_options[i].m_Value);
                        slider.lowValue = range.x;
                        slider.highValue = range.z;
                        slider.value = range.y;
                        slider.RegisterValueChangedCallback(
                            (evt) =>
                            {
                                _options[i].m_Value = ConfigValueUtil.SetValue(
                                    new Vector3(slider.lowValue, slider.value, slider.highValue)
                                );
                            }
                        );
                        break;
                        case ValueType.String : case ValueType.Int : case ValueType.Float:
                        valueField.style.display = DisplayStyle.Flex;
                        valueField.value = _options[i].m_Value;
                        valueField.RegisterValueChangedCallback(
                            (evt) =>
                            {
                                _options[i].m_Value = ConfigValueUtil.SetValue(evt.newValue);
                            }
                        );
                        break;
                        case ValueType.Vector2:
                        vector2Field.style.display = DisplayStyle.Flex;
                        vector2Field.value = ConfigValueUtil.GetValue<Vector2>(_options[i].m_Value);
                        vector2Field.RegisterValueChangedCallback(
                            (evt) =>
                            {
                                _options[i].m_Value = ConfigValueUtil.SetValue(evt.newValue);
                            }
                        );
                        break;
                        case ValueType.Vector3:
                        vector3Field.style.display = DisplayStyle.Flex;
                        vector3Field.value = ConfigValueUtil.GetValue<Vector3>(_options[i].m_Value);
                        vector3Field.RegisterValueChangedCallback(
                            (evt) =>
                            {
                                _options[i].m_Value = ConfigValueUtil.SetValue(evt.newValue);
                            }
                        );
                        break;
                    default:
                        break;
                }

                try
                {
                    Label nameField = item.Q<Label>("LabelName");
                    nameField.text = _options[i].m_Name;

                    Label descField = item.Q<Label>("LabelDescription");
                    descField.text = _options[i].m_Description;

                    //valueField.value = _options[i].m_Value;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                }

                _optionsList.contentContainer.Add(item);
            }
        }
    }
}
