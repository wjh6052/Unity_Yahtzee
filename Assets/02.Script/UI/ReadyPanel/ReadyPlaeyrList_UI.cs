using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadyPlaeyrList_UI : MonoBehaviour
{
    public TextMeshProUGUI ReadyPlayerName_Text;
    public GameObject Ready_Image;
    public GameObject RoomHost_Image;

    public void SetReadyPlaeyrList(string playerName, bool isReady, bool isRoomHost)
    {
        ReadyPlayerName_Text.text = playerName;
        Ready_Image.SetActive(isReady);
        RoomHost_Image.SetActive(isRoomHost);
    }

}
