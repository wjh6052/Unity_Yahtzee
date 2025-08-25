using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Dice_Ctrl : MonoBehaviour
{
    public Transform[] DiceNumPos = new Transform[6];  // �ֻ��� ���� �Ǵ�
    Rigidbody body;


    // �ֻ���ID
    public int DiceID = -1;

    // �ֻ��� ���� ��
    public int DiceValue = -1;


    // �ֻ��� ������ ���� bool ����
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

    // ó�� �����Ҷ� �������� ������ ���� ȣ��Ǵ� �ڷ�ƾ �Լ� 
    public IEnumerator Co_Roll(int InDiceID)
    {
        DiceID = InDiceID;
        DiceValue = -1;

        body = GetComponent<Rigidbody>();
        body.AddTorque(Random.onUnitSphere * 50f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        // �ݾ� �ָ��ϰ� ������ �ð��� �ʹ� ������ �ʵ��� �ϱ� ���� �ð� ����
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

    // �ֻ��� �̵��� ���� �ڷ�ƾ �Լ�
    public IEnumerator Co_MoveDiec(float moveTime, Vector3 targetPos, Quaternion targetRot = new Quaternion())
    {
        yield return null;
        // �ֻ��� �߷� ����
        pv.RPC("StopDice", RpcTarget.All);

        Vector3 startPos = this.transform.position;       // ���� ��ġ
        Quaternion startRot = this.transform.rotation; // ���� ȸ��

        float time = 0;

        while (time < moveTime)
        {
            if (this == null)
                break;

            time += Time.deltaTime;
            float lerp = time / moveTime;


            // ��ġ�� ȸ���� ���� �������� �̵�/ȸ��
            transform.position = Vector3.Lerp(startPos, targetPos, lerp);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, lerp);

            yield return null;
        }
    }
}
