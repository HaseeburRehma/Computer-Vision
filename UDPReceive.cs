using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public bool startReceiving = true;
    public bool printToConsole = false;

    private GestureManager gestureManager;

    private void Start()
    {
        gestureManager = GestureManager.Instance;

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startReceiving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                string newData = Encoding.UTF8.GetString(dataByte);

                if (printToConsole)
                {
                    print(newData);
                }

                // Store the received gesture data in GestureManager
                gestureManager.SetRecognizedGesture(newData);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}








/*
    public void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // Receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startReceiving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                string newData = Encoding.UTF8.GetString(dataByte);

                if (printToConsole)
                {
                    print(newData);
                }

                // Store the latest received gesture data
                latestGestureData = newData;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // Method to get the latest gesture data
    public string GetLatestGestureData()
    {
        return latestGestureData;
    }
} 

     */