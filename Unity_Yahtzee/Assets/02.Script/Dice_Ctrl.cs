using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Dice_Ctrl : MonoBehaviour
{
    public Transform[] DiceNumPos = new Transform[6];  // 주사위 숫자 판단
    Rigidbody body;


    // 주사위ID
    public int DiceID = -1;

    // 주사위 눈금 값
    public int DiceValue = -1;


    // 주사위 고정을 위한 bool 변수
    public bool m_IsHeld = false;


    PhotonView pv;


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    public void SetColor()
    {
        pv.RPC("RPC_SetColor", RpcTarget.All, m_IsHeld);
    }

    [PunRPC]
    public void RPC_SetColor(bool IsHeld)
    {
        m_IsHeld = IsHeld;
        if (m_IsHeld)
            this.GetComponent<Renderer>().material.color = Color.red;
        else
            this.GetComponent<Renderer>().material.color = Color.white;
    }

    [PunRPC]
    public void StopDice()
    {
        body.isKinematic = true;
    }

    [PunRPC]
    public void RPC_SetDiceValue(int newDiceValue, int diceID)
    {
        DiceValue = newDiceValue;
        DiceID = diceID;
    }

    // 처음 스폰할때 랜덤으로 굴리기 위해 호출되는 코루틴 함수 
    public IEnumerator Co_Roll(int InDiceID)
    {
        DiceID = InDiceID;
        DiceValue = -1;

        body = GetComponent<Rigidbody>();
        body.AddTorque(Random.onUnitSphere * 50f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        // 반약 애매하게 꼈을때 시간이 너무 끌리지 않도록 하기 위한 시간 변수
        float delaytime = 3.0f;

        while (true)
        {
            if(body)
            {
                delaytime -= Time.deltaTime;

                bool isStoppend = body.velocity.sqrMagnitude < 0.0001f && body.angularVelocity.sqrMagnitude < 0.0001f;
                if (body.IsSleeping() || isStoppend || delaytime <= 0.0f)
                {
                    int num = 0;
                    for (int i = 1; i < DiceNumPos.Length; i++)
                    {
                        if (DiceNumPos[num].position.y < DiceNumPos[i].position.y)
                            num = i;
                    }

                    pv.RPC("RPC_SetDiceValue", RpcTarget.All, num + 1, DiceID);

                    if(PhotonNetwork.IsMasterClient)
                    {
                        Table_Mgr.Inst.StopDice();
                        break;
                    }
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }

    // 주사위 이동을 위한 코루틴 함수
    public IEnumerator Co_MoveDiec(float moveTime, Vector3 targetPos, Quaternion targetRot = new Quaternion())
    {
        yield return null;
        // 주사위 중력 제거
        pv.RPC("StopDice", RpcTarget.All);

        Vector3 startPos = this.transform.position;       // 시작 위치
        Quaternion startRot = this.transform.rotation; // 시작 회전

        float time = 0;

        while (time < moveTime)
        {
            if (this == null)
                break;

            time += Time.deltaTime;
            float lerp = time / moveTime;


            // 위치와 회전을 선형 보간으로 이동/회전
            transform.position = Vector3.Lerp(startPos, targetPos, lerp);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, lerp);

            yield return null;
        }
    }
}
