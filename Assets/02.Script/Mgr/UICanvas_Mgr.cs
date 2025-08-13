using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EUIType // 현재 UI상태
{
    Loading,
    Play,
    Ready,
    Game,
    GameEnd
}


public class UICanvas_Mgr : MonoBehaviourPunCallbacks
{
    public Transform SystemMessagesBox;
    SystemMessages SystemMessages_Prefab = null;


    [Header("---Panels---")]
    public LoadingPanel_UI LoadingPanel;
    public GameObject PlayPanel;
    public ReadyPanel_UI ReadyPanel;
    public GameObject GamePanel;
    public GameEndPenel_UI GameEndPenel;
    public ScorePanel_UI ScorePanel;
    public ChatPanel_UI ChatPanel;



    [Header("---Game---")]
    public Interaction_UI Interaction_UI;

    public Button DiceRoll_Button;
    public TextMeshProUGUI DiceRollTurn_Text;
    


    [Header("---Play---")]
    public ScoreCard_UI ScoreCard;


    public EUIType UIType = EUIType.Play;


    PhotonView pv;

    public static UICanvas_Mgr Inst = null;

    private void Awake()
    {
        Inst = this;
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {

        // 초기 설정
        {
            // 처음에 패널 타입을 Play로 변경
            UIType = EUIType.Loading;
            SetUIType(UIType);

            // 상호작용UI 끄기 
            SetInteractionUI(false);

            // 점수판UI 끄기
            ScoreCard.gameObject.SetActive(false);


            // 주사위 굴리기 버튼 설정 및 끄기
            DiceRoll_Button.onClick.AddListener(DiceRollButtonOnClick);
            SetDiceRollButton(false);

            ChatPanel.gameObject.SetActive(false);
        }
        
    }

    public void SetUIType(EUIType InUIType)
    {
        UIType = InUIType;

        if(UIType == EUIType.Play || UIType == EUIType.Ready)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        // Loading 패널
        LoadingPanel.gameObject.SetActive(UIType == EUIType.Loading);

        // Play 패널
        PlayPanel.gameObject.SetActive(UIType == EUIType.Play);

        // Ready 패널
        ReadyPanel.gameObject.SetActive(UIType == EUIType.Ready);

        // Game 패널
        GamePanel.gameObject.SetActive(UIType == EUIType.Game);
        ScoreCard.gameObject.SetActive(UIType == EUIType.Game);

        // GameEnd 패널
        GameEndPenel.gameObject.SetActive(UIType == EUIType.GameEnd);

        // ScorePanel 패널
        ScorePanel.SetScorePanel(UIType == EUIType.GameEnd || UIType == EUIType.Game);

        // ChatPanel
        ChatPanel.gameObject.SetActive(UIType != EUIType.Loading);

    }

    public void SystemMessages(string inMessages, float inRemovalTime = 3.5f, bool isNotMaster = false)
    {
        if (!PhotonNetwork.InRoom)
        {
            // 아직 방이 아님 → 로컬로만 표시하거나 큐에 저장
            RPC_SystemMessages(inMessages, inRemovalTime);
            return;
        }

        if(isNotMaster == false)
        {
            if (PhotonNetwork.IsMasterClient)
                pv.RPC("RPC_SystemMessages", RpcTarget.AllBuffered, inMessages, inRemovalTime);
        }
        else
            RPC_SystemMessages(inMessages, inRemovalTime);

    }

    [PunRPC]
    public void RPC_SystemMessages(string inMessages, float inRemovalTime)
    {
        if (SystemMessages_Prefab == null)
            SystemMessages_Prefab = Resources.Load("UI/SystemMessages_Prefab").GetComponent<SystemMessages>();

        SystemMessages obj = Instantiate(SystemMessages_Prefab.gameObject, SystemMessagesBox).GetComponent<SystemMessages>();

        obj.SetSystemMessages(inMessages, inRemovalTime);
    }

    public void SetInteractionUI(bool IsOn, Interaction_Base input = null)
    {
        if (IsOn)
            Interaction_UI.Name_Text.text = input.InteractionName;

        Interaction_UI.gameObject.SetActive(IsOn);
    }

    public void RemoveInteractionUI()
    {

    }


    // 주사위 굴리기 버튼 클릭시 호출
    void DiceRollButtonOnClick()
    {
        // 방장에게 주사위 굴리기 호출
        Table_Mgr.Inst.RequestDiceRoll();
        SetDiceRollButton(false);
    }

    public void SetDiceRollButton(bool IsOn)
    {
        DiceRoll_Button.gameObject.SetActive(IsOn);

        if(IsOn)
        {
            DiceRollTurn_Text.text = $"{Table_Mgr.Inst.CurrentDiceRolls} / {Table_Mgr.Inst.MaxDiceRolls}";
        }
    }


}
