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

        // ������ �ִ� ������ ��ư�� ����
        foreach (Transform child in ScoreCardButton_Box)
            Destroy(child.gameObject);
            

        for (int i = 0; i < (int)EScoreType.Count; i++)
        {
            if ((EScoreType)i == EScoreType.Bonus) continue;

            ScoreCardButton_UI scoreCardButton = Instantiate(ScoreCardButton_Prefab, ScoreCardButton_Box);

            if(IsScoreCheckOnly == false)
            {
                // �÷��̾� ������ �ִ��� Ȯ��, ���ٸ� ��ư Ȱ��ȭ
                bool isScore = InPlayerScore.GetScore((EScoreType)i) == -1;

                // �÷��̾��� ������ ���ٸ� ���� ������ ���� ����, �ִٸ� �÷��̾��� ������ ����
                int score = isScore ? InNowScore.GetScore((EScoreType)i) : InPlayerScore.GetScore((EScoreType)i);

                scoreCardButton.SetScoreCardButton((EScoreType)i, score, isScore);
            }
            else // �ܼ� ������ ������ �����ֱ� ���� ���
            {
                int score = InPlayerScore.GetScore((EScoreType)i);

                scoreCardButton.SetScoreCardButton((EScoreType)i, score, false);
            }
            
        }
    }


}
