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

// 게임을 시작했는지를 따지는 열거형
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

    // 카메라를 붙일 지점
    public GameObject CameraTargetRoot;

    // 상호작용 모듈
    [HideInInspector] public M_Interaction m_Interaction;



    public PhotonView pv;


    #region 이동관련 변수
    public bool IsCanMove = true;

    // 원격 캐릭터의 움직임
    Vector3 NetworkPos;     // 네트워크에서 받은 위치
    Quaternion NetworkRot;  // 네트워크에서 받은 회전


    EMoveType MoveType = EMoveType.Idle; // 이동 상태
    public Vector2 Move = Vector2.zero;  // 이동 방향

    public float Speed;                  // 이동 속도 변수
    protected float WalkSpeed = 2.0f;       // 걷기 속도
    protected float JogingSpeed = 3.5f;       // 걷기 속도


    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    private float CinemachineTargetYaw;
    private float CinemachineTargetPitch;

    public float TargetRotation = 0.0f;  // 회전 타겟 방향
    protected float RotationVelocity;       // 회전 속도

    public float SpeedChangeRate = 10.0f;   // 속도 변화율
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;    // 회전시 천천히 돌때 사용


    [Header("Module")]
    public M_Animation m_Animation;

    protected float AnimationBlend;      // 이동시 애니메이션 블랜드
    #endregion


    // 게임 관련 변수
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
    public ScoreCombination PlayerScore = new ScoreCombination(); // 각 플레이어들의 점수




    void Awake()
    {
        // 컴포넌트
        Controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();

        // 모듈
        m_Animation = gameObject.AddComponent<M_Animation>();

        m_Interaction = gameObject.AddComponent<M_Interaction>();


        // 플레이어의 상태
        PlayerState = EPlayerState.NotReady;
        // 플레이어의 턴 관리
        PlayTurnState = EPlayTurnState.NotMyTurn;
    }

    void Start()
    {
        PlayerName = pv.Owner.NickName;

        // 로컬일 때만
        if (pv.IsMine)
        {
            // PlayerInput_Mgr에 로컬 플레이어(조작 중인 플레이어 저장)
            Game_Mgr.Inst.LocalPlayer = this;

            // 카메라 지정
            Camera_Mgr.Inst.ChangeTarget(CameraTargetRoot.transform);
            Camera_Mgr.Inst.SetCameraZoom(2.5f);

            // 카메라 회전값 초기화
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

    // 카메라 회전
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

 
    // 움직임
    void CharMove()
    {
        if (pv.IsMine)
        {
            #region 로컬일때 움직임
            if (!IsCanMove) return;

            // 인풋시스템에서 Vector2값 가져오기
            Move = PlayerInput_Mgr.Inst.InputMove;

            if (Move == Vector2.zero)
            {
                MoveType = EMoveType.Idle;
            }
            else
            {
                MoveType = (PlayerInput_Mgr.Inst.bWalk) ? EMoveType.Walk : EMoveType.Joging;
            }

            // 속도 설정
            float targetSpeed = 0.0f;



            // 입력이 있을경우 경우 속도를 설정
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


            // 노멀라이즈
            Vector3 inputDirection = new Vector3(Move.x, 0.0f, Move.y).normalized;

            //이동 입력이 있는 경우 플레이어가 이동할 때 회전
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


            // 애니메이션
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
            #region 원격 움직임
            // 회전 보간
            transform.rotation = Quaternion.Lerp(transform.rotation, NetworkRot, Time.deltaTime * 10f);

            // 위치 보간
            Vector3 targetPos = NetworkPos - transform.position;

            Controller.Move(targetPos.normalized * (Speed * Time.deltaTime) +
                                 new Vector3(0.0f, -2, 0.0f) * Time.deltaTime);

            // 애니메이션
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
        {   //로컬 플레이(IsMine)의 위치 정보 송신
            stream.SendNext((Vector3)transform.position);
            stream.SendNext((Quaternion)transform.rotation);
            stream.SendNext((float)Speed);
            stream.SendNext((int)MoveType);

            if (m_Animation)
                stream.SendNext((bool)m_Animation.IsSit);
        }
        else  //원격 플레이어(아바타)의 위치 정보 수신
        {
            NetworkPos = (Vector3)stream.ReceiveNext();
            NetworkRot = (Quaternion)stream.ReceiveNext();
            Speed = (float)stream.ReceiveNext();
            MoveType = (EMoveType)stream.ReceiveNext();

            if (m_Animation)
                m_Animation.IsSit = (bool)stream.ReceiveNext();
        }
    }




    // 의자에 앉을 때 코루틴 함수
    public IEnumerator Co_SitDown(Transform SitPos)
    {
        
        IsCanMove = false; // 플레이어 움직임 막기

        // 이동 방향으로 회전
        Quaternion lookRot = Quaternion.LookRotation(SitPos.position - this.transform.position);
        lookRot.x = 0.0f;
        lookRot.z = 0.0f;
        while (Quaternion.Angle(this.transform.rotation, lookRot) > 10.0f)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRot, 4.0f * Time.deltaTime);
            yield return null;
        }



        // 지정된 위치로 이동
        while(Vector3.Distance(this.transform.position, SitPos.position) > 0.1f)
        {
            Vector3 direction = (SitPos.position - this.transform.position).normalized;
            direction.y = 0.0f;

            Controller.Move(direction * WalkSpeed * Time.deltaTime);

            // 애니메이션
            m_Animation.MoveType((int)EMoveType.Walk);

           yield return null;
        }
            m_Animation.MoveType((int)EMoveType.Idle);

        // 회전
        while (Quaternion.Angle(this.transform.rotation, SitPos.rotation) > 1.0f)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, SitPos.rotation, 3.0f * Time.deltaTime);
            yield return null;
        }

        // 앉는 애니메이션 실행
        m_Animation.IsSit = true;



        if(pv.IsMine)
        {
            // UI 변경
            UICanvas_Mgr.Inst.SetUIType(EUIType.Ready);

            // 플레이어의 상태를 변경
            PlayerState = EPlayerState.Ready;
        }
        
    }


    public IEnumerator Co_SitUp()
    {
        // 가장 가까히 있는 의자의 상호작용 활성화
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


        m_Animation.IsSit = false; // 앉는 애니메이션 실행
        PlayerState = EPlayerState.NotReady; // 상태 수정
        UICanvas_Mgr.Inst.SetUIType(EUIType.Play); // UI 업데이트

        yield return new WaitForSeconds(3.3f); // 일어난 후

        // 플레이어 움직임 켜기
        IsCanMove = true;
    }


}
