using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EUIType // ���� UI����
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

        // �ʱ� ����
        {
            // ó���� �г� Ÿ���� Play�� ����
            UIType = EUIType.Loading;
            SetUIType(UIType);

            // ��ȣ�ۿ�UI ���� 
            SetInteractionUI(false);

            // ������UI ����
            ScoreCard.gameObject.SetActive(false);


            // �ֻ��� ������ ��ư ���� �� ����
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

        // Loading �г�
        LoadingPanel.gameObject.SetActive(UIType == EUIType.Loading);

        // Play �г�
        PlayPanel.gameObject.SetActive(UIType == EUIType.Play);

        // Ready �г�
        ReadyPanel.gameObject.SetActive(UIType == EUIType.Ready);

        // Game �г�
        GamePanel.gameObject.SetActive(UIType == EUIType.Game);
        ScoreCard.gameObject.SetActive(UIType == EUIType.Game);

        // GameEnd �г�
        GameEndPenel.gameObject.SetActive(UIType == EUIType.GameEnd);

        // ScorePanel �г�
        ScorePanel.SetScorePanel(UIType == EUIType.GameEnd || UIType == EUIType.Game);

        // ChatPanel
        ChatPanel.gameObject.SetActive(UIType != EUIType.Loading);

    }

    public void SystemMessages(string inMessages, float inRemovalTime = 3.5f, bool isNotMaster = false)
    {
        if (!PhotonNetwork.InRoom)
        {
            // ���� ���� �ƴ� �� ���÷θ� ǥ���ϰų� ť�� ����
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


    // �ֻ��� ������ ��ư Ŭ���� ȣ��
    void DiceRollButtonOnClick()
    {
        // ���忡�� �ֻ��� ������ ȣ��
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
