using System.Collections.Generic;
using System.Linq;
using Novena.Configuration;
using UnityEngine;

namespace Novena
{
    [CreateAssetMenu(fileName = "NConfiguration", menuName = "Novena/Configuration/NConfiguration")]
    public class NConfiguration : ScriptableObject
    {
        public ConfigurationData ConfigurationData;
        public static NConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<NConfiguration>("NConfiguration/NConfiguration");
                    if (_instance == null)
                    {
                        Debug.LogError("NConfiguration not found in Resources folder.");
                        _instance = CreateInstance<NConfiguration>();
                    }
                }
                return _instance;
            }
        }
        private static NConfiguration _instance;

        private ConfigurationRepository _configurationRepository;

        /* public NConfiguration()
        {
            _configurationRepository = new ConfigurationRepository();
        }  */

        private void ApplayConfiguration()
        {
            /* var screenMode = Options.Find(x => x.Name == "Screen mode");
            var screenResolution = Options.Find(x => x.Name == "Screen resolution");

            var resolution = screenResolution.Value.Split('x');
            Screen.fullScreenMode =
                screenMode.Value == "Fullscreen"
                    ? FullScreenMode.ExclusiveFullScreen
                    : FullScreenMode.Windowed;
            Screen.SetResolution(
                int.Parse(resolution[0]),
                int.Parse(resolution[1]),
                Screen.fullScreenMode
            ); */
        }
    }
}
