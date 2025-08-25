using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interaction_Base : MonoBehaviourPunCallbacks
{
    public string InteractionName = "";

    public bool IsOnInteraction = true;

    PhotonView pv;

    public virtual void UseInteraction()
    {
        Debug.Log(this.gameObject.name);
    }


    public void SetIsOnInteraction(bool isOn)
    {
        pv = GetComponent<PhotonView>();

        pv.RPC("RPC_SetIsOnInteraction", RpcTarget.AllBuffered, isOn);
    }

    [PunRPC]
    public void RPC_SetIsOnInteraction(bool isOn)
    {
        IsOnInteraction = isOn;
    }
}
