using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListButton_UI : MonoBehaviour
{
    public Button RoomListButton;
    public TextMeshProUGUI RoomName_Text;
    public TextMeshProUGUI PlayerNum_Text;

    public RoomInfo RoomData;

    public void SetRoomListButton(RoomInfo roomData, LoadingPanel_UI owner)
    {
        RoomData = roomData;

        RoomName_Text.text = RoomData.Name;
        PlayerNum_Text.text = $"{RoomData.PlayerCount} / {RoomData.MaxPlayers}";

        ChoiceRoomListButton(false);

        RoomListButton.onClick.AddListener(()=>
            {
                owner.ChoiceRoomListButton(this);
            });
    }

    public void ChoiceRoomListButton(bool isChoice)
    {
        if (isChoice)
            RoomListButton.image.color = new Color32(255, 255, 150, 255); // ¿¬³ë¶û
        else
            RoomListButton.image.color = new Color32(255, 255, 255, 255); // Èò»ö
    }
}
