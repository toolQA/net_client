using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TcpClient : MonoBehaviour
{
    ToolDelegate.String recvCB = null;
    Socket socket = null;
    IPEndPoint targetEP = null;
    public string targetIP = "127.0.0.1";
    public int targetPort = 8080;

    Thread connectThread = null;

    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];

    public SocketType socketType = SocketType.Stream;
    public AddressFamily addressFamily = AddressFamily.InterNetwork;

    public void Init(string _targetIP, ToolDelegate.String _recvCB)
    {
        recvCB = _recvCB;

        Quit();

        if (!string.IsNullOrEmpty(_targetIP))
        {
            targetIP = _targetIP;
        }

        targetEP = new IPEndPoint(IPAddress.Parse(targetIP), targetPort);
        socket = new Socket(addressFamily, socketType, ProtocolType.Tcp);
        socket.Connect(targetEP);

        connectThread = new Thread(new ThreadStart(Receive));
        connectThread.Start();
    }


    void Receive()
    {
        while (true)
        {
            try
            {
                int len = socket.Receive(recvData);
                if (len > 0)
                {
                    string info = System.Text.Encoding.UTF8.GetString(recvData, 0, len);
                    Debug.Log("接收 " + targetEP.ToString() + " " + info);
                }
            }
            catch (System.Exception e)
            {

            }
        }
    }

    public void Send(string info)
    {
        if (string.IsNullOrEmpty(info))
        {
            return;
        }

        try
        {
            byte[] bData = System.Text.Encoding.UTF8.GetBytes(info);
            socket.Send(bData);
            Debug.Log("发送 " + targetEP.ToString() + " " + info);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("send " + e.Message);
        }
    }

    public void Quit()
    {
        if (socket != null)
        {
            socket.Close();

            socket = null;

            Debug.LogWarning(targetIP + " server close");
        }
    }
}
