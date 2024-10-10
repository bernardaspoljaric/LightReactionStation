using System;
using Doozy.Engine;
using Novena.UiUtility.Base;
using TMPro;
using UnityEngine;

namespace Novena.Admin.Pages.Login
{
    public class LoginView : UiController
    {
        [Header("Password input field")]
        [SerializeField] private TMP_InputField _inputField;

        private LoginController _loginController = new LoginController();

        void OnEnable()
        {
            _inputField.onValueChanged.AddListener(CheckPassword);
        }

        void OnDestroy()
        {
            _inputField.onValueChanged.RemoveListener(CheckPassword);
        }

        public override void OnShowViewFinished()
        {
/* #if UNITY_EDITOR
            GameEventMessage.SendEvent("GoToControlPanel");
#endif */
        }

        private void CheckPassword(String text)
        {
            var isValid = _loginController.IsPasswordValid(text);

            if (isValid)
            {
                GameEventMessage.SendEvent("GoToControlPanel");
            }
        }
    }
}
