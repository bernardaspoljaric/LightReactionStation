using System;
using TMPro;
using UnityEngine;

namespace Novena.Admin.Prefabs.AppVersion
{
    [RequireComponent(typeof(TMP_Text))]
    public class AppVersion : MonoBehaviour
    {
        private TMP_Text _versionNumberTMPText;
        private void Awake()
        {
            try
            {
                _versionNumberTMPText = GetComponent<TMP_Text>();
                _versionNumberTMPText.text = Application.version;
            }
            catch (Exception e)
            {
                print(e);
            }
        }
    }
}
