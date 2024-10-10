using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Novena.Admin.Prefabs.Applicationinfo
{
    public class ApplicationInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text _content;
         
        private void Awake()
        {
            FillApplicationData();
        }

        void FillApplicationData()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Product name: {Application.productName}");
            stringBuilder.AppendLine($"Identifier: {Application.identifier}");
            stringBuilder.AppendLine($"Application version: {Application.version}");
            stringBuilder.AppendLine($"Company name: {Application.companyName}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Unity version: {Application.unityVersion}");
            stringBuilder.AppendLine($"Data path: {Application.persistentDataPath}");

            _content.text = stringBuilder.ToString();
        }
    }
}
