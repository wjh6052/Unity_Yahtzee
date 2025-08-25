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

    List<RoomInfo> RoomList; // 서버의 방 정보들을 저장해둘 변수

    public static Photon_Mgr Inst;
    private void Awake()
    {
        Inst = this;

        // 포톤 서버 접속확인
        if (!PhotonNetwork.IsConnected)
        {
            // 1번. 포톤 클라우드에 접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 2번 PhotonNetwork.ConnectUsingSettings 함수 호출에 대한 서버 접속이 성공하면 호출되는 콜백 함수
    // PhotinNetwork.LeaveRoom(); 으로 방을 떠날 때도 로비로 나오면서 이 함수가 자동으로 호출된다.
    public override void OnConnectedToMaster()
    {
        UICanvas_Mgr.Inst.SystemMessages("서버 접속 완료");
        Debug.Log("포톤 서버 접속 완료");

        // 3번 포톤에서 제공해 주는 가상의 로비에 접속 시도
        PhotonNetwork.JoinLobby();
    }

    // 로비 접속 성공후 자동 호출
    public override void OnJoinedLobby()
    {
        UICanvas_Mgr.Inst.SystemMessages("로비 접속 완료");
        Debug.Log("로비 접속 완료");

        UICanvas_Mgr.Inst.LoadingPanel.SetLoadingStatus(true);
    }


    //생성된 룸 목록이 변경 되었을 때 호출되는 오버라이드 함수
    //방 리스트 갱신은 포톤 클라우드 로비에서만 가능하다.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomList = roomList;

        if (UICanvas_Mgr.Inst.UIType != EUIType.Loading) return;

        // 방 목록 리스트 세팅
        UICanvas_Mgr.Inst.LoadingPanel.SetRoomList(RoomList);
    }


    // 방생성 함수 LoadingPanel_UI에서 호출
    public void MakeRoom(string inRoomName, int MaxPlayerNum, string PlayerName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;      // 입장 가능 여부
        roomOptions.IsVisible = true;   // 로비에서 룸의 노출 여부
        roomOptions.MaxPlayers = MaxPlayerNum;

        PhotonNetwork.NickName = PlayerName;
        PhotonNetwork.CreateRoom(inRoomName, roomOptions, TypedLobby.Default);
    }

    
    // 방 생성 성공
    public override void OnCreatedRoom()
    {
        UICanvas_Mgr.Inst.SystemMessages("방 생성 성공!", 3.5f, true);
        Table_Mgr.Inst.SettingTable(PhotonNetwork.CurrentRoom.MaxPlayers);
    }


    // 내가 방에 들어올때 나만 호출
    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");

        
        UICanvas_Mgr.Inst.SetUIType(EUIType.Play); // UI를 플레이로 변경
        Game_Mgr.Inst.SpawnPlayer(); // 플레이어 캐릭터 방에 스폰

    }

    // 다른 유저가 들어올 때 자동호출 들어온 플레이어 재외 모두 호출됨
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UICanvas_Mgr.Inst.SystemMessages($"{newPlayer.NickName}님께서 입장하였습니다");

        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);
        Table_Mgr.Inst.PlayerReady();
    }


    // 로컬플레이어의 상태값을 변경하여 서버에 전달하는 역활
    public void SetPlayerState(EPlayerState state)
    {
        var props = new ExitGames.Client.Photon.Hashtable();
        props["EPlayerState"] = (int)state;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // 누군가의 SetCustomProperties 값이 변경될 때마다 방 안에 있는 모든 플레이어의 클라이언트에서 자동 호출
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(changedProps.ContainsKey("EPlayerState"))
        {
            EPlayerState state = (EPlayerState)changedProps["EPlayerState"];
            Debug.Log($"{targetPlayer.NickName} 상태 변경: {state}");

            // 테이블에 신호 보내기
            Table_Mgr.Inst.PlayerReady();
        }
    }




    // 방을 나갈때
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // OnLeftRoom() 은 방을 떠난 “로컬 플레이어” 한 명에게만 호출
    public override void OnLeftRoom()
    {
        UICanvas_Mgr.Inst.SystemMessages("방에서 성공적으로 나왔습니다", 3.5f, true);

        // 카메라 이동
        Camera_Mgr.Inst.ChangeTarget(Camera_Mgr.Inst.LoadingCameraRoot);
        Camera_Mgr.Inst.SetCameraZoom(0);

        //UI수정
        UICanvas_Mgr.Inst.SetUIType(EUIType.Loading);
    }

    // 다른 플레이어가 방에서 나갈때 호출
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);
        Table_Mgr.Inst.PlayerReady();
    }

    // 방장이 비정상적으로 사라졌을 때(크래시/네트워크 끊김 등) 자동 호출
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UICanvas_Mgr.Inst.SystemMessages("방장이 나갔기때문에 방을 제거합니다", 3.5f, true);
        LeaveRoom();
    }
}
