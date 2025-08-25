using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;



public class Photon_Mgr : MonoBehaviourPunCallbacks
{
    public int MaxPlayerCount = 4;
    public int RoomPlayerCount = 1;

    List<RoomInfo> RoomList; // ������ �� �������� �����ص� ����

    public static Photon_Mgr Inst;
    private void Awake()
    {
        Inst = this;

        // ���� ���� ����Ȯ��
        if (!PhotonNetwork.IsConnected)
        {
            // 1��. ���� Ŭ���忡 ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 2�� PhotonNetwork.ConnectUsingSettings �Լ� ȣ�⿡ ���� ���� ������ �����ϸ� ȣ��Ǵ� �ݹ� �Լ�
    // PhotinNetwork.LeaveRoom(); ���� ���� ���� ���� �κ�� �����鼭 �� �Լ��� �ڵ����� ȣ��ȴ�.
    public override void OnConnectedToMaster()
    {
        UICanvas_Mgr.Inst.SystemMessages("���� ���� �Ϸ�");
        Debug.Log("���� ���� ���� �Ϸ�");

        // 3�� ���濡�� ������ �ִ� ������ �κ� ���� �õ�
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� ������ �ڵ� ȣ��
    public override void OnJoinedLobby()
    {
        UICanvas_Mgr.Inst.SystemMessages("�κ� ���� �Ϸ�");
        Debug.Log("�κ� ���� �Ϸ�");

        UICanvas_Mgr.Inst.LoadingPanel.SetLoadingStatus(true);
    }


    //������ �� ����� ���� �Ǿ��� �� ȣ��Ǵ� �������̵� �Լ�
    //�� ����Ʈ ������ ���� Ŭ���� �κ񿡼��� �����ϴ�.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;

        if (UICanvas_Mgr.Inst.UIType != EUIType.Loading) return;

        // �� ��� ����Ʈ ����
        UICanvas_Mgr.Inst.LoadingPanel.SetRoomList(RoomList);
    }


    // ����� �Լ� LoadingPanel_UI���� ȣ��
    public void MakeRoom(string inRoomName, int MaxPlayerNum, string PlayerName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;      // ���� ���� ����
        roomOptions.IsVisible = true;   // �κ񿡼� ���� ���� ����
        roomOptions.MaxPlayers = MaxPlayerNum;

        PhotonNetwork.NickName = PlayerName;
        PhotonNetwork.CreateRoom(inRoomName, roomOptions, TypedLobby.Default);
    }

    
    // �� ���� ����
    public override void OnCreatedRoom()
    {
        UICanvas_Mgr.Inst.SystemMessages("�� ���� ����!", 3.5f, true);
        Table_Mgr.Inst.SettingTable(PhotonNetwork.CurrentRoom.MaxPlayers);
    }


    // ���� �濡 ���ö� ���� ȣ��
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");

        
        UICanvas_Mgr.Inst.SetUIType(EUIType.Play); // UI�� �÷��̷� ����
        Game_Mgr.Inst.SpawnPlayer(); // �÷��̾� ĳ���� �濡 ����

    }

    // �ٸ� ������ ���� �� �ڵ�ȣ�� ���� �÷��̾� ��� ��� ȣ���
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UICanvas_Mgr.Inst.SystemMessages($"{newPlayer.NickName}�Բ��� �����Ͽ����ϴ�");

        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);
        Table_Mgr.Inst.PlayerReady();
    }


    // �����÷��̾��� ���°��� �����Ͽ� ������ �����ϴ� ��Ȱ
    public void SetPlayerState(EPlayerState state)
    {
        var props = new ExitGames.Client.Photon.Hashtable();
        props["EPlayerState"] = (int)state;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // �������� SetCustomProperties ���� ����� ������ �� �ȿ� �ִ� ��� �÷��̾��� Ŭ���̾�Ʈ���� �ڵ� ȣ��
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(changedProps.ContainsKey("EPlayerState"))
        {
            EPlayerState state = (EPlayerState)changedProps["EPlayerState"];
            Debug.Log($"{targetPlayer.NickName} ���� ����: {state}");

            // ���̺� ��ȣ ������
            Table_Mgr.Inst.PlayerReady();
        }
    }




    // ���� ������
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // OnLeftRoom() �� ���� ���� ������ �÷��̾ �� ���Ը� ȣ��
    public override void OnLeftRoom()
    {
        UICanvas_Mgr.Inst.SystemMessages("�濡�� ���������� ���Խ��ϴ�", 3.5f, true);

        // ī�޶� �̵�
        Camera_Mgr.Inst.ChangeTarget(Camera_Mgr.Inst.LoadingCameraRoot);
        Camera_Mgr.Inst.SetCameraZoom(0);

        //UI����
        UICanvas_Mgr.Inst.SetUIType(EUIType.Loading);
    }

    // �ٸ� �÷��̾ �濡�� ������ ȣ��
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);
        Table_Mgr.Inst.PlayerReady();
    }

    // ������ ������������ ������� ��(ũ����/��Ʈ��ũ ���� ��) �ڵ� ȣ��
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UICanvas_Mgr.Inst.SystemMessages("������ �����⶧���� ���� �����մϴ�", 3.5f, true);
        LeaveRoom();
    }
}
