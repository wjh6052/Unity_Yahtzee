using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class M_Interaction : MonoBehaviour
{
    // 접촉중인 상호작용 오브젝트를 저장할 변수
    [HideInInspector] public Interaction_Base NowInteractionUI = null;

    public Player_Ctrl OwnerPlayer;


    // 사용 호출
    public void UseInteraction()
    {
        if (!OwnerPlayer.pv.IsMine) return;
        if (NowInteractionUI == null) return;

        if(NowInteractionUI.IsOnInteraction == false)
        {
            EndInteraction();
            return;
        }

        if (NowInteractionUI != null)
            NowInteractionUI.UseInteraction();

        EndInteraction();
    }

    // 추가
    public void StartInteraction(Interaction_Base input)
    {
        if(OwnerPlayer.pv.IsMine)
        {
            NowInteractionUI = input;
            UICanvas_Mgr.Inst.SetInteractionUI(true, NowInteractionUI);
        }
       
    }

    // 제거
    public void EndInteraction()
    {
        if (OwnerPlayer.pv.IsMine)
        {
            NowInteractionUI = null;
            UICanvas_Mgr.Inst.SetInteractionUI(false);
        }
    }



    // 충돌 시작
    void OnTriggerEnter(Collider coll)
    {
        if (!OwnerPlayer.pv.IsMine) return;

            Interaction_Base interaction = coll.GetComponent<Interaction_Base>();

        if (interaction)
        {
            if (interaction.IsOnInteraction == false) return;

            if(OwnerPlayer.pv.IsMine)
                StartInteraction(interaction);
        }
    }

    // 충돌 끝
    private void OnTriggerExit(Collider coll)
    {
        if (!OwnerPlayer.pv.IsMine) return;

        Interaction_Base interaction = coll.GetComponent<Interaction_Base>();
        if (interaction)
        {
            EndInteraction();
        }
    }

    
}
