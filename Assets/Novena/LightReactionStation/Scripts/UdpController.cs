using UnityEngine;

namespace Novena
{
  public class UdpController : MonoBehaviour
  {
    private NUDP _nudp;

    private void Awake()
    {
      OutputController.OnUdpSignalSend += SendData;
      GameManager.OnGameEnd += SendData;
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
      OutputController.OnUdpSignalSend -= SendData;
      GameManager.OnGameEnd -= SendData;
    }

    public void OnDataReceived(string message)
    {
      if(message == "start\r")
        GameManager.Instance.StartGame();

      if(message == _nudp.LastSentMessage + "\r")
      {
        GameManager.Instance.OnLightClicked();
      }
    }

    private void SendData(string message)
    {
      _nudp.SendData(message);
    }
  }
}
