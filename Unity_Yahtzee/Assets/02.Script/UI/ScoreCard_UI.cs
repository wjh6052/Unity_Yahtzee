using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreCard_UI : MonoBehaviour
{
    //[Header("---Text---")]
    public ScoreCardButton_UI ScoreCardButton_Prefab;

    public Transform ScoreCardButton_Box;

    public TextMeshProUGUI Turn_Text;

    public void SetScoreCard(ScoreCombination InPlayerScore, ScoreCombination InNowScore, string turnPlayerName, bool IsScoreCheckOnly = false)
    {
        Turn_Text.text = turnPlayerName;

        // 기존에 있던 점수판 버튼들 제거
        foreach (Transform child in ScoreCardButton_Box)
            Destroy(child.gameObject);
            

        for (int i = 0; i < (int)EScoreType.Count; i++)
        {
            if ((EScoreType)i == EScoreType.Bonus) continue;

            ScoreCardButton_UI scoreCardButton = Instantiate(ScoreCardButton_Prefab, ScoreCardButton_Box);

            if(IsScoreCheckOnly == false)
            {
                // 플레이어 점수가 있는지 확인, 없다면 버튼 활성화
                bool isScore = InPlayerScore.GetScore((EScoreType)i) == -1;

                // 플레이어의 점수가 없다면 지금 족보의 점수 적용, 있다면 플레이어의 점수를 설정
                int score = isScore ? InNowScore.GetScore((EScoreType)i) : InPlayerScore.GetScore((EScoreType)i);

                scoreCardButton.SetScoreCardButton((EScoreType)i, score, isScore);
            }
            else // 단순 현재의 점수만 보여주기 위해 사용
            {
                int score = InPlayerScore.GetScore((EScoreType)i);

                scoreCardButton.SetScoreCardButton((EScoreType)i, score, false);
            }
            
        }
    }


}
