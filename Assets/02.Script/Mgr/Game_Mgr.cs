using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Game_Mgr : MonoBehaviourPunCallbacks
{

    public List<Player_Ctrl> PlayerList = new List<Player_Ctrl>();

    [HideInInspector] public Player_Ctrl LocalPlayer = null;

    public static Game_Mgr Inst;

    private void Awake()
    {
        Inst = this;
        PlayerList.Clear();
    }


    public void SpawnPlayer()
    {
        GameObject obj = PhotonNetwork.Instantiate(
            "Player",
            Vector3.zero,
            Quaternion.identity
            );

        Player_Ctrl playerObj = obj.GetComponent<Player_Ctrl>();
    }

}
