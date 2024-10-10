using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Novena.VirtualKeyboard
{
    /// <summary>
    /// Main class for the virtual keyboard.
    /// 
    /// Author: GoGs
    /// </summary>
    public class NVirtualKeyboard : MonoBehaviour
    {
        /// <summary>
        /// The input field that the keyboard will interact with.
        /// </summary>
        [field: SerializeField]
        public TMP_InputField inputField { get; private set; }

        private List<NVirtualKey> _keys = new List<NVirtualKey>();

        private List<NVirtualKeyboardLayout> _layouts = new List<NVirtualKeyboardLayout>();

        private Animator _animator;

        void Awake()
        {
            _keys = GetComponentsInChildren<NVirtualKey>(true).ToList();
            _layouts = GetComponentsInChildren<NVirtualKeyboardLayout>(true).ToList();
            NVirtualKey.OnKeyPress += OnKeyPress;
            _animator = GetComponent<Animator>();
        }

        void OnDestroy()
        {
            NVirtualKey.OnKeyPress -= OnKeyPress;
        }

        public void ShowKeyboard()
        {
             _animator.Play("Show");
        }

        public void HideKeyboard()
        {
            _animator.Play("Hide");
        }     

        #region private methods

        private void OnKeyPress(NVirtualKey key)
        {
            if (inputField == null)
                return;

            if (key.KeyType == NVirtualKeyType.Normal)
            {
                WriteValueToInputFiled(key.Value);
                return;
            }

            InvokeSpecialKey(key);
        }

        private void WriteValueToInputFiled(string value)
        {
            // Get the current text from the input field
            string currentText = inputField.text;

            // Get the caret position (cursor position)
            int caretPosition = inputField.caretPosition;

            // Insert the character at the caret position
            string newText = currentText.Insert(caretPosition, value);

            // Update the text in the input field
            inputField.text = newText;

            // Move the caret position to the right
            inputField.caretPosition = caretPosition + 1;

            //check if the caret position is at the end of the text
            if (inputField.caretPosition == inputField.text.Length)
            {
                inputField.MoveTextEnd(false);
            }
        }

        private void InvokeSpecialKey(NVirtualKey key)
        {
            switch (key.KeyType)
            {
                case NVirtualKeyType.Backspace:
                    Backspace();
                    break;
                case NVirtualKeyType.Shift:
                    Shift();
                    break;
                case NVirtualKeyType.Space:
                    Space();
                    break;
                case NVirtualKeyType.LeftArrow:
                    MoveCaret(false);
                    break;
                case NVirtualKeyType.RightArrow:
                    MoveCaret(true);
                    break;
                case NVirtualKeyType.NumberKeypad:
                    SwitchKeyboardLayout(NVirtualKeyboardLayoutType.NUMBER);
                    break;
                case NVirtualKeyType.NormalKeypad:
                    SwitchKeyboardLayout(NVirtualKeyboardLayoutType.NORMAL);
                    break;
                case NVirtualKeyType.HideKeyboard:
                    inputField = null;
                    HideKeyboard();
                    break;
                default:
                    break;
            }
        }   

        private void SetInputFieldActive()
        {
            if (inputField != null && !inputField.isFocused)
            {
                inputField.ActivateInputField();
            }
        }

        #region Special Keys Methods

        private void Backspace()
        {
            // Get the caret position (cursor position)
            int caretPosition = inputField.caretPosition;

            if (
                inputField.selectionFocusPosition != inputField.caretPosition
                || inputField.selectionAnchorPosition != inputField.caretPosition
            )
            {
                if (inputField.selectionAnchorPosition > inputField.selectionFocusPosition) // right to left
                {
                    inputField.text =
                        inputField.text.Substring(0, inputField.selectionFocusPosition)
                        + inputField.text.Substring(inputField.selectionAnchorPosition);
                    inputField.caretPosition = inputField.selectionFocusPosition;
                }
                else // left to right
                {
                    inputField.text =
                        inputField.text.Substring(0, inputField.selectionAnchorPosition)
                        + inputField.text.Substring(inputField.selectionFocusPosition);
                    inputField.caretPosition = inputField.selectionAnchorPosition;
                }
                caretPosition = inputField.caretPosition;
                inputField.selectionAnchorPosition = caretPosition;
                inputField.selectionFocusPosition = caretPosition;
            }
            else
            {
                caretPosition = inputField.caretPosition;

                if (caretPosition > 0)
                {
                    --caretPosition;
                    inputField.text = inputField.text.Remove(caretPosition, 1);
                    inputField.caretPosition = caretPosition;
                }
            }
        }

        private bool capsEnabled = false;

        /// <summary>
        /// Handles the shift key press.
        /// If the shift key is pressed, the keyboard will change the case of the keys to uppercase.
        /// </summary>
        private void Shift()
        {
            if (capsEnabled == false)
            {
                foreach (NVirtualKey key in _keys)
                {
                    key.SetToCapitallized();
                }

                capsEnabled = true;
            }
            else
            {
                foreach (NVirtualKey key in _keys)
                {
                    key.SetToLowerCase();
                }

                capsEnabled = false;
            }
        }

        private void Space()
        {
            WriteValueToInputFiled(" ");
        }

        private void MoveCaret(bool right)
        {
            if (right)
            {
                if (inputField.caretPosition < inputField.text.Length)
                {
                    inputField.caretPosition += 1;
                }
            }
            else
            {
                if (inputField.caretPosition > 0)
                {
                    inputField.caretPosition -= 1;
                }
            }
        }

        private void SwitchKeyboardLayout(NVirtualKeyboardLayoutType layoutType)
        {
            _layouts.ForEach(
                layout => layout.gameObject.SetActive(layout.LayoutType == layoutType)
            );
        }

        #endregion

        #endregion

        IEnumerator CheckIsInputOrKeyboardClicked()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                TMP_InputField input = results
                    .FirstOrDefault(
                        result => result.gameObject.GetComponent<TMP_InputField>() != null
                    )
                    .gameObject?.GetComponent<TMP_InputField>();

                var isKeyboard = results.Any(
                    result => result.gameObject.GetComponentInParent<NVirtualKeyboard>() != null
                );

                if (isKeyboard)
                {
                    SetInputFieldActive();
                }
                else if (input != null)
                {
                    yield return new WaitForEndOfFrame();

                    if (input.isFocused)
                    {
                        inputField = input;
                        ShowKeyboard();
                    }
                    else
                    {
                        inputField = null;
                        HideKeyboard();
                    }
                }
                else
                {
                    inputField = null;
                    HideKeyboard();
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CheckIsInputOrKeyboardClicked());
            }          
        }
    }
}
