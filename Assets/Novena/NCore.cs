using UnityEditor;
using UnityEngine;

namespace Novena
{
    /// <summary>
    /// NCore is a singleton class that contains all the Core data and API endpoints for the application.
    /// </summary>
    public class NCore : ScriptableObject
    {
        #region PublicProperties

        /// <summary>
        /// Contains the path to a persistent data directory (Read Only).
        ///<para>Accessible from another thread (ASYNC).</para>
        /// </summary>
        public string PersistentDataPath { get; private set; }

        #endregion //PublicProperties


        private static NCore _instance;
        public static NCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<NCore>("NCore/NCore");
                    if (_instance == null)
                    {
                        Debug.LogError(
                            "NCore not found in resource folder. Please add it to the folder."
                        );
                        return null;
                    }
                }

                return _instance;
            }
        }

        #region Unity methods

        private void OnEnable()
        {
            if (_instance == null)
            {
                //_instance = this;
                Initialize();
            }
        }

        #endregion //Unity methods

        private void Initialize()
        {
            PersistentDataPath = Application.persistentDataPath;
        }

        #region Internal Editor

# if UNITY_EDITOR

        [MenuItem("Novena/Create NCore Asset")]
        static void CreateFolder()
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            string guid = AssetDatabase.CreateFolder("Assets/Resources", "NCore");
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

            NCore nCore = new NCore();
            string newAssetPath = newFolderPath + "/NCore.asset";
            AssetDatabase.CreateAsset(nCore, newAssetPath);
        }

# endif

        #endregion //Internal Editor
    }
}
