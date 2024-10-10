using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novena.VirtualKeyboard
{
    [RequireComponent(typeof(Button))]
    public class NVirtualKey : MonoBehaviour
    {
        /// <summary>
        /// Event that is triggered when a key is pressed.
        /// </summary>
        public static Action<NVirtualKey> OnKeyPress;

        [field: SerializeField]
        public string Value { get; private set; }

        [SerializeField]
        public NVirtualKeyType KeyType = NVirtualKeyType.Normal;

        #region Private Variables

        private Button _button;

        [SerializeField]
        private TMP_Text m_labelText;

        [SerializeField]
        private Image m_iconImage;

        #endregion

        #region Unity Methods
        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnKey_Pressed);
        }

        void OnDestroy()
        {
            _button.onClick.RemoveListener(OnKey_Pressed);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (m_labelText != null)
            {
                SetText(Value);
            }

            string titlePart = KeyType != NVirtualKeyType.Normal ? "Special" : "";
            gameObject.name = "Key - " + titlePart + " " + Value;
        }

#endif

        #endregion

        public void OnKey_Pressed()
        {
            OnKeyPress?.Invoke(this);
        }

        public void SetToCapitallized()
        {
            Value = Value.ToUpper();
            SetText(Value);
        }

        public void SetToLowerCase()
        {
            Value = Value.ToLower();
            SetText(Value);
        }

        #region Private Methods

        private void SetText(string text)
        {
            if (m_labelText != null)
            {
                m_labelText.text = text;
            }
        }

        #endregion
    }
}
