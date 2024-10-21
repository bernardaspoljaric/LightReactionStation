using Novena;
//using Novena.Settings;
using UnityEngine;

public class UDPController : MonoBehaviour
{
    private NUDP nudp;
    private void Awake()
    {
        nudp = GetComponent<NUDP>();
    }
    private void Start()
    {
        //Settings.LoadSettings();
        //string remoteIP = Settings.GetValue<string>("RemoteIP");
        //int localPort = Settings.GetValue<int>("LocalPort");
        //int remotePort = Settings.GetValue<int>("RemotePort");

        //nudp.InitializeUDPClient(remoteIP, localPort, remotePort);
    }
}

// 1. Add properties to Settings
// 2. Add this method to Settings.cs
/*
public static void LoadSettings()
{
    _settingsManager.LoadSettings();
}
*/