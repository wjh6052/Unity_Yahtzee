using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EScoreType
{
    Aces,
    Twos,
    Threes,
    Fours,
    Fives,
    Sixes,

    BonusTotal,
    Bonus,

    Chance,
    Three_Of_A_Kind,
    Four_Of_A_Kind,
    Full_House,
    Small_Straight,
    Large_Straight,
    Yahtzze,

    
    Total,
    Count
}

public class ScoreCombination
{
    public int Aces = -1;    // 1이 나온 합
    public int Twos = -1;    // 2이 나온 합
    public int Threes = -1;  // 3이 나온 합
    public int Fours = -1;   // 4이 나온 합
    public int Fives = -1;   // 5이 나온 합
    public int Sixes = -1;   // 6이 나온 합

    public int BonusTotal = 0;   // 1~6의 합   Bonus에 적용 전 1~6을 더해둘 함수
    public int Bonus = 0;   // 1~6의 합   합점수 <= 63 -> 35고정

    public int Chance = -1;          // 현재 주사위의 합
    public int Three_Of_A_Kind = -1; // 같은 숫자 3개 이상의 합
    public int Four_Of_A_Kind = -1;  // 같은 숫자 4개 이상의 합
    public int Full_House = -1;      // 같은 숫자 3개 + 다른 숫자 2개의 합
    public int Small_Straight = -1;  // 연속된 숫자 4개의 합     30점 고정
    public int Large_Straight = -1;  // 연속된 숫자 5개의 합     40점 고정
    public int Yahtzze = -1;         // 5개 모두 같은 숫자       50점 고정

    public int Total = 0;

    // 보너스와 토탈점수를 업데이트 시키는 함수
    public void ScoreUpdate()
    {
        BonusTotal =
            (
            (Aces == -1 ? 0 : Aces) +
            (Twos == -1 ? 0 : Twos) +
            (Threes == -1 ? 0 : Threes) +
            (Fours == -1 ? 0 : Fours) +
            (Fives == -1 ? 0 : Fives) +
            (Sixes == -1 ? 0 : Sixes)
            );
        Bonus = (BonusTotal >= 63) ? 35 : 0;

        Total =
            (
                (BonusTotal == -1 ? 0 : BonusTotal) +
                (Bonus == -1 ? 0 : Bonus) +
                (Chance == -1 ? 0 : Chance) +
                (Three_Of_A_Kind == -1 ? 0 : Three_Of_A_Kind) +
                (Four_Of_A_Kind == -1 ? 0 : Four_Of_A_Kind) +
                (Full_House == -1 ? 0 : Full_House) +
                (Small_Straight == -1 ? 0 : Small_Straight) +
                (Large_Straight == -1 ? 0 : Large_Straight) +
                (Yahtzze == -1 ? 0 : Yahtzze)
            );
    }


    // EScoreType을 받으면 해당 점수를 리턴해주는 역활
    public int GetScore(EScoreType InScoreType)
    {
        switch(InScoreType)
        {
            case EScoreType.Aces:
                return Aces;
            case EScoreType.Twos:
                return Twos;
            case EScoreType.Threes:
                return Threes;
            case EScoreType.Fours:
                return Fours;
            case EScoreType.Fives:
                return Fives;
            case EScoreType.Sixes:
                return Sixes;
            case EScoreType.BonusTotal:
                return BonusTotal;
            case EScoreType.Bonus:
                return Bonus;
            case EScoreType.Three_Of_A_Kind:
                return Three_Of_A_Kind;
            case EScoreType.Four_Of_A_Kind:
                return Four_Of_A_Kind;
            case EScoreType.Full_House:
                return Full_House;
            case EScoreType.Small_Straight:
                return Small_Straight;
            case EScoreType.Large_Straight:
                return Large_Straight;
            case EScoreType.Yahtzze:
                return Yahtzze;
            case EScoreType.Chance:
                return Chance;
            case EScoreType.Total:
                return Total;
        }

        return -1;
    }

    // EScoreType과 점수를 받아 해당 점수에 적용해주는 역활
    public void SetScore(EScoreType InScoreType, int Score)
    {
        switch (InScoreType)
        {
            case EScoreType.Aces:
                Aces = Score;
                break;
            case EScoreType.Twos:
                Twos = Score;
                break;
            case EScoreType.Threes:
                Threes = Score;
                break;
            case EScoreType.Fours:
                Fours = Score;
                break;
            case EScoreType.Fives:
                Fives = Score;
                break;
            case EScoreType.Sixes:
                Sixes = Score;
                break;
            case EScoreType.Bonus:
                Bonus = Score;
                break;
            case EScoreType.Three_Of_A_Kind:
                Three_Of_A_Kind = Score;
                break;
            case EScoreType.Four_Of_A_Kind:
                Four_Of_A_Kind = Score;
                break;
            case EScoreType.Full_House:
                Full_House = Score;
                break;
            case EScoreType.Small_Straight:
                Small_Straight = Score;
                break;
            case EScoreType.Large_Straight:
                Large_Straight = Score;
                break;
            case EScoreType.Yahtzze:
                Yahtzze = Score;
                break;
            case EScoreType.Chance:
                Chance = Score;
                break;
            case EScoreType.Total:
                Total = Score;
                break;
        }

        ScoreUpdate();
        return;
    }

    // int[]를 받아서 점수에 적용
    public void SetScoreArr(int[] scoresArr)
    {
        Aces = scoresArr[0];
        Twos = scoresArr[1];
        Threes = scoresArr[2];
        Fours = scoresArr[3];
        Fives = scoresArr[4];
        Sixes = scoresArr[5];

        Chance = scoresArr[6];

        Three_Of_A_Kind = scoresArr[7];
        Four_Of_A_Kind = scoresArr[8];
        Yahtzze = scoresArr[9];

        Full_House = scoresArr[10];
        Small_Straight = scoresArr[11];
        Large_Straight = scoresArr[12];

        // 점수 업데이트
        ScoreUpdate();
    }

}



public class Table_Mgr : MonoBehaviourPunCallbacks
{
    public Transform TableCameraRoot = null; // 카메라가 테이블로 이동할 위치

    // 테이블 세팅
    public float DistanceFromTable = 2.0f;  // 테이블과 의자의 거리


    // 주사위 설정
    public Transform DiceSpawnPoint = null; // 주사위 스폰 지점
    Dice_Ctrl[] DiceList = new Dice_Ctrl[5];   // 주사위들을 저장할 변수
    List<Vector3> DiceStopPos = new List<Vector3>();    // 주사위가 다 멈춘 후 카메라에 보일 위치
    List<Vector3> DiceHeldPos = new List<Vector3>();    // 주사위를 선택했을때 이동할 위치


    // 방 설정
    public int ChairCount = 1;    // 의자의 개수
    public List<Player_Ctrl> PlayerList = new List<Player_Ctrl>();   // 준비중인 플레이어 수

    // 플레이어가 모두 준비중인지 확인
    bool IsAllReady = false;

    // 게임 설정
    public int MaxRound = 13;      // 총 라운드
    public int CurrentRound = 0;   // 현재 라운드

    public int CurrentPlayerTurn = -1;  // 현재 턴을 진행중인 플레이어의 번호

    [HideInInspector] public int MaxDiceRolls = 3;       // 주사위를 굴릴수 있는 횟수
    public int CurrentDiceRolls = 0;   // 현재 주사위를 굴린 횟수


    PhotonView pv;

    public static Table_Mgr Inst = null;


    private void Awake()
    {
        Inst = this;
        pv = GetComponent<PhotonView>();
    }


    #region ----- 시작 전 -----

    // 처음 테이블 세팅 (방장이 처음 방을 만들고 최대인원수만큼 호출)
    public void SettingTable(int inChairCount)
    {
        ChairCount = inChairCount;

        // 플레이어 리스트 초기화
        PlayerList.Clear();

        // 플레이어의 인원수에 맞춰 의자 세팅
        {
            for (int i = 0; i < ChairCount; i++)
            {
                float angle = i * (360.0f / ChairCount);
                float rad = angle * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(Mathf.Cos(rad), this.transform.position.y, Mathf.Sin(rad)) * DistanceFromTable;
                Vector3 chairPos = this.transform.position + offset;

                Quaternion chairRot = Quaternion.LookRotation(this.transform.position - chairPos);

                GameObject obj = PhotonNetwork.Instantiate("Chair", chairPos, chairRot);
                                                           
                obj.transform.SetParent(this.transform);
            }
        }
        
    }

    // 플레이어 준비 
    public void PlayerReady()
    {
        // 준비중인 플레이어 UI업데이트
        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);

        IsAllReady = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            bool isReady = (p.CustomProperties.ContainsKey("EPlayerState") &&
                (EPlayerState)p.CustomProperties["EPlayerState"] == EPlayerState.Ready);

            if (IsAllReady)
                IsAllReady = isReady;
        }

        // 방장의 게임 시작 버튼 설정
        if (PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.ReadyPanel.PlayGame.SetActive(IsAllReady);
    }

    // 플레이어 인풋시스템에서 상호작용시 호출
    public void TryStartGame()
    {
        // 방장이며 모두 준비상태가 아닌경우 리턴
        if (!PhotonNetwork.IsMasterClient || !IsAllReady || !UICanvas_Mgr.Inst.ReadyPanel.PlayGame.activeSelf) return;

        pv.RPC("RPC_GameStart", RpcTarget.All);
    }

    [PunRPC]
    void RPC_GameStart()
    {
        if(PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.SystemMessages("게임 시작");

        // 카메라 위치 이동
        Camera_Mgr.Inst.ChangeTarget(TableCameraRoot);
        // 카메라 위치 조정
        Camera_Mgr.Inst.SetCameraZoom(0);

        // UI 설정
        UICanvas_Mgr.Inst.SetUIType(EUIType.Game);

        Player_Ctrl[] playersObj = FindObjectsOfType<Player_Ctrl>();

        PlayerList.Clear();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            foreach (Player_Ctrl p in playersObj)
            {
                if (player.NickName == p.PlayerName)
                {
                    // 플레이어 점수 초기화
                    p.PlayerScore = new ScoreCombination();
                    PlayerList.Add(p);
                    break;
                }
            }
        }

        for (int i = 0; i < PlayerList.Count; i++)
        {
            // 플레이어들의 상태를 게임중으로 변경
            PlayerList[i].PlayerState = EPlayerState.Play;

            // 플레이어들에게 점수판 부여
            PlayerList[i].PlayerScore = new ScoreCombination();
        }

        // 주사위 위치값 초기화
        DiceStopPos.Clear();
        DiceHeldPos.Clear();


        // 게임 시작 설정
        CurrentRound = 0;        // 라운드 수
        CurrentPlayerTurn = 100; // 1라운드부터 시작할 수 있도록 높은 값을 설정, NextPlayerTurn에서 0으로 초기화 될 예정
        CurrentDiceRolls = 100;    // 주사위 롤 횟수 초기화 예정
        NextTurn(); // 0번 유저에게 턴 부여하기
    }

    #endregion


    #region ----- 게임 시작 -----

    // 턴 넘기기, 라운드 증가
    [PunRPC]
    void NextTurn()
    {
        // 주사위 롤 횟수 초기화
        CurrentDiceRolls = 0;

        // 플레이어 턴 증가
        CurrentPlayerTurn++;

        // 플레이어 턴이 플레이어보다 많을 경우
        if (CurrentPlayerTurn >= PlayerList.Count)
        {
            // 다시 0번 위치의 플레이어를 시작하게 설정
            CurrentPlayerTurn = 0;

            // 라운드 수 증가
            CurrentRound++;

            // 라운드가 끝났을때 게임 종료
            if (CurrentRound > MaxRound)
            {
                GameEnd();
                return;
            }
        }

        // 플레이어 턴 수정
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerList[i].PlayTurnState = (i == CurrentPlayerTurn) ? EPlayTurnState.MyTurn : EPlayTurnState.NotMyTurn;
        }

        

        // 주사위 굴리기 버튼 활성화
        pv.RPC("RPC_SetDiceRollButton", RpcTarget.AllViaServer);

        // UI 라운드 수 표시 및 설정
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, null, PlayerList[CurrentPlayerTurn].PlayerName, true);

        if (PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.SystemMessages($"{PlayerList[CurrentPlayerTurn].PlayerName}의 턴");
    }

    // 현재 턴인 플레이어의 주사위 굴리기 버튼 활성화 [PunRPC]
    [PunRPC]
    public void RPC_SetDiceRollButton()
    {
        UICanvas_Mgr.Inst.SetDiceRollButton(Game_Mgr.Inst.LocalPlayer.PlayTurnState == EPlayTurnState.MyTurn);
    }

    // 주사위 굴리기 신호 수신(방장에게 신호를 보내 주사위 스폰)
    public void RequestDiceRoll()
    {
        pv.RPC("RPC_DiceRoll", RpcTarget.AllViaServer);
    }

    // 주사위 스폰
    [PunRPC]
    public void RPC_DiceRoll()
    {
        // 주사위 굴린 횟수 증가
        CurrentDiceRolls++;

        // 점수판 UI 버튼 제거
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, null, PlayerList[CurrentPlayerTurn].PlayerName, true);

        // 방장만 주사위 스폰
        if(PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 5; i++)
            {
                if (DiceList[i] != null)
                {
                    if (DiceList[i].m_IsHeld)
                        continue;
                    else
                        PhotonNetwork.Destroy(DiceList[i].gameObject); // 선택이 안된 주사위는 서버에서 삭제
                }

                GameObject obj = PhotonNetwork.Instantiate("Dice", DiceSpawnPoint.position, DiceSpawnPoint.rotation);
                DiceList[i] = null;
                DiceList[i] = obj.GetComponent<Dice_Ctrl>();
                StartCoroutine(DiceList[i].Co_Roll(i));
            }
        }
    }


    // 멈춘 주사위에서 호출할 변수
    // 번호를 저장하여 모든 주사위의 번호를 받았다면 족보로 넘기는 역활
    public void StopDice()
    {
        for (int i = 0; i < 5; i++)
        {
            // 아직 멈춘 주사위가 있다면 함수 종료
            if (DiceList[i].DiceValue == -1) return;
        }

        // 주사위를 카메라 앞으로 이동
        StartCoroutine(Co_MoveDiceToCamera());

        // 족보 체크
        List<int> scoresList = CalculateScores(DiceList);


        // UI를 업데이트 및 현재 턴인 플레이어 점수 적용을 모두에게 호출
        pv.RPC("RPC_UpdateYahtzeeScoreUI", RpcTarget.All, scoresList.ToArray());
    }

    // 족보 체크 후 List<int>를 리턴
    public List<int> CalculateScores(Dice_Ctrl[] InDiceList)
    {
        List<int> returnList = new List<int>();
        ScoreCombination nowScore = new ScoreCombination();
        // 각 주사위의 값
        int[] values = InDiceList.Select(x => x.DiceValue).ToArray();

        // ----상단 항목----
        returnList.Add(values.Where(v => v == 1).Sum());   // Aces
        returnList.Add(values.Where(v => v == 2).Sum());   // Twos
        returnList.Add(values.Where(v => v == 3).Sum());   // Threes
        returnList.Add(values.Where(v => v == 4).Sum());   // Fours
        returnList.Add(values.Where(v => v == 5).Sum());   // Fives 
        returnList.Add(values.Where(v => v == 6).Sum());   // Sixes 

        // -----하단 항목----
        // 현재 주사위의 총 합
        returnList.Add(values.Sum());   //Chance


        // 리스트에서 중복 숫자가 몇 개씩 있는지 센 후 내림차순으로 정렬
        // 같은 숫자들을 모아 분류 -> 분류된 숫자들의 갯수를 정리 -> 카운트가 많은 순서대로 내림차순 -> 리스트화
        List<int> groups = values.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToList();

        // 같은 숫자 3개 이상
        returnList.Add((groups.Any(g => g >= 3)) ? values.Sum() : 0);   // Three_Of_A_Kind

        // 같은 숫자 4개 이상
        returnList.Add((groups.Any(g => g >= 4)) ? values.Sum() : 0);   // Four_Of_A_Kind

        // 같은 숫자 5개
        // 리스트 중에 ()안에 값과 동일한 값이 있다면 true 없다면 false
        returnList.Add((groups.Contains(5)) ? 50 : 0);  // Yahtzze

        // 같은 숫자 3개 + 2개
        returnList.Add((groups.Contains(3) && groups.Contains(2)) ? 25 : 0);    // Full_House


        // 스트레이트 판정
        // 리스트에 중복제거 + 오름차순 정렬 - > 리스트화
        List<int> sorted = values.Distinct().OrderBy(v => v).ToList();
        returnList.Add(HasStraight(sorted, 4) ? 30 : 0);    // Small_Straight
        returnList.Add(HasStraight(sorted, 5) ? 40 : 0);    // Large_Straight

        return returnList;
    }

    // 연속된 숫자가 주어진 길이 이상 있는지 판정
    bool HasStraight(List<int> sortedDistinct, int length)
    {
        int count = 1;

        for (int i = 1; i < sortedDistinct.Count; i++)
        {
            if (sortedDistinct[i] == sortedDistinct[i - 1] + 1)
                count++;
            else
                count = 1;

            // 길이 만족 시 true 반환
            if (count >= length)
                return true;
        }

        return false;
    }

    // UI를 업데이트 및 현재 턴인 플레이어 점수 적용 
    [PunRPC]
    public void RPC_UpdateYahtzeeScoreUI(int[] scoresArr)
    {
        ScoreCombination nowScore = new ScoreCombination();
        nowScore.SetScoreArr(scoresArr);
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, nowScore, PlayerList[CurrentPlayerTurn].PlayerName);


        // 주사위 롤 버튼 활성화 여부
        if(CurrentDiceRolls < MaxDiceRolls)
        UICanvas_Mgr.Inst.SetDiceRollButton(Game_Mgr.Inst.LocalPlayer.PlayTurnState == EPlayTurnState.MyTurn);
    }

    
    

    #endregion



    #region --- 코루틴 함수 ---

    // 주사위가 멈추고 카메라 앞으로 이동하는 코루틴 함수
    [PunRPC]
    IEnumerator Co_MoveDiceToCamera(bool IsStart = true, int Index = -1)
    {
        // 다이스 위치값 설정
        if (DiceStopPos.Count <= 0)
        {
            Camera cam = Camera.main;

            // 카메라로부터 떨어진 깊이 (월드 단위)
            float zWorld = cam.nearClipPlane + 0.2f;

            // 기준 해상도 정의
            float REF_W = 1280f; // 기준 가로 해상도
            float REF_H = 720f;  // 기준 세로 해상도

            // 현재 화면 해상도에 따른 스케일 비율 계산
            // 짧은 변을 기준으로 비율 계산 (가로/세로 비율 유지)
            float scale = Mathf.Min(cam.pixelWidth / REF_W, cam.pixelHeight / REF_H);

            // 중앙 원형 배치 관련 설정
            float radiusPx = 130f * scale; // 원형 반지름(px)
            Vector2 centerScreen = new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f); // 화면 중앙(px)

            // 화면 위쪽 가로 정렬 관련 설정
            float heldSpacingPx = 150f * scale;        // 주사위 간격(px)
            float verticalHeldOffsetPx = 80f * scale; // 화면 상단에서 내려오는 거리(px)
            float heldStartOffsetPx = -((5 - 1) * 0.5f * heldSpacingPx); // 첫 주사위 시작 X 오프셋(px)
            float heldRightOffsetPx = 150f * scale; // 오른쪽으로 px만큼 이동

            // 좌표 계산
            for (int i = 0; i < 5; i++)
            {
                // -------- 중앙 원형 배치 --------
                {
                    float angleRad = (Mathf.PI * 2f / 5f) * i - Mathf.PI / 2f; // 각도 계산

                    float stopX = centerScreen.x + Mathf.Cos(angleRad) * radiusPx;
                    float stopY = centerScreen.y + Mathf.Sin(angleRad) * radiusPx;

                    Vector3 screenPosStop = new Vector3(stopX, stopY, zWorld);
                    DiceStopPos.Add(cam.ScreenToWorldPoint(screenPosStop));
                }

                // -------- 화면 위쪽 가로 정렬 --------
                {
                    float heldX = centerScreen.x + heldStartOffsetPx + i * heldSpacingPx + heldRightOffsetPx;
                    float heldY = cam.pixelHeight - verticalHeldOffsetPx;

                    Vector3 screenPosHeld = new Vector3(heldX, heldY, zWorld);
                    DiceHeldPos.Add(cam.ScreenToWorldPoint(screenPosHeld));
                }
            }

        }

        yield return null;

        float moveTime = IsStart ? 0.4f : 0.1f; // 이동에 걸릴 시간

        for (int i = 0; i < 5; i++)
        {
            if (IsStart == true && DiceList[i].m_IsHeld == true) continue;

            if (Index != -1 && i != Index) continue;



            Vector3 targetPos = Vector3.zero;
            Quaternion targetRot = Quaternion.identity;

            // 위치 설정
            targetPos = (DiceList[i].m_IsHeld) ? DiceHeldPos[i] : DiceStopPos[i];

            // 회전 설정
            {
                Transform face = DiceList[i].DiceNumPos[DiceList[i].DiceValue - 1];

                Vector3 targetForward = -Camera.main.transform.forward.normalized;  // 눈금이 향할 방향 (카메라 정면)
                Vector3 targetUp = Vector3.up;                   // 눈금의 위쪽이 향할 방향 (수직 위)

                // 눈금이 카메라를 보면서 위로도 똑바로 향하게 회전값 생성
                Quaternion Rot = Quaternion.LookRotation(targetForward, targetUp);

                // 현재 눈금이 어디를 향하고 있는지 계산
                Vector3 currentForward = (face.position - DiceList[i].transform.position).normalized;
                Vector3 currentUp = face.up;

                // 현재 회전 기준 (눈금의 방향과 위쪽)
                Quaternion currentRot = Quaternion.LookRotation(currentForward, currentUp);

                // 주사위 전체가 회전해야 하는 보정값 = 타겟 회전 * 현재 회전의 역
                Quaternion finalRot = Rot * Quaternion.Inverse(currentRot);

                // 주사위에 보정 회전을 곱해 최종 회전 계산
                targetRot = finalRot * DiceList[i].transform.rotation;
            }

            StartCoroutine(DiceList[i].Co_MoveDiec(moveTime, targetPos, targetRot));

            
        }
    }

    #endregion



    #region --- 외부에서 호출 ---

    // 마우스로 다이스를 선택했을때 호출
    public void OnMouseClick()
    {
        if (Game_Mgr.Inst.LocalPlayer?.PlayTurnState != EPlayTurnState.MyTurn) return;
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f); // 테스트용 레이저
        if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, LayerMask.GetMask("Dice")))
        {
            Dice_Ctrl dice = hit.collider.gameObject.GetComponent<Dice_Ctrl>();

            if (dice.DiceValue <= 0) return;

            dice.m_IsHeld = !dice.m_IsHeld;
            dice.SetColor();
            pv.RPC("Co_MoveDiceToCamera", RpcTarget.MasterClient, false, dice.DiceID);
        }
    }


    // 족보를 확인 후 UI버튼 클릭시 호출
    public void ClickScoreCardButton(EScoreType ScoreType, int Score)
    {
        UICanvas_Mgr.Inst.SystemMessages($"{ScoreType.ToString()}을 선택", 4.5f, true);

        pv.RPC("RPC_ClickScore", RpcTarget.AllViaServer, (int)ScoreType, Score);
    }


    [PunRPC]
    public void RPC_ClickScore(int ScoreType, int Score)
    {
        // 점수추가
        PlayerList[CurrentPlayerTurn].PlayerScore.SetScore((EScoreType)ScoreType, Score);

        // 방장만 호출 주사위 삭제
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 5; i++)
            {
                if (DiceList[i])
                    PhotonNetwork.Destroy(DiceList[i].gameObject);
            }
        }
    
        // 턴넘김
        NextTurn();
    }


    #endregion



    #region --- 게임 끝 ---

    // NextTurn에서 모든 라운드가 끝나고 호출됨 (모든 플레이어가 호출됨)
    void GameEnd()
    {
        Debug.Log("게임 끝");

        // UI 변경
        UICanvas_Mgr.Inst.SetUIType(EUIType.GameEnd);

        // UI에 현재 참가중인 플레이어들의 최종 점수를 보여줌
        UICanvas_Mgr.Inst.GameEndPenel.SetGameEndPenel();

        Game_Mgr.Inst.LocalPlayer.PlayerState = EPlayerState.NotReady;

        // 최종 판정은 방장만 호출
        if (PhotonNetwork.IsMasterClient)
        {
            Player_Ctrl winnerPlayer = PlayerList[0];
            for(int i = 1; i < PlayerList.Count; i++)
            {
                if(winnerPlayer.PlayerScore.Total < PlayerList[i].PlayerScore.Total)
                {
                    winnerPlayer = PlayerList[i];
                }
            }


            pv.RPC("RPC_WinnerPlayer", RpcTarget.AllBuffered, winnerPlayer.PlayerName);
        }


    }

    [PunRPC]
    public void RPC_WinnerPlayer(string winnerPlayerName)
    {
        UICanvas_Mgr.Inst.GameEndPenel.WinnerPlayerName_Text.text = 
            $"우승자 : {winnerPlayerName}";
    }

    #endregion

}
