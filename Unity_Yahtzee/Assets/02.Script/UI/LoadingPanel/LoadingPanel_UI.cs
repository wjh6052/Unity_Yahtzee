using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class LoadingPanel_UI : MonoBehaviour
{
    public GameObject RoomPanel;

    [Header("스크롤 뷰")]
    public Transform RoomList_Content; // Room 보여줄 스크롤뷰


    [Header("닉네임 입력")]
    public TMP_InputField PlayerName_InputField;  // 플레이어의 닉네임

    [Header("입장하기 버튼")]
    public Button EnterRoom_Button;


    [Header("방만들기")]
    public Button MackRoom_Button;      // 방만들기 열기
    public GameObject RoomMake_Panel;   // 방 만들기
    public Button BackRoomMake_Button;      // 방만들기 열기
    public TMP_InputField RoomName_InputField;  // 방의 닉네임
    public TMP_Dropdown MaxPlayer_Dropdown; // 최대 인원수


    // 방들의 목록
    List<RoomListButton_UI> RoomList = new List<RoomListButton_UI>();
    RoomListButton_UI ChoiceRoomList; // 선택된 방정보

    private void Awake()
    {
        SetLoadingStatus(false);
    }


    private void Start()
    {
        // 입장하기 버튼
        {
            EnterRoom_Button.onClick.AddListener(OnClickEnterRoomButton);
            EnterRoom_Button.gameObject.SetActive(false);
        }

        // 방 만들기 버튼
        {
            RoomMake_Panel.gameObject.SetActive(false);

            MackRoom_Button.onClick.AddListener(OnClickMackRoomButton);

            BackRoomMake_Button.onClick.AddListener(CloseMackRoom);
        }
    }

    public void SetLoadingStatus(bool isOn)
    {
        RoomPanel.gameObject.SetActive(isOn);
    }


    // 방들 리스트를 셋팅
    public void SetRoomList(List<RoomInfo> newRoomList)
    {
        // 기존의 방들은 삭제
        foreach (Transform child in RoomList_Content)
            Destroy(child.gameObject);


        RoomListButton_UI roomBtnPrefab = Resources.Load("UI/RoomListButton_UI").GetComponent<RoomListButton_UI>();

        if (roomBtnPrefab == null) return;

        RoomList.Clear();
        foreach (RoomInfo roomInfo in newRoomList)
        {
            if(roomInfo.PlayerCount <= 0 || roomInfo.MaxPlayers <= 0) continue;

            RoomListButton_UI roomBut = Instantiate(roomBtnPrefab, RoomList_Content);
            roomBut.SetRoomListButton(roomInfo, this);
            RoomList.Add(roomBut);
        }

    }

    // 방 만들기 버튼 클릭
    void OnClickMackRoomButton()
    {
        if(RoomMake_Panel.activeSelf)
        {
            // 닉네임이 입력이 안된 경우
            if (PlayerName_InputField.text == "")
            {
                UICanvas_Mgr.Inst.SystemMessages("닉네임이 입력되지 않았습니다");
                return;
            }

            // 방의 이름이 입력이 안된 경우
            if (RoomName_InputField.text == "")
            {
                UICanvas_Mgr.Inst.SystemMessages("방 이름이 입력되지 않았습니다");
                return;
            }

            int maxPlayer = int.Parse(MaxPlayer_Dropdown.options[MaxPlayer_Dropdown.value].text);


            Photon_Mgr.Inst.MakeRoom(RoomName_InputField.text, maxPlayer, PlayerName_InputField.text);

        }
        else
        {
            RoomMake_Panel.gameObject.SetActive(true);
        }
    }

    // 방 만들기 버튼 끄기 버튼 클릭
    void CloseMackRoom()
    {
        RoomName_InputField.text = "";


        RoomMake_Panel.gameObject.SetActive(false);
    }


    // 방 리스트 버튼을 클릭했을때 호출
    public void ChoiceRoomListButton(RoomListButton_UI choice)
    {
        if (ChoiceRoomList == choice)
            ChoiceRoomList = null;
        else
            ChoiceRoomList = choice;

        foreach (RoomListButton_UI room in RoomList)
        {
            room.ChoiceRoomListButton(room == ChoiceRoomList);
        }


        // ChoiceRoomList가 Null이 아니라면 입장하기 버튼 활성화
        EnterRoom_Button.gameObject.SetActive(ChoiceRoomList != null);
    }


    // 입장하기 버튼 클릭시 호출
    void OnClickEnterRoomButton()
    {
        // 방 리스트가 선택되지 않은 경우
        if(ChoiceRoomList == null)
        {
            EnterRoom_Button.gameObject.SetActive(false);
            return;
        }


        // 닉네임이 입력이 안된 경우
        if (PlayerName_InputField.text == "")
        {
            UICanvas_Mgr.Inst.SystemMessages("닉네임이 입력되지 않았습니다");
            return;
        }

        PhotonNetwork.NickName = PlayerName_InputField.text;
        PhotonNetwork.JoinRoom(ChoiceRoomList.RoomData.Name);
    }

}
