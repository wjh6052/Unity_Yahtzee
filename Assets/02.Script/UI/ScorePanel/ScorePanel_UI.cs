using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ScorePanel_UI : MonoBehaviour
{
    public Button CheckScore_Button;
    public TextMeshProUGUI CheckScoreButton_Text;


    bool IsOnPanel = false;

    [Header("--- ScoreListPanel ---")]
    public GameObject ScoreListPanel;
    public Transform SmallScoreCardBox;
    public ScoreCard_UI ScoreCard;

    [Header("--- Prefab ---")]
    public SmallScoreCard_UI SmallScoreCard_Prefab;


    private void Start()
    {
        CheckScore_Button.onClick.AddListener(()=> { OnScoreListPanel(!IsOnPanel); });
    }


    public void SetScorePanel(bool isOn)
    {
        this.gameObject.SetActive(isOn);
        OnScoreListPanel(false);

        if (isOn == false) return;

        IsOnPanel = false;
        ScoreListPanel.SetActive(false);
    }

    void OnScoreListPanel(bool isOnPanel)
    {
        IsOnPanel = isOnPanel;


        ScoreListPanel.SetActive(IsOnPanel);

        CheckScoreButton_Text.text = (IsOnPanel) ? "닫기" : "점수확인";

        if (!IsOnPanel) return;

        // 종합 점수판 끄기
        ScoreCard.gameObject.SetActive(false);


        // 기존의 플레이어 정보 제거
        foreach (Transform child in SmallScoreCardBox)
            Destroy(child.gameObject);

        //  플레이어 정보 추가
        foreach (Player_Ctrl player in Table_Mgr.Inst.PlayerList)
        {
            SmallScoreCard_UI obj = Instantiate(SmallScoreCard_Prefab.gameObject, SmallScoreCardBox).GetComponent<SmallScoreCard_UI>();
            obj.SetSmallScoreCard(player, this);
        }

    }

    public void OnClickSmallScoreCard(Player_Ctrl inPlayerData)
    {
        // 종합 점수판 켜기
        ScoreCard.gameObject.SetActive(true);


        ScoreCard.SetScoreCard(inPlayerData.PlayerScore, null, inPlayerData.PlayerName, true);
    }

}
