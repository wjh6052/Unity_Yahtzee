using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPenel_UI : MonoBehaviour
{
    

    public TextMeshProUGUI WinnerPlayerName_Text;

    public Button GameEnd_Button;

    public void SetGameEndPenel()
    {
        // 확인버튼을 눌렀을때
        GameEnd_Button.onClick.AddListener(() =>
        {
            Game_Mgr.Inst.LocalPlayer.PlayerState = EPlayerState.Ready;

            // ui변경
            UICanvas_Mgr.Inst.SetUIType(EUIType.Ready);

            // 카메라 위치 이동
            Camera_Mgr.Inst.ChangeTarget(Game_Mgr.Inst.LocalPlayer.CameraTargetRoot.transform);
            // 카메라 위치 조정
            Camera_Mgr.Inst.SetCameraZoom(2.5f);

        });
    }

}
