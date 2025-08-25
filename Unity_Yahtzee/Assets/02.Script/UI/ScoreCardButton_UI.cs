using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;



public class ScoreCardButton_UI : MonoBehaviour
{
    public EScoreType ScoreType = EScoreType.Aces;


    public Image ScoreType_Image;
    public Sprite[] T_ScoreType = new Sprite[13];


    public TextMeshProUGUI ScoreType_Text;
    public TextMeshProUGUI Score_Text;

    public Button ScoreCardButton;
    public TextMeshProUGUI NowScore_Text;

    public float EnableDelayTime = 0.25f;


    int Score;
    int AddScore;

    public void SetScoreCardButton(EScoreType InScoreType, int InScore, bool IsOn)
    {
        ScoreType = InScoreType;
        Score = InScore;

        // 이미지 적용
        if (ScoreType == EScoreType.Bonus || ScoreType == EScoreType.BonusTotal || ScoreType == EScoreType.Total)
            ScoreType_Image.color = new Color(0, 0, 0, 0);
        else
            ScoreType_Image.sprite = T_ScoreType[((int)ScoreType < 7) ? (int)ScoreType : (int)ScoreType - 2];


        // 점수 종류 이름
        string scoreType = "";
        switch (ScoreType)
        {
            case EScoreType.Three_Of_A_Kind:
                scoreType = "3 of a Kind";
                break;

            case EScoreType.Four_Of_A_Kind:
                scoreType = "4 of a Kind";
                break;

            case EScoreType.Full_House:
                scoreType = "Full House";
                break;

            case EScoreType.Small_Straight:
                scoreType = "S Straight";
                break;

            case EScoreType.Large_Straight:
                scoreType = "L Straight";
                break;

            case EScoreType.BonusTotal:
                scoreType = "Bonus";
                break;

            default:
                scoreType = ScoreType.ToString();
                break;
        }
        ScoreType_Text.text = scoreType;


        // 현재 점수 입력
        if (IsOn || InScore == -1)
            Score_Text.text = "-";
        else if (ScoreType == EScoreType.BonusTotal)
            Score_Text.text =$"{InScore}/63";
        else
            Score_Text.text = InScore.ToString();


        // 버튼 설정
        NowScore_Text.gameObject.SetActive(false);
        if (IsOn && ScoreType != EScoreType.Bonus && ScoreType != EScoreType.Total)
        {
            AddScore = InScore;
            StartCoroutine(Co_Button());
        }
        else
            ScoreCardButton.image.color = new Color(0, 0, 0, 0);
    }


    IEnumerator Co_Button()
    {
        ScoreCardButton.image.fillAmount = 0.0f;
        float time = 0.0f;

        while (time <= EnableDelayTime)
        {
            time += Time.deltaTime;
            ScoreCardButton.image.fillAmount = Mathf.Clamp01(time / EnableDelayTime);

            yield return null;
        }

        NowScore_Text.gameObject.SetActive(true);
        NowScore_Text.text = AddScore.ToString();

        // 버튼 클릭 후
        ScoreCardButton.onClick.AddListener(() => 
        {
            if (Game_Mgr.Inst.LocalPlayer?.PlayTurnState != EPlayTurnState.MyTurn) return;

            NowScore_Text.gameObject.SetActive(false);
            Table_Mgr.Inst.ClickScoreCardButton(ScoreType, AddScore);
        });
    }
}
