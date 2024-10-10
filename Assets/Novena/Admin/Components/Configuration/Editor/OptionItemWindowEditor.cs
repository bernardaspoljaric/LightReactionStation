using System.Linq;
using Novena;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionItemWindowEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    public static Option m_option;

    public static string[] m_Values;
    private static ConfigurationData _configData;

    [MenuItem("Window/UI Toolkit/OptionItemWindowEditor")]
    public static void ShowExample(Option option, ConfigurationData configData)
    {
        m_option = option;
        _configData = configData;
        OptionItemWindowEditor wnd = GetWindow<OptionItemWindowEditor>();
        wnd.titleContent = new GUIContent("Option Editor");
    }

    private void OnDestroy()
    {
        m_option = null;
        Debug.Log("Window closed");
    }

    private void ValuesListViewBuild()
    {
        VisualElement root = rootVisualElement;
        ListView valuesListView = root.Q<ListView>("ValuesListView");

        valuesListView.itemsSource = m_option.m_Values;
        
        valuesListView.makeItem = () =>
        {
            var item = new TextField();
            return item;
        };

        valuesListView.bindItem = (element, i) =>
        {
            TextField textField = element as TextField;
            textField.value = m_option.m_Values[i];
            textField.RegisterValueChangedCallback(
                (evt) =>
                {
                    m_option.m_Values[i] = evt.newValue;
                }
            );
        };
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        m_VisualTreeAsset.CloneTree(root);

        TextField nameField = root.Q<TextField>("NameText");
        nameField.value = m_option.m_Name;
        nameField.RegisterValueChangedCallback(
            (evt) =>
            {
                m_option.m_Name = evt.newValue;
            }
        );

        TextField descField = root.Q<TextField>("DescriptionText");
        descField.value = m_option.m_Description;
        descField.RegisterValueChangedCallback(
            (evt) =>
            {
                m_option.m_Description = evt.newValue;
            }
        );

        var category = _configData.m_Categories.FirstOrDefault();
        if (m_option.m_CategoryID != 0 && category != null)
        {
            m_option.m_CategoryID = category.m_ID;
        }
        string categoryName = _configData.m_Categories
            .FirstOrDefault(x => x.m_ID == m_option.m_CategoryID)
            ?.m_Name;

        //CategoryDropDown
        DropdownField categoryDropdown = root.Q<DropdownField>("CategoryDropDown");
        categoryDropdown.choices = _configData.m_Categories.Select(x => x.m_Name).ToList();
        categoryDropdown.value = categoryName;
        categoryDropdown.RegisterValueChangedCallback(
            (evt) =>
            {
                m_option.m_CategoryID = _configData.m_Categories
                    .First(x => x.m_Name == evt.newValue)
                    .m_ID;
            }
        );

        var stringField = root.Q<TextField>("ValueString");
        var intField = root.Q<IntegerField>("ValueInteger");
        var floatField = root.Q<FloatField>("ValueFloat");
        var boolField = root.Q<Toggle>("ValueToggle");
        var vector2Field = root.Q<Vector2Field>("ValueVector2");
        var vector3Field = root.Q<Vector3Field>("ValueVector3");

        var sliderElement = root.Q<VisualElement>("SliderVisualElement");
        var rangeField = sliderElement.Q<Slider>("ValueSlider");
        var rangeMinField = sliderElement.Q<IntegerField>("SliderMinField");
        var rangeMaxField = sliderElement.Q<IntegerField>("SliderMaxField");

        ValuesListViewBuild();

        //ValueTypeDropDown
        DropdownField valueTypeDropdown = root.Q<DropdownField>("ValueTypeDropDown");
        valueTypeDropdown.choices = System.Enum.GetNames(typeof(ValueType)).ToList();
        valueTypeDropdown.value = m_option.m_ValueType.ToString();
        valueTypeDropdown.RegisterValueChangedCallback(
            (evt) =>
            {
                m_option.m_ValueType = (ValueType)System.Enum.Parse(typeof(ValueType), evt.newValue);

                //Hide all fields
                stringField.style.display = DisplayStyle.None;
                intField.style.display = DisplayStyle.None;
                floatField.style.display = DisplayStyle.None;
                boolField.style.display = DisplayStyle.None;
                vector2Field.style.display = DisplayStyle.None;
                vector3Field.style.display = DisplayStyle.None;
                sliderElement.style.display = DisplayStyle.None;

                //Show selected field
                switch (m_option.m_ValueType)
                {
                    case ValueType.String:
                        stringField.style.display = DisplayStyle.Flex;
                        stringField.value = ConfigValueUtil.GetValue<string>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(stringField.value);
                        stringField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Int:
                        intField.style.display = DisplayStyle.Flex;
                        intField.value = ConfigValueUtil.GetValue<int>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(intField.value);
                        intField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Float:
                        floatField.style.display = DisplayStyle.Flex;
                        floatField.value = ConfigValueUtil.GetValue<float>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(floatField.value);
                        floatField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Bool:
                        boolField.style.display = DisplayStyle.Flex;
                        boolField.value = ConfigValueUtil.GetValue<bool>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(boolField.value);
                        boolField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Vector2:
                        vector2Field.style.display = DisplayStyle.Flex;
                        vector2Field.value = ConfigValueUtil.GetValue<Vector2>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(vector2Field.value);
                        vector2Field.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Vector3:
                        vector3Field.style.display = DisplayStyle.Flex;
                        vector3Field.value = ConfigValueUtil.GetValue<Vector3>(m_option.m_Value);
                        m_option.m_Value = ConfigValueUtil.SetValue(vector3Field.value);
                        vector3Field.RegisterValueChangedCallback(
                            (e) =>
                            {
                                m_option.m_Value = ConfigValueUtil.SetValue(e.newValue);
                            }
                        );
                        break;
                    case ValueType.Range:
                        sliderElement.style.display = DisplayStyle.Flex;
                        var range = ConfigValueUtil.GetValue<Vector3>(m_option.m_Value);
                        rangeField.value = range.y;
                        rangeField.lowValue = range.x;
                        rangeField.highValue = range.z;

                        rangeMinField.value = (int)range.x;
                        rangeMinField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                rangeField.lowValue = e.newValue;
                                Vector3 rangeValue = new Vector3(e.newValue, rangeField.value, rangeField.highValue);
                                m_option.m_Value = ConfigValueUtil.SetValue(rangeValue);
                            }
                        );

                        rangeMaxField.value = (int)range.z;
                        rangeMaxField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                rangeField.highValue = e.newValue;
                                Vector3 rangeValue = new Vector3(rangeField.lowValue, rangeField.value, e.newValue);
                                m_option.m_Value = ConfigValueUtil.SetValue(rangeValue);
                            }
                        );

                        m_option.m_Value = ConfigValueUtil.SetValue(range);
                        rangeField.RegisterValueChangedCallback(
                            (e) =>
                            {
                                Vector3 rangeValue = new Vector3(rangeField.lowValue, e.newValue, rangeField.highValue);
                                m_option.m_Value = ConfigValueUtil.SetValue(rangeValue);
                            }
                        );
                        break;
                }
            }
        );
    }
}
