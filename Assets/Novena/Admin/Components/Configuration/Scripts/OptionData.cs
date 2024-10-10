using UnityEngine;

namespace Novena
{
    [CreateAssetMenu(fileName = "OptionData", menuName = "Novena/Configuration/OptionData")]
    public class OptionData : ScriptableObject
    {       
        [Tooltip("Default value for this option")]
        public string Value;

        [Tooltip("Possible values for this option")]
        public string[] Values;
        public string Name;
        public string Description;

        private void OnValidate() 
        {
            Save();
        }

        private void Save() 
        {

        }
    }
}