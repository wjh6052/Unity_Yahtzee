using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_ExitRoom : Interaction_Base
{

    public override void UseInteraction()
    {
        // 방 나가기
        Photon_Mgr.Inst.LeaveRoom(); 
    }
}
