using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public UdpClient udpClient = null;

    public GameObject btnGo_sendMsg = null;

    public InputField input_msg = null;

    public InputField input_ip = null;

    // Start is called before the first frame update
    void Start()
    {
        input_ip.onEndEdit.AddListener(JudgeIsCorrectIPV4);

        Tools.SetButton(btnGo_sendMsg, Btn_SendMsg);
    }

    private void JudgeIsCorrectIPV4(string arg0)
    {
        if (Tools.IsCorrectIPV4(arg0))
        {
            udpClient.Init(arg0, GetNetMsg);
        }
        else
        {
            Debug.LogError("【error】server ip is not correct.");
        }
    }

    private void Btn_SendMsg()
    {
        udpClient.Send(input_msg.text);
    }

    private void GetNetMsg(string txt)
    {
        
    }
}
