using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace Novena.Components.Dialog
{
    [RequireComponent(typeof(TMP_Text))]
    public class DialogUi : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _bodyText;
        [SerializeField] private UIButton _closeBtn;

        [Header("Buttons components")]
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private UIButton _buttonPrefab;

        private void Awake()
        {
            _setCloseBtn();
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Set default close btn click event.
        /// </summary>
        private void _setCloseBtn()
        {
            _closeBtn.OnClick.OnTrigger.Event.AddListener(() =>
            {
                Close();
            });
        }

        /// <summary>
        /// Initialize dialog!
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Text"></param>
        /// <param name="buttons"></param>
        public void Setup(string Title, string Text, List<DialogButton> buttons)
        {
            try
            {
                _titleText.text = Title;
                _bodyText.text = Text;

                if (buttons != null && buttons.Any())
                {
                    _generateButtons(buttons: buttons);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void _generateButtons(List<DialogButton> buttons)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var dialogButton = buttons[i];
                
                UIButton button = Instantiate(_buttonPrefab, _buttonsContainer);
                button.OnClick.OnTrigger.Event.AddListener(() => dialogButton.Action());
                button.OnClick.OnTrigger.Event.AddListener(Close);
                button.SetLabelText(dialogButton.Label);
                button.gameObject.SetActive(true);
            }
        }
    }
}
