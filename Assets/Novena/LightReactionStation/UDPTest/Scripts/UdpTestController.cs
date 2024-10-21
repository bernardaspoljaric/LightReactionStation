using System;
using UnityEngine;

namespace Novena
{
  public class UdpTestController : MonoBehaviour
  {
    public static Action<string> OnLightColorChange;
    public static Action OnGameEnd;
    private NUDP _nudp;

    private void Awake()
    {
      LightTest.OnButtonClick += SetMessage;
      GameTestController.OnStart += SendData;
    }
    private void Start()
    {
      _nudp = GetComponent<NUDP>();

      Settings.Settings.LoadSettings();
      string remoteIP = Settings.Settings.GetValue<string>("RemoteIP");
      int localPort = Settings.Settings.GetValue<int>("LocalPort");
      int remotePort = Settings.Settings.GetValue<int>("RemotePort");

      _nudp.InitializeUDPClient(remoteIP, localPort, remotePort);
    }

    private void OnDestroy()
    {
      LightTest.OnButtonClick -= SetMessage;
      GameTestController.OnStart -= SendData;
    }

    public void OnDataReceived(string message)
    {
      if (message.Contains("Light")) 
        OnLightColorChange?.Invoke(message);

      if (message == "end")
        OnGameEnd?.Invoke();
    }

    private void SetMessage()
    {
      var message = _nudp.LastReceivedMessage;
      SendData(message);
    }

    private void SendData(string message)
    {
      _nudp.SendData(message);
    }
  }
}
