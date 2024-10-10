using System;
using System.Collections.Generic;
using UnityEngine;

namespace Novena
{
    [CreateAssetMenu(fileName = "ConfigurationData", menuName = "Novena/Configuration/ConfigurationData")]
    public class ConfigurationData : ScriptableObject
    {
        public List<Category> m_Categories;
        public List<Option> m_Options;
    }

    [Serializable]
    public class Category
    {
        public int m_ID;
        public string m_Name;
    }

    [Serializable]
    public class Option
    {
        public string m_Name;
        public string m_Description;
        public int m_CategoryID;
        public ValueType m_ValueType;
        public string m_Value;
        public List<string> m_Values;
    }
}
