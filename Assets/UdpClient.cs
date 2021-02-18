using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class UdpClient : MonoBehaviour
{
    ToolDelegate.String recvCB = null;
    private Socket socket = null;
    private EndPoint targetEP = null;
    private string m_ip = "127.0.0.1";
    private int m_port = 8080;
    private bool isRunning = false;

    public void Init(string targetIP, ToolDelegate.String _recvCB)
    {
        recvCB = _recvCB;

        Quit();

        if (!string.IsNullOrEmpty(m_ip))
        {
            m_ip = targetIP;
        }

        targetEP = (EndPoint)(new IPEndPoint(IPAddress.Parse(m_ip), m_port));
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        Send("init");
    }

    public void Send(string info)
    {
        try
        {
            byte[] byData = System.Text.Encoding.UTF8.GetBytes(info);

            socket.SendTo(byData, targetEP);

            Debug.Log("发送 " + info);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("send " + e.Message);
        }
    }

    public void Quit()
    {
        if (isRunning)
        {
            socket.Close();

            Debug.LogWarning(m_ip + " server close");
        }
    }
}