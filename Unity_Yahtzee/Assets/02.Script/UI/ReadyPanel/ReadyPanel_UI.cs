using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReadyPanel_UI : MonoBehaviour
{
    public Transform ReadyPlaeyrListBox;
    public GameObject PlayGame;

    public GameObject CancelReady;

    void Start()
    {
        // 초기 ReadPlaeyrList삭제
        foreach (Transform child in ReadyPlaeyrListBox)
            Destroy(child.gameObject);

        PlayGame.SetActive(false);
        CancelReady.SetActive(true);
    }

    public void SetReadyPlaeyrList(Player[] inPlayerList, int inMaxPalyer)
    {
        // 기존의 ReadPlaeyrList삭제
        foreach (Transform child in ReadyPlaeyrListBox)
            Destroy(child.gameObject);

        // 프리팹 가져오기
        ReadyPlaeyrList_UI readyObj = Resources.Load("UI/ReadyPlaeyrListUI_Prefab").GetComponent<ReadyPlaeyrList_UI>();

        
        for(int i = 0; i < inMaxPalyer; i++)
        {
            ReadyPlaeyrList_UI obj = Instantiate(readyObj, ReadyPlaeyrListBox, false);

            if (i <= inPlayerList.Length - 1)
            {
                bool isReady = (inPlayerList[i].CustomProperties.ContainsKey("EPlayerState") &&
                (EPlayerState)inPlayerList[i].CustomProperties["EPlayerState"] == EPlayerState.Ready);

                obj.SetReadyPlaeyrList(inPlayerList[i].NickName, isReady, inPlayerList[i].IsMasterClient);
            }
            else
            {
                obj.SetReadyPlaeyrList("", false, false);
            }
        }
        //readyObj.SetReadyPlaeyrList();


    }

}
