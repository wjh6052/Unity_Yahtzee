using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatPanel_UI : MonoBehaviour
{
    public TextMeshProUGUI Chat_Text;
    public TMP_InputField Chat_InputField;


    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        Chat_Text.text = "";
        SetChatPanel(false);
    }

    public void SetChatPanel(bool isOn)
    {
        if(isOn)
        {
            Chat_InputField.gameObject.SetActive(true);
            Chat_InputField.ActivateInputField();   //<-- 키보드 커서 입력 상자 쪽으로 가게 만들어 줌
        }
        else
        {
            Chat_InputField.gameObject.SetActive(false);
            if (string.IsNullOrEmpty(Chat_InputField.text.Trim()) == false)
            {
                BroadcastingChat();
            }

        }
    }

    void BroadcastingChat()
    {
        string msg = "\n<color=#ffffff>[" +
                        PhotonNetwork.LocalPlayer.NickName + "] " +
                        Chat_InputField.text + "</color>";

        pv.RPC("RPC_LogMsg", RpcTarget.AllBuffered, msg, true);

        Chat_InputField.text = "";
    }

    List<string> m_MsgList = new List<string>();
    [PunRPC]
    void RPC_LogMsg(string msg, bool isChatMsg, PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal == true && isChatMsg == true)
        {
            //내가 쓴 글씨는 노란색으로 바꿔서 표시해 주기...
            msg = msg.Replace("#ffffff", "#ffff00");
        }

        m_MsgList.Add(msg);
        if (20 < m_MsgList.Count)
            m_MsgList.RemoveAt(0);

        Chat_Text.text = "";
        for (int i = 0; i < m_MsgList.Count; i++)
        {
            Chat_Text.text += m_MsgList[i];
        }
    }

}
