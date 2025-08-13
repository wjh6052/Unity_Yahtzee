using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class M_Interaction : MonoBehaviour
{
    // �������� ��ȣ�ۿ� ������Ʈ�� ������ ����
    [HideInInspector] public Interaction_Base NowInteractionUI = null;



    // ��� ȣ��
    public void UseInteraction()
    {
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

    // �߰�
    public void StartInteraction(Interaction_Base input)
    {
        NowInteractionUI = input;
        UICanvas_Mgr.Inst.SetInteractionUI(true, NowInteractionUI);
    }

    // ����
    public void EndInteraction()
    {
        NowInteractionUI = null;
        UICanvas_Mgr.Inst.SetInteractionUI(false);
    }



    // �浹 ����
    void OnTriggerEnter(Collider coll)
    {
        Interaction_Base interaction = coll.GetComponent<Interaction_Base>();
        if (interaction)
        {
            if (interaction.IsOnInteraction == false) return;

            if(Game_Mgr.Inst.LocalPlayer.pv.IsMine)
                StartInteraction(interaction);
        }
    }

    // �浹 ��
    private void OnTriggerExit(Collider coll)
    {
        Interaction_Base interaction = coll.GetComponent<Interaction_Base>();
        if (interaction)
        {
            EndInteraction();
        }
    }

    
}
