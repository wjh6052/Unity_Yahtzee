using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Chair : Interaction_Base
{
    public override void UseInteraction()
    {
        // 플레이어가 의자에 앉는 코루틴 실행
        StartCoroutine(Game_Mgr.Inst.LocalPlayer.Co_SitDown(this.transform));

        SetIsOnInteraction(false);
    }
    

}
