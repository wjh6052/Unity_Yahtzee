using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Chair : Interaction_Base
{
    public override void UseInteraction()
    {
        // �÷��̾ ���ڿ� �ɴ� �ڷ�ƾ ����
        StartCoroutine(Game_Mgr.Inst.LocalPlayer.Co_SitDown(this.transform));

        SetIsOnInteraction(false);
    }
    

}
