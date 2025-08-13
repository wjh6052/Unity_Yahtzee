using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmallScoreCard_UI : MonoBehaviour
{
    public Button SmallScoreCard_Button;
    public TextMeshProUGUI PlayerName_Text;
    public TextMeshProUGUI TotalScore_Text;


    Player_Ctrl PlaeyrData;
    ScorePanel_UI Onwer_UI;

    public void SetSmallScoreCard(Player_Ctrl inPlaeyr, ScorePanel_UI onwer_UI)
    {
        PlaeyrData = inPlaeyr;
        Onwer_UI = onwer_UI;


        PlayerName_Text.text = PlaeyrData.PlayerName;
        TotalScore_Text.text = $"Total:{PlaeyrData.PlayerScore.GetScore(EScoreType.Total)}";

        SmallScoreCard_Button.onClick.AddListener(() => { Onwer_UI.OnClickSmallScoreCard(PlaeyrData); });
    }

}
