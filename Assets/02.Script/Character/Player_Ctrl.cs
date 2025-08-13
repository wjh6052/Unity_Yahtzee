using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;


public enum EMoveType
{
    Idle = 0,
    Walk = 1,
    Joging = 2,
}

// ������ �����ߴ����� ������ ������
public enum EPlayerState
{
    NotReady,
    Ready,
    Play
}

public enum EPlayTurnState
{
    NotMyTurn,
    MyTurn
}

public class Player_Ctrl : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Controller")]
    protected CharacterController Controller;

    
    public string PlayerName = "";

    // ī�޶� ���� ����
    public GameObject CameraTargetRoot;

    // ��ȣ�ۿ� ���
    [HideInInspector] public M_Interaction m_Interaction;



    public PhotonView pv;


    #region �̵����� ����
    public bool IsCanMove = true;

    // ���� ĳ������ ������
    Vector3 NetworkPos;     // ��Ʈ��ũ���� ���� ��ġ
    Quaternion NetworkRot;  // ��Ʈ��ũ���� ���� ȸ��


    EMoveType MoveType = EMoveType.Idle; // �̵� ����
    public Vector2 Move = Vector2.zero;  // �̵� ����

    public float Speed;                  // �̵� �ӵ� ����
    protected float WalkSpeed = 2.0f;       // �ȱ� �ӵ�
    protected float JogingSpeed = 3.5f;       // �ȱ� �ӵ�


    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    private float CinemachineTargetYaw;
    private float CinemachineTargetPitch;

    public float TargetRotation = 0.0f;  // ȸ�� Ÿ�� ����
    protected float RotationVelocity;       // ȸ�� �ӵ�

    public float SpeedChangeRate = 10.0f;   // �ӵ� ��ȭ��
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;    // ȸ���� õõ�� ���� ���


    [Header("Module")]
    public M_Animation m_Animation;

    protected float AnimationBlend;      // �̵��� �ִϸ��̼� ����
    #endregion


    // ���� ���� ����
    public int TurnNum = -1;

    EPlayerState m_PlayerState;
    public EPlayerState PlayerState
    {
        get { return m_PlayerState; }
        set 
        { 
            m_PlayerState = value;

            if(pv == null)
                pv = GetComponent<PhotonView>();
            if (pv && pv.IsMine)
                Photon_Mgr.Inst.SetPlayerState(m_PlayerState);
        }
    }
    public EPlayTurnState PlayTurnState;
    public ScoreCombination PlayerScore = new ScoreCombination(); // �� �÷��̾���� ����




    void Awake()
    {
        // ������Ʈ
        Controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();

        // ���
        m_Animation = gameObject.AddComponent<M_Animation>();

        m_Interaction = gameObject.AddComponent<M_Interaction>();


        // �÷��̾��� ����
        PlayerState = EPlayerState.NotReady;
        // �÷��̾��� �� ����
        PlayTurnState = EPlayTurnState.NotMyTurn;
    }

    void Start()
    {
        PlayerName = pv.Owner.NickName;

        // ������ ����
        if (pv.IsMine)
        {
            // PlayerInput_Mgr�� ���� �÷��̾�(���� ���� �÷��̾� ����)
            Game_Mgr.Inst.LocalPlayer = this;

            // ī�޶� ����
            Camera_Mgr.Inst.ChangeTarget(CameraTargetRoot.transform);
            Camera_Mgr.Inst.SetCameraZoom(2.5f);

            // ī�޶� ȸ���� �ʱ�ȭ
            CinemachineTargetYaw = CameraTargetRoot.transform.rotation.eulerAngles.y;
        }


        Game_Mgr.Inst.PlayerList.Add(this);



    }


    void Update()
    {
        CharMove();

    }

    void LateUpdate()
    { 
        CameraRotation();
    }

    // ī�޶� ȸ��
    void CameraRotation()
    {
        if (pv.IsMine == false) return;

        Vector2 look = PlayerInput_Mgr.Inst.InputLook;

        if (look.sqrMagnitude >= 0.01f)
        {
            CinemachineTargetYaw += look.x;
            CinemachineTargetPitch += look.y;
        }

        CinemachineTargetYaw = MathUtility.ClampAngle(CinemachineTargetYaw, float.MinValue, float.MaxValue);
        CinemachineTargetPitch = MathUtility.ClampAngle(CinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CameraTargetRoot.transform.rotation = Quaternion.Euler(CinemachineTargetPitch + 0.0f,
            CinemachineTargetYaw, 0.0f);
    }

 
    // ������
    void CharMove()
    {
        if (pv.IsMine)
        {
            #region �����϶� ������
            if (!IsCanMove) return;

            // ��ǲ�ý��ۿ��� Vector2�� ��������
            Move = PlayerInput_Mgr.Inst.InputMove;

            if (Move == Vector2.zero)
            {
                MoveType = EMoveType.Idle;
            }
            else
            {
                MoveType = (PlayerInput_Mgr.Inst.bWalk) ? EMoveType.Walk : EMoveType.Joging;
            }

            // �ӵ� ����
            float targetSpeed = 0.0f;



            // �Է��� ������� ��� �ӵ��� ����
            if (Move != Vector2.zero)
            {
                switch (MoveType)
                {
                    case EMoveType.Walk:
                        targetSpeed = WalkSpeed;
                        break;
                    case EMoveType.Joging:
                        targetSpeed = JogingSpeed;
                        break;
                }
            }
            else
                targetSpeed = 0.0f;

            Speed = Mathf.Lerp(Speed, targetSpeed, Time.deltaTime * SpeedChangeRate);
            Speed = Mathf.Round(Speed * 1000f) / 1000f;


            // ��ֶ�����
            Vector3 inputDirection = new Vector3(Move.x, 0.0f, Move.y).normalized;

            //�̵� �Է��� �ִ� ��� �÷��̾ �̵��� �� ȸ��
            if (Move != Vector2.zero)
            {
                TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation, ref RotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, TargetRotation, 0.0f) * Vector3.forward;

            // move the player
            Controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) +
                                 new Vector3(0.0f, -2, 0.0f) * Time.deltaTime);


            // �ִϸ��̼�
            if (m_Animation)
            {
                AnimationBlend = Mathf.Lerp(AnimationBlend, (int)MoveType, Time.deltaTime * SpeedChangeRate);
                if (AnimationBlend < 0.01f) AnimationBlend = 0f;

                m_Animation.MoveType(AnimationBlend);
            }
            #endregion
        }
        else
        {
            #region ���� ������
            // ȸ�� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, NetworkRot, Time.deltaTime * 10f);

            // ��ġ ����
            Vector3 targetPos = NetworkPos - transform.position;

            Controller.Move(targetPos.normalized * (Speed * Time.deltaTime) +
                                 new Vector3(0.0f, -2, 0.0f) * Time.deltaTime);

            // �ִϸ��̼�
            if (m_Animation)
            {
                AnimationBlend = Mathf.Lerp(AnimationBlend, (int)MoveType, Time.deltaTime * SpeedChangeRate);
                if (AnimationBlend < 0.01f) AnimationBlend = 0f;

                m_Animation.MoveType(AnimationBlend);
            }
            #endregion
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {   //���� �÷���(IsMine)�� ��ġ ���� �۽�
            stream.SendNext((Vector3)transform.position);
            stream.SendNext((Quaternion)transform.rotation);
            stream.SendNext((float)Speed);
            stream.SendNext((int)MoveType);

            if (m_Animation)
                stream.SendNext((bool)m_Animation.IsSit);
        }
        else  //���� �÷��̾�(�ƹ�Ÿ)�� ��ġ ���� ����
        {
            NetworkPos = (Vector3)stream.ReceiveNext();
            NetworkRot = (Quaternion)stream.ReceiveNext();
            Speed = (float)stream.ReceiveNext();
            MoveType = (EMoveType)stream.ReceiveNext();

            if (m_Animation)
                m_Animation.IsSit = (bool)stream.ReceiveNext();
        }
    }




    // ���ڿ� ���� �� �ڷ�ƾ �Լ�
    public IEnumerator Co_SitDown(Transform SitPos)
    {
        
        IsCanMove = false; // �÷��̾� ������ ����

        // �̵� �������� ȸ��
        Quaternion lookRot = Quaternion.LookRotation(SitPos.position - this.transform.position);
        lookRot.x = 0.0f;
        lookRot.z = 0.0f;
        while (Quaternion.Angle(this.transform.rotation, lookRot) > 10.0f)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRot, 4.0f * Time.deltaTime);
            yield return null;
        }



        // ������ ��ġ�� �̵�
        while(Vector3.Distance(this.transform.position, SitPos.position) > 0.1f)
        {
            Vector3 direction = (SitPos.position - this.transform.position).normalized;
            direction.y = 0.0f;

            Controller.Move(direction * WalkSpeed * Time.deltaTime);

            // �ִϸ��̼�
            m_Animation.MoveType((int)EMoveType.Walk);

           yield return null;
        }
            m_Animation.MoveType((int)EMoveType.Idle);

        // ȸ��
        while (Quaternion.Angle(this.transform.rotation, SitPos.rotation) > 1.0f)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, SitPos.rotation, 3.0f * Time.deltaTime);
            yield return null;
        }

        // �ɴ� �ִϸ��̼� ����
        m_Animation.IsSit = true;



        if(pv.IsMine)
        {
            // UI ����
            UICanvas_Mgr.Inst.SetUIType(EUIType.Ready);

            // �÷��̾��� ���¸� ����
            PlayerState = EPlayerState.Ready;
        }
        
    }


    public IEnumerator Co_SitUp()
    {
        // ���� ������ �ִ� ������ ��ȣ�ۿ� Ȱ��ȭ
        {
            Interaction_Chair[] chairs = FindObjectsOfType<Interaction_Chair>();

            if (chairs.Length > 0)
            {
                Interaction_Chair nearest = chairs
                            .OrderBy(c => Vector3.Distance(this.transform.position, c.transform.position))
                            .FirstOrDefault();

                nearest.SetIsOnInteraction(true);
            }
        }


        m_Animation.IsSit = false; // �ɴ� �ִϸ��̼� ����
        PlayerState = EPlayerState.NotReady; // ���� ����
        UICanvas_Mgr.Inst.SetUIType(EUIType.Play); // UI ������Ʈ

        yield return new WaitForSeconds(3.3f); // �Ͼ ��

        // �÷��̾� ������ �ѱ�
        IsCanMove = true;
    }


}
