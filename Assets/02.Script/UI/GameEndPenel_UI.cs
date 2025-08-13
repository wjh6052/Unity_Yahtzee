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
        // Ȯ�ι�ư�� ��������
        GameEnd_Button.onClick.AddListener(() =>
        {
            Game_Mgr.Inst.LocalPlayer.PlayerState = EPlayerState.Ready;

            // ui����
            UICanvas_Mgr.Inst.SetUIType(EUIType.Ready);

            // ī�޶� ��ġ �̵�
            Camera_Mgr.Inst.ChangeTarget(Game_Mgr.Inst.LocalPlayer.CameraTargetRoot.transform);
            // ī�޶� ��ġ ����
            Camera_Mgr.Inst.SetCameraZoom(2.5f);

        });
    }

}
