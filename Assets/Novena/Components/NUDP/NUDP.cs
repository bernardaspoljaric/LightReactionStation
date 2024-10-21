//Author: GoGs

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Novena
{
    public class NUDP : MonoBehaviour
    {
        // The UdpClient object is used for sending and receiving UDP datagrams.
        private UdpClient udpClient;

        // The IPEndPoint represents the IP address and port number of the remote endpoint.
        private IPEndPoint remoteEndPoint;

        // A boolean flag indicating whether the UDP listener is running.
        private bool isRunning = false;

        // A thread-safe queue for actions that need to be executed on the main thread.
        private ConcurrentQueue<Action> mainThreadQueue = new ConcurrentQueue<Action>();

        /// <summary>
        /// The IP address to send messages to.
        /// </summary>
        public string remoteIP = "127.0.0.1"; // IP address to send messages to

        /// <summary>
        /// The port to send messages to.
        /// </summary>
        public int remotePort = 5005; // Port to send messages to

        /// <summary>
        /// The port to listen for incoming messages.
        /// </summary>
        public int localPort = 5005; // Port to listen for incoming messages

        /// <summary>
        /// The last message received.
        /// </summary>
        [field: SerializeField, Tooltip("The last message received.")]
        public string LastReceivedMessage { get; private set; }

        /// <summary>
        /// The last message that was sent.
        /// </summary>
        [field: SerializeField, Tooltip("The last message that was sent.")]
        public string LastSentMessage { get; private set; }

        /// <summary>
        /// Event that is invoked when data is received.
        /// </summary>
        [Space(10)]
        public UnityEvent<string> OnDataReceived;

        public void InitializeUDPClient(string newRemoteIP, int newLocalPort, int newRemotePort)
        {
            StopListening();
            udpClient?.Close();

            remoteIP = newRemoteIP;
            localPort = newLocalPort;
            remotePort = newRemotePort;

            // InitializeUDP
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
            udpClient = new UdpClient(localPort);

            StartListening();
        }

        /// <summary>
        /// Start listening for incoming messages.
        /// </summary>
        public void StartListening()
        {
            isRunning = true;
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        /// <summary>
        /// Stop listening for incoming messages.
        /// </summary>
        public void StopListening()
        {
            isRunning = false;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (isRunning)
            {
                byte[] receiveBytes = udpClient.EndReceive(ar, ref remoteEndPoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                mainThreadQueue.Enqueue(() => DataReceived(receiveString));

                // Restart listening for new messages
                udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
        }

        private void DataReceived(string data)
        {
            //Debug.Log("Data received: " + data);
            LastReceivedMessage = data;
            OnDataReceived?.Invoke(data);
        }

        public void SendData(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            byte[] messageWithCR = new byte[messageBytes.Length + 1];
            Buffer.BlockCopy(messageBytes, 0, messageWithCR, 0, messageBytes.Length);
            messageWithCR[messageBytes.Length] = 0x0D; // Dodavanje \r (carriage return)

            try
            {
                int bytesToSend = messageWithCR.Length;
                int bytesSent = udpClient.Send(messageWithCR, bytesToSend, remoteEndPoint);

                if (bytesSent != bytesToSend)
                {
                    Debug.LogError(
                        "Not all bytes were sent. Bytes to send: "
                            + bytesToSend
                            + ", Bytes sent: "
                            + bytesSent
                    );
                }

                LastSentMessage = message;
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred while sending data: " + ex.Message);
            }
        }

        private void OnDisable()
        {
            isRunning = false;
            udpClient?.Close();
        }

        private void Update()
        {
            // Execute all actions in the main thread queue.
            while (mainThreadQueue.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }
    }
}
