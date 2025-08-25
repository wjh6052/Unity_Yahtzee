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

    [Header("��ũ�� ��")]
    public Transform RoomList_Content; // Room ������ ��ũ�Ѻ�


    [Header("�г��� �Է�")]
    public TMP_InputField PlayerName_InputField;  // �÷��̾��� �г���

    [Header("�����ϱ� ��ư")]
    public Button EnterRoom_Button;


    [Header("�游���")]
    public Button MackRoom_Button;      // �游��� ����
    public GameObject RoomMake_Panel;   // �� �����
    public Button BackRoomMake_Button;      // �游��� ����
    public TMP_InputField RoomName_InputField;  // ���� �г���
    public TMP_Dropdown MaxPlayer_Dropdown; // �ִ� �ο���


    // ����� ���
    List<RoomListButton_UI> RoomList = new List<RoomListButton_UI>();
    RoomListButton_UI ChoiceRoomList; // ���õ� ������

    private void Awake()
    {
        SetLoadingStatus(false);
    }


    private void Start()
    {
        // �����ϱ� ��ư
        {
            EnterRoom_Button.onClick.AddListener(OnClickEnterRoomButton);
            EnterRoom_Button.gameObject.SetActive(false);
        }

        // �� ����� ��ư
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


    // ��� ����Ʈ�� ����
    public void SetRoomList(List<RoomInfo> newRoomList)
    {
        // ������ ����� ����
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

    // �� ����� ��ư Ŭ��
    void OnClickMackRoomButton()
    {
        if(RoomMake_Panel.activeSelf)
        {
            // �г����� �Է��� �ȵ� ���
            if (PlayerName_InputField.text == "")
            {
                UICanvas_Mgr.Inst.SystemMessages("�г����� �Էµ��� �ʾҽ��ϴ�");
                return;
            }

            // ���� �̸��� �Է��� �ȵ� ���
            if (RoomName_InputField.text == "")
            {
                UICanvas_Mgr.Inst.SystemMessages("�� �̸��� �Էµ��� �ʾҽ��ϴ�");
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

    // �� ����� ��ư ���� ��ư Ŭ��
    void CloseMackRoom()
    {
        RoomName_InputField.text = "";


        RoomMake_Panel.gameObject.SetActive(false);
    }


    // �� ����Ʈ ��ư�� Ŭ�������� ȣ��
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


        // ChoiceRoomList�� Null�� �ƴ϶�� �����ϱ� ��ư Ȱ��ȭ
        EnterRoom_Button.gameObject.SetActive(ChoiceRoomList != null);
    }


    // �����ϱ� ��ư Ŭ���� ȣ��
    void OnClickEnterRoomButton()
    {
        // �� ����Ʈ�� ���õ��� ���� ���
        if(ChoiceRoomList == null)
        {
            EnterRoom_Button.gameObject.SetActive(false);
            return;
        }


        // �г����� �Է��� �ȵ� ���
        if (PlayerName_InputField.text == "")
        {
            UICanvas_Mgr.Inst.SystemMessages("�г����� �Էµ��� �ʾҽ��ϴ�");
            return;
        }

        PhotonNetwork.NickName = PlayerName_InputField.text;
        PhotonNetwork.JoinRoom(ChoiceRoomList.RoomData.Name);
    }

}
