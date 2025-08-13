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
    public int Aces = -1;    // 1�� ���� ��
    public int Twos = -1;    // 2�� ���� ��
    public int Threes = -1;  // 3�� ���� ��
    public int Fours = -1;   // 4�� ���� ��
    public int Fives = -1;   // 5�� ���� ��
    public int Sixes = -1;   // 6�� ���� ��

    public int BonusTotal = 0;   // 1~6�� ��   Bonus�� ���� �� 1~6�� ���ص� �Լ�
    public int Bonus = 0;   // 1~6�� ��   ������ <= 63 -> 35����

    public int Chance = -1;          // ���� �ֻ����� ��
    public int Three_Of_A_Kind = -1; // ���� ���� 3�� �̻��� ��
    public int Four_Of_A_Kind = -1;  // ���� ���� 4�� �̻��� ��
    public int Full_House = -1;      // ���� ���� 3�� + �ٸ� ���� 2���� ��
    public int Small_Straight = -1;  // ���ӵ� ���� 4���� ��     30�� ����
    public int Large_Straight = -1;  // ���ӵ� ���� 5���� ��     40�� ����
    public int Yahtzze = -1;         // 5�� ��� ���� ����       50�� ����

    public int Total = 0;

    // ���ʽ��� ��Ż������ ������Ʈ ��Ű�� �Լ�
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


    // EScoreType�� ������ �ش� ������ �������ִ� ��Ȱ
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

    // EScoreType�� ������ �޾� �ش� ������ �������ִ� ��Ȱ
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

    // int[]�� �޾Ƽ� ������ ����
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

        // ���� ������Ʈ
        ScoreUpdate();
    }

}



public class Table_Mgr : MonoBehaviourPunCallbacks
{
    public Transform TableCameraRoot = null; // ī�޶� ���̺�� �̵��� ��ġ

    // ���̺� ����
    public float DistanceFromTable = 2.0f;  // ���̺�� ������ �Ÿ�


    // �ֻ��� ����
    public Transform DiceSpawnPoint = null; // �ֻ��� ���� ����
    Dice_Ctrl[] DiceList = new Dice_Ctrl[5];   // �ֻ������� ������ ����
    List<Vector3> DiceStopPos = new List<Vector3>();    // �ֻ����� �� ���� �� ī�޶� ���� ��ġ
    List<Vector3> DiceHeldPos = new List<Vector3>();    // �ֻ����� ���������� �̵��� ��ġ


    // �� ����
    public int ChairCount = 1;    // ������ ����
    public List<Player_Ctrl> PlayerList = new List<Player_Ctrl>();   // �غ����� �÷��̾� ��

    // �÷��̾ ��� �غ������� Ȯ��
    bool IsAllReady = false;

    // ���� ����
    public int MaxRound = 13;      // �� ����
    public int CurrentRound = 0;   // ���� ����

    public int CurrentPlayerTurn = -1;  // ���� ���� �������� �÷��̾��� ��ȣ

    [HideInInspector] public int MaxDiceRolls = 3;       // �ֻ����� ������ �ִ� Ƚ��
    public int CurrentDiceRolls = 0;   // ���� �ֻ����� ���� Ƚ��


    PhotonView pv;

    public static Table_Mgr Inst = null;


    private void Awake()
    {
        Inst = this;
        pv = GetComponent<PhotonView>();
    }


    #region ----- ���� �� -----

    // ó�� ���̺� ���� (������ ó�� ���� ����� �ִ��ο�����ŭ ȣ��)
    public void SettingTable(int inChairCount)
    {
        ChairCount = inChairCount;

        // �÷��̾� ����Ʈ �ʱ�ȭ
        PlayerList.Clear();

        // �÷��̾��� �ο����� ���� ���� ����
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

    // �÷��̾� �غ� 
    public void PlayerReady()
    {
        // �غ����� �÷��̾� UI������Ʈ
        UICanvas_Mgr.Inst.ReadyPanel.SetReadyPlaeyrList(PhotonNetwork.PlayerList, PhotonNetwork.CurrentRoom.MaxPlayers);

        IsAllReady = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            bool isReady = (p.CustomProperties.ContainsKey("EPlayerState") &&
                (EPlayerState)p.CustomProperties["EPlayerState"] == EPlayerState.Ready);

            if (IsAllReady)
                IsAllReady = isReady;
        }

        // ������ ���� ���� ��ư ����
        if (PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.ReadyPanel.PlayGame.SetActive(IsAllReady);
    }

    // �÷��̾� ��ǲ�ý��ۿ��� ��ȣ�ۿ�� ȣ��
    public void TryStartGame()
    {
        // �����̸� ��� �غ���°� �ƴѰ�� ����
        if (!PhotonNetwork.IsMasterClient || !IsAllReady || !UICanvas_Mgr.Inst.ReadyPanel.PlayGame.activeSelf) return;

        pv.RPC("RPC_GameStart", RpcTarget.All);
    }

    [PunRPC]
    void RPC_GameStart()
    {
        if(PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.SystemMessages("���� ����");

        // ī�޶� ��ġ �̵�
        Camera_Mgr.Inst.ChangeTarget(TableCameraRoot);
        // ī�޶� ��ġ ����
        Camera_Mgr.Inst.SetCameraZoom(0);

        // UI ����
        UICanvas_Mgr.Inst.SetUIType(EUIType.Game);

        Player_Ctrl[] playersObj = FindObjectsOfType<Player_Ctrl>();

        PlayerList.Clear();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            foreach (Player_Ctrl p in playersObj)
            {
                if (player.NickName == p.PlayerName)
                {
                    // �÷��̾� ���� �ʱ�ȭ
                    p.PlayerScore = new ScoreCombination();
                    PlayerList.Add(p);
                    break;
                }
            }
        }

        for (int i = 0; i < PlayerList.Count; i++)
        {
            // �÷��̾���� ���¸� ���������� ����
            PlayerList[i].PlayerState = EPlayerState.Play;

            // �÷��̾�鿡�� ������ �ο�
            PlayerList[i].PlayerScore = new ScoreCombination();
        }

        // �ֻ��� ��ġ�� �ʱ�ȭ
        DiceStopPos.Clear();
        DiceHeldPos.Clear();


        // ���� ���� ����
        CurrentRound = 0;        // ���� ��
        CurrentPlayerTurn = 100; // 1������� ������ �� �ֵ��� ���� ���� ����, NextPlayerTurn���� 0���� �ʱ�ȭ �� ����
        CurrentDiceRolls = 100;    // �ֻ��� �� Ƚ�� �ʱ�ȭ ����
        NextTurn(); // 0�� �������� �� �ο��ϱ�
    }

    #endregion


    #region ----- ���� ���� -----

    // �� �ѱ��, ���� ����
    [PunRPC]
    void NextTurn()
    {
        // �ֻ��� �� Ƚ�� �ʱ�ȭ
        CurrentDiceRolls = 0;

        // �÷��̾� �� ����
        CurrentPlayerTurn++;

        // �÷��̾� ���� �÷��̾�� ���� ���
        if (CurrentPlayerTurn >= PlayerList.Count)
        {
            // �ٽ� 0�� ��ġ�� �÷��̾ �����ϰ� ����
            CurrentPlayerTurn = 0;

            // ���� �� ����
            CurrentRound++;

            // ���尡 �������� ���� ����
            if (CurrentRound > MaxRound)
            {
                GameEnd();
                return;
            }
        }

        // �÷��̾� �� ����
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerList[i].PlayTurnState = (i == CurrentPlayerTurn) ? EPlayTurnState.MyTurn : EPlayTurnState.NotMyTurn;
        }

        

        // �ֻ��� ������ ��ư Ȱ��ȭ
        pv.RPC("RPC_SetDiceRollButton", RpcTarget.AllViaServer);

        // UI ���� �� ǥ�� �� ����
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, null, PlayerList[CurrentPlayerTurn].PlayerName, true);

        if (PhotonNetwork.IsMasterClient)
            UICanvas_Mgr.Inst.SystemMessages($"{PlayerList[CurrentPlayerTurn].PlayerName}�� ��");
    }

    // ���� ���� �÷��̾��� �ֻ��� ������ ��ư Ȱ��ȭ [PunRPC]
    [PunRPC]
    public void RPC_SetDiceRollButton()
    {
        UICanvas_Mgr.Inst.SetDiceRollButton(Game_Mgr.Inst.LocalPlayer.PlayTurnState == EPlayTurnState.MyTurn);
    }

    // �ֻ��� ������ ��ȣ ����(���忡�� ��ȣ�� ���� �ֻ��� ����)
    public void RequestDiceRoll()
    {
        pv.RPC("RPC_DiceRoll", RpcTarget.AllViaServer);
    }

    // �ֻ��� ����
    [PunRPC]
    public void RPC_DiceRoll()
    {
        // �ֻ��� ���� Ƚ�� ����
        CurrentDiceRolls++;

        // ������ UI ��ư ����
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, null, PlayerList[CurrentPlayerTurn].PlayerName, true);

        // ���常 �ֻ��� ����
        if(PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 5; i++)
            {
                if (DiceList[i] != null)
                {
                    if (DiceList[i].m_IsHeld)
                        continue;
                    else
                        PhotonNetwork.Destroy(DiceList[i].gameObject); // ������ �ȵ� �ֻ����� �������� ����
                }

                GameObject obj = PhotonNetwork.Instantiate("Dice", DiceSpawnPoint.position, DiceSpawnPoint.rotation);
                DiceList[i] = null;
                DiceList[i] = obj.GetComponent<Dice_Ctrl>();
                StartCoroutine(DiceList[i].Co_Roll(i));
            }
        }
    }


    // ���� �ֻ������� ȣ���� ����
    // ��ȣ�� �����Ͽ� ��� �ֻ����� ��ȣ�� �޾Ҵٸ� ������ �ѱ�� ��Ȱ
    public void StopDice()
    {
        for (int i = 0; i < 5; i++)
        {
            // ���� ���� �ֻ����� �ִٸ� �Լ� ����
            if (DiceList[i].DiceValue == -1) return;
        }

        // �ֻ����� ī�޶� ������ �̵�
        StartCoroutine(Co_MoveDiceToCamera());

        // ���� üũ
        List<int> scoresList = CalculateScores(DiceList);


        // UI�� ������Ʈ �� ���� ���� �÷��̾� ���� ������ ��ο��� ȣ��
        pv.RPC("RPC_UpdateYahtzeeScoreUI", RpcTarget.All, scoresList.ToArray());
    }

    // ���� üũ �� List<int>�� ����
    public List<int> CalculateScores(Dice_Ctrl[] InDiceList)
    {
        List<int> returnList = new List<int>();
        ScoreCombination nowScore = new ScoreCombination();
        // �� �ֻ����� ��
        int[] values = InDiceList.Select(x => x.DiceValue).ToArray();

        // ----��� �׸�----
        returnList.Add(values.Where(v => v == 1).Sum());   // Aces
        returnList.Add(values.Where(v => v == 2).Sum());   // Twos
        returnList.Add(values.Where(v => v == 3).Sum());   // Threes
        returnList.Add(values.Where(v => v == 4).Sum());   // Fours
        returnList.Add(values.Where(v => v == 5).Sum());   // Fives 
        returnList.Add(values.Where(v => v == 6).Sum());   // Sixes 

        // -----�ϴ� �׸�----
        // ���� �ֻ����� �� ��
        returnList.Add(values.Sum());   //Chance


        // ����Ʈ���� �ߺ� ���ڰ� �� ���� �ִ��� �� �� ������������ ����
        // ���� ���ڵ��� ��� �з� -> �з��� ���ڵ��� ������ ���� -> ī��Ʈ�� ���� ������� �������� -> ����Ʈȭ
        List<int> groups = values.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToList();

        // ���� ���� 3�� �̻�
        returnList.Add((groups.Any(g => g >= 3)) ? values.Sum() : 0);   // Three_Of_A_Kind

        // ���� ���� 4�� �̻�
        returnList.Add((groups.Any(g => g >= 4)) ? values.Sum() : 0);   // Four_Of_A_Kind

        // ���� ���� 5��
        // ����Ʈ �߿� ()�ȿ� ���� ������ ���� �ִٸ� true ���ٸ� false
        returnList.Add((groups.Contains(5)) ? 50 : 0);  // Yahtzze

        // ���� ���� 3�� + 2��
        returnList.Add((groups.Contains(3) && groups.Contains(2)) ? 25 : 0);    // Full_House


        // ��Ʈ����Ʈ ����
        // ����Ʈ�� �ߺ����� + �������� ���� - > ����Ʈȭ
        List<int> sorted = values.Distinct().OrderBy(v => v).ToList();
        returnList.Add(HasStraight(sorted, 4) ? 30 : 0);    // Small_Straight
        returnList.Add(HasStraight(sorted, 5) ? 40 : 0);    // Large_Straight

        return returnList;
    }

    // ���ӵ� ���ڰ� �־��� ���� �̻� �ִ��� ����
    bool HasStraight(List<int> sortedDistinct, int length)
    {
        int count = 1;

        for (int i = 1; i < sortedDistinct.Count; i++)
        {
            if (sortedDistinct[i] == sortedDistinct[i - 1] + 1)
                count++;
            else
                count = 1;

            // ���� ���� �� true ��ȯ
            if (count >= length)
                return true;
        }

        return false;
    }

    // UI�� ������Ʈ �� ���� ���� �÷��̾� ���� ���� 
    [PunRPC]
    public void RPC_UpdateYahtzeeScoreUI(int[] scoresArr)
    {
        ScoreCombination nowScore = new ScoreCombination();
        nowScore.SetScoreArr(scoresArr);
        UICanvas_Mgr.Inst.ScoreCard.SetScoreCard(PlayerList[CurrentPlayerTurn].PlayerScore, nowScore, PlayerList[CurrentPlayerTurn].PlayerName);


        // �ֻ��� �� ��ư Ȱ��ȭ ����
        if(CurrentDiceRolls < MaxDiceRolls)
        UICanvas_Mgr.Inst.SetDiceRollButton(Game_Mgr.Inst.LocalPlayer.PlayTurnState == EPlayTurnState.MyTurn);
    }

    
    

    #endregion



    #region --- �ڷ�ƾ �Լ� ---

    // �ֻ����� ���߰� ī�޶� ������ �̵��ϴ� �ڷ�ƾ �Լ�
    [PunRPC]
    IEnumerator Co_MoveDiceToCamera(bool IsStart = true, int Index = -1)
    {
        // ���̽� ��ġ�� ����
        if (DiceStopPos.Count <= 0)
        {
            Camera cam = Camera.main;

            // ī�޶�κ��� ������ ���� (���� ����)
            float zWorld = cam.nearClipPlane + 0.2f;

            // ���� �ػ� ����
            float REF_W = 1280f; // ���� ���� �ػ�
            float REF_H = 720f;  // ���� ���� �ػ�

            // ���� ȭ�� �ػ󵵿� ���� ������ ���� ���
            // ª�� ���� �������� ���� ��� (����/���� ���� ����)
            float scale = Mathf.Min(cam.pixelWidth / REF_W, cam.pixelHeight / REF_H);

            // �߾� ���� ��ġ ���� ����
            float radiusPx = 130f * scale; // ���� ������(px)
            Vector2 centerScreen = new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f); // ȭ�� �߾�(px)

            // ȭ�� ���� ���� ���� ���� ����
            float heldSpacingPx = 150f * scale;        // �ֻ��� ����(px)
            float verticalHeldOffsetPx = 80f * scale; // ȭ�� ��ܿ��� �������� �Ÿ�(px)
            float heldStartOffsetPx = -((5 - 1) * 0.5f * heldSpacingPx); // ù �ֻ��� ���� X ������(px)
            float heldRightOffsetPx = 150f * scale; // ���������� px��ŭ �̵�

            // ��ǥ ���
            for (int i = 0; i < 5; i++)
            {
                // -------- �߾� ���� ��ġ --------
                {
                    float angleRad = (Mathf.PI * 2f / 5f) * i - Mathf.PI / 2f; // ���� ���

                    float stopX = centerScreen.x + Mathf.Cos(angleRad) * radiusPx;
                    float stopY = centerScreen.y + Mathf.Sin(angleRad) * radiusPx;

                    Vector3 screenPosStop = new Vector3(stopX, stopY, zWorld);
                    DiceStopPos.Add(cam.ScreenToWorldPoint(screenPosStop));
                }

                // -------- ȭ�� ���� ���� ���� --------
                {
                    float heldX = centerScreen.x + heldStartOffsetPx + i * heldSpacingPx + heldRightOffsetPx;
                    float heldY = cam.pixelHeight - verticalHeldOffsetPx;

                    Vector3 screenPosHeld = new Vector3(heldX, heldY, zWorld);
                    DiceHeldPos.Add(cam.ScreenToWorldPoint(screenPosHeld));
                }
            }

        }

        yield return null;

        float moveTime = IsStart ? 0.4f : 0.1f; // �̵��� �ɸ� �ð�

        for (int i = 0; i < 5; i++)
        {
            if (IsStart == true && DiceList[i].m_IsHeld == true) continue;

            if (Index != -1 && i != Index) continue;



            Vector3 targetPos = Vector3.zero;
            Quaternion targetRot = Quaternion.identity;

            // ��ġ ����
            targetPos = (DiceList[i].m_IsHeld) ? DiceHeldPos[i] : DiceStopPos[i];

            // ȸ�� ����
            {
                Transform face = DiceList[i].DiceNumPos[DiceList[i].DiceValue - 1];

                Vector3 targetForward = -Camera.main.transform.forward.normalized;  // ������ ���� ���� (ī�޶� ����)
                Vector3 targetUp = Vector3.up;                   // ������ ������ ���� ���� (���� ��)

                // ������ ī�޶� ���鼭 ���ε� �ȹٷ� ���ϰ� ȸ���� ����
                Quaternion Rot = Quaternion.LookRotation(targetForward, targetUp);

                // ���� ������ ��� ���ϰ� �ִ��� ���
                Vector3 currentForward = (face.position - DiceList[i].transform.position).normalized;
                Vector3 currentUp = face.up;

                // ���� ȸ�� ���� (������ ����� ����)
                Quaternion currentRot = Quaternion.LookRotation(currentForward, currentUp);

                // �ֻ��� ��ü�� ȸ���ؾ� �ϴ� ������ = Ÿ�� ȸ�� * ���� ȸ���� ��
                Quaternion finalRot = Rot * Quaternion.Inverse(currentRot);

                // �ֻ����� ���� ȸ���� ���� ���� ȸ�� ���
                targetRot = finalRot * DiceList[i].transform.rotation;
            }

            StartCoroutine(DiceList[i].Co_MoveDiec(moveTime, targetPos, targetRot));

            
        }
    }

    #endregion



    #region --- �ܺο��� ȣ�� ---

    // ���콺�� ���̽��� ���������� ȣ��
    public void OnMouseClick()
    {
        if (Game_Mgr.Inst.LocalPlayer?.PlayTurnState != EPlayTurnState.MyTurn) return;
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f); // �׽�Ʈ�� ������
        if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, LayerMask.GetMask("Dice")))
        {
            Dice_Ctrl dice = hit.collider.gameObject.GetComponent<Dice_Ctrl>();

            if (dice.DiceValue <= 0) return;

            dice.m_IsHeld = !dice.m_IsHeld;
            dice.SetColor();
            pv.RPC("Co_MoveDiceToCamera", RpcTarget.MasterClient, false, dice.DiceID);
        }
    }


    // ������ Ȯ�� �� UI��ư Ŭ���� ȣ��
    public void ClickScoreCardButton(EScoreType ScoreType, int Score)
    {
        UICanvas_Mgr.Inst.SystemMessages($"{ScoreType.ToString()}�� ����", 4.5f, true);

        pv.RPC("RPC_ClickScore", RpcTarget.AllViaServer, (int)ScoreType, Score);
    }


    [PunRPC]
    public void RPC_ClickScore(int ScoreType, int Score)
    {
        // �����߰�
        PlayerList[CurrentPlayerTurn].PlayerScore.SetScore((EScoreType)ScoreType, Score);

        // ���常 ȣ�� �ֻ��� ����
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 5; i++)
            {
                if (DiceList[i])
                    PhotonNetwork.Destroy(DiceList[i].gameObject);
            }
        }
    
        // �ϳѱ�
        NextTurn();
    }


    #endregion



    #region --- ���� �� ---

    // NextTurn���� ��� ���尡 ������ ȣ��� (��� �÷��̾ ȣ���)
    void GameEnd()
    {
        Debug.Log("���� ��");

        // UI ����
        UICanvas_Mgr.Inst.SetUIType(EUIType.GameEnd);

        // UI�� ���� �������� �÷��̾���� ���� ������ ������
        UICanvas_Mgr.Inst.GameEndPenel.SetGameEndPenel();

        Game_Mgr.Inst.LocalPlayer.PlayerState = EPlayerState.NotReady;

        // ���� ������ ���常 ȣ��
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
            $"����� : {winnerPlayerName}";
    }

    #endregion

}
