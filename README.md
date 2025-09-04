## 미리보기  

<img alt="image" src="https://github.com/user-attachments/assets/d44dff66-6960-4b07-a9cc-9ccb5bc572fd" /><

<img alt="image" src="https://github.com/user-attachments/assets/2a665537-2c27-4a75-9027-93bbc0c3125b" />

<img alt="image" src="https://github.com/user-attachments/assets/b022b12e-3836-4bdc-a6d9-c902d5d500b7" />

  ***
[플레이 영상 (제작중)]( )

***

## 목차

[0. 프로젝트 개요](#0-프로젝트-개요)<br>
[1. 시스템 구조](#1-시스템-구조)<br>
[2. 3D 캐릭터](#2-3D-캐릭터)<br>
[3. 상호작용](#3-상호작용)<br>
[4. 보드게임](#4-보드게임)<br>
[5. 카메라 관리](#5-카메라-관리)<br>
[6. UI](#6-UI)<br>
[7. 멀티 플레이 동기화](#7-멀티-플레이-동기화)<br>
[포트폴리오 관련 링크](#포트폴리오-관련-링크)<br>

***  


## 0. 프로젝트 개요

| 항목       | 내용 |
|------------|------|
| **프로젝트명** | 유니티 주사위 야추 |
| **개발 기간** | 2025.07.23 ~ 2025.08.11 (3주) |
|**사용 Tool**| Unity.2022.3.62f1 <br> - Input System <br> - Cinemachine|
| **목표** | - Unity와 Photon PUN2를 활용한 멀티플레이 보드게임 제작 <br> - 네트워크 동기화 및 턴 기반 게임 로직 구현 |
| **주요 기능** | - 주사위 굴리기, 홀드, 재굴림 기능 <br> - 점수판 및 족보 체크 시스템 <br> - Photon PUN2 기반 멀티플레이어 턴 동기화 <br> - 인풋시스템을 이용한 플레이어 컨트롤 구현 |
| **기여도** | 1인 개발 (설계부터 구현까지 전체 담당) |

**프로젝트 목표**
- Unity & Photon PUN2 네트워크 학습 <br>
- 중앙 관리 스크립트를 통해 게임 시스템 아키텍처 설계 능력 강화 <br>

***  

## 1. 시스템 구조
<details open>
  <summary>시스템 구조 (접기 / 펼치기)</summary>
 
<details open>
  <summary>매니저 스크립트 (접기 / 펼치기)</summary>

- Game_Mgr
    - 로컬 플레이어의 데이터와 플레이어 생성을 담당
    - [Game_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/Game_Mgr.cs)

- Camera_Mgr
    - 카메라의 위치, 줌 인/아웃을 담당
    - [Camera_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/Camera_Mgr.cs)

- Player_Input_Mgr
    - Input System에서 받은 유저의 입력을 저장 및 다른 매니저 스크립트로 옮기는 작업
    - [Player_Input_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/PlayerInput_Mgr.cs)

- Table_Mgr
    - 보드게임의 실질적 턴, 주사위, 준비된 플레이어 데이터를 관리
    - [Table_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/Table_Mgr.cs)

- Photon_Mgr
    - Photon PUN2을 활용한 서버 접속, 로비 접속, 방 찾기, 방 생성, 방 접속, 플레이어의 상태 동기화 등 멀티플레이 기능을 담당하는 스크립트
    - [Photon_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/Photon_Mgr.cs)
 
- UICanvas_Mgr
    - 전체적으로 사용되는 UI를 관리
    - Canvas에서 게임의 상태에 따라 Panel로 관리하여 상태에 따라 스위칭하여 UI가 바뀌도록 설정
    - Canvas안의 UI들의 데이터를 저장 및 관리
    - [UICanvas_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/UICanvas_Mgr.cs)

</details>

<details open>
  <summary>컨트롤 스크립트 (접기 / 펼치기)</summary>

- Player_Ctrl
    - 플레이어 캐릭터의 기능(이동 등)을 담당하며 기능에 따른 모듈을 추가하여 관리합니다
    - [Player_Ctrl 소스코드](Unity_Yahtzee/Assets/02.Script/Character/Player_Ctrl.cs)

- Interaction_Base
    - 상호작용 오브젝트에 붙는 상호작용 오브젝트들의 공통 부모 스크립트
    - 오브젝트의 이름, 활성화여부, 상호작용 이벤트 등 자식에서 재정의 사용하는 공통 기능을 관리합니다
    - 인터페이스를 사용하지 않은 이유 : 상호작용 오브젝트에만 사용될 스크립트로 판단하여 Debug등 공통 코드를 이용하기 위해 상속을 선택함
    - [Interaction_Base 소스코드](Unity_Yahtzee/Assets/02.Script/Interaction/Interaction_Base.cs)
    - 예시 : 준비완료/의자, 방에서 나가기
    - [상호작용 예시(준비완료 / 의자) 소스코드](Unity_Yahtzee/Assets/02.Script/Interaction/Interaction_Chair.cs)

- Dice_Ctrl
    - 주사위 오브젝트에서 사용되는 스크립트
    - Table_Mgr에서 관리되며 스폰되어 굴리기, 멈춘 후 상단을 향하는 눈금을 Table_Mgr를 넘기는 역활을 합니다
    - [Dice_Ctrl 소스코드](Unity_Yahtzee/Assets/02.Script/Dice_Ctrl.cs)

</details>

<details open>
  <summary>모듈형 스크립트 (접기 / 펼치기)</summary>

- M_Animation
    - Player_Ctrl에 의해 플레이어 캐릭터에 자동으로 붙으며 캐릭터의 애니메이션을 담당한다
    - [M_Animation 소스코드](Unity_Yahtzee/Assets/02.Script/Module/M_Animation.cs)

- M_Interaction
    - Player_Ctrl에 의해 플레이어 캐릭터에 자동으로 붙으며 플레이어의 상호작용 데이터 관리를 담당한다
    - [M_Animation 소스코드](Unity_Yahtzee/Assets/02.Script/Module/M_Interaction.cs)

</details>
</details>

## 2. 3D 모델링
<details open>
  <summary>3D 모델링 (접기 / 펼치기)</summary>

<details open>
  <summary>캐릭터 (접기 / 펼치기)</summary>

![Character](https://github.com/user-attachments/assets/44a3c709-be2d-426d-a4cf-10e3fb059522)
![Character Animator](https://github.com/user-attachments/assets/a90feb9d-b31c-4d4f-9a3e-d56c9e4a64b0)
![Character Animator Tree](https://github.com/user-attachments/assets/b64a531a-4aeb-4742-bcc8-ed0c3a119e0e)

- 캐릭터 모델링은 언리얼5의 퀸마네킹을 사용했습니다
- Rig은 Humanoid를 사용하여 리깅하여 애니메이션을 사용했습니다
- Player_Ctrl에서 생성된 M_Animation를 이용하여 애니메이션을 관리하며 Move Tree에서 float값을 이용하여 정지, 걷기, 뛰기를 제어했습니다
- bool IsSit 파라미터 변수릉 이용하여 앉는 애니메이션 -> 앉아있는 자세 -> 일어서는 애니메이션 으로 변경되며 관리했습니다
  
</details>

<details open>
  <summary>맵 (접기 / 펼치기)</summary>
  
![Map](https://github.com/user-attachments/assets/c5ae206c-37b6-4c52-8f2e-21a230439a80))

  - 맵 에셋은 언리얼에서 제공하는 Saloon Interior에셋을 Exporter for Unreal to/for Unity 2025플러그인을 이용하여 유니티로 이주 하여 사용했습니다
  - 엔진 변경 과정에서 머티리얼이 깨지는 현상이 생겨 머티리얼을 제작하여 Base Map, Normal Map, Occlusion Map을 수정하여 사용하였습니다

</details>

<details open>
  <summary>기타 오브젝트 (접기 / 펼치기)</summary>
  
- 의자 및 테이블

    ![Chair / Table](https://github.com/user-attachments/assets/68344161-9549-46b8-853e-85e75f083e49)
  
    - 맵과 동일하게 Saloon Interior에서 가져왔으며 프리팹으로 만들어 관리하고 있습니다
    - 의자의 경우 Photon_Mgr에서 방이 만들어진 후 Table_Mgr에서 방의 최대 인원수 만큼 테이블 주위에 의자가 생성 됩니다
      
 - 주사위

    ![Dice](https://github.com/user-attachments/assets/72dc9888-f037-4d19-82f8-e25ce6bd2d05)
   
    - 게임에서 사용될 주사위 오브젝트 입니다 메쉬는 에셋스토어의 Dice d6 game ready PBR에서 가져왔습니다.
    - Table_Mgr에서 턴이 시작되고 해당 턴의 플레이어가 주사위 굴림 버튼 클릭시 5개가 스폰되며 Rigbody.isKinematic로 멈춤을 판단, 멈추고 0.75초 후 Table_Mgr에 주사위 번호를 넘겨 줍니다.
  
</details>

</details>


## 3. 상호작용
<details open>
  <summary>상호작용 (접기 / 펼치기)</summary>

![상호작용 / 준비 완료](https://github.com/user-attachments/assets/afa11d13-0962-4072-84e1-0466ffd929df)
![상호작용 / 방 나가기](https://github.com/user-attachments/assets/6b20c330-bbc3-40aa-abd7-84c0948ad250)

- 상호작용 오브젝트에는 Interaction_Base를 상속 받은 스크립트와 Collider(Is Trigger = false)컴포넌트를 추가 합니다
- 상호작용 오브젝트의 Collider와 충돌시 플레이어의 모듈인 M_Interaction에서 OnTriggerEnter가 호출, Interaction_Base를 상속받은 오브젝트 여부 판단, 상속을 받았다면 M_Interaction.NowInteraction에 저장합니다.
- 플레이어가 상호작용키(F) 입력 시 M_Interaction.NowInteraction의 null 여부를 판단 후 null이 아니라면 NowInteraction.UseInteraction()을 호출하여 상호작용이 진행됩니다
<details open>
  <summary>상호작용 흐름 이미지 (접기 / 펼치기)</summary>
  
![상호작용 흐름 이미지](https://github.com/user-attachments/assets/3e0db867-5402-4cc8-9e56-727c9a93dce2)

</details>



## 4. 보드게임
<details open>
  <summary>보드게임 (접기 / 펼치기)</summary>

** 게임 시작 **

<img width="432" height="415" alt="image" src="https://github.com/user-attachments/assets/d44dff66-6960-4b07-a9cc-9ccb5bc572fd" /><br>
0. 플레이어들이 각자의 의자에 앉으면 Table_Mgr에서 플레이어의 정보를 List에 저장합니다. 앉은 상태에서 Esc키를 누를 경우 준비 취소를 하며 자리에서 일어나는 애니메이션이 실행됩니다
<br>

<img width="519" height="417" alt="image" src="https://github.com/user-attachments/assets/804bc721-292b-481b-9c9a-6cfc380c2fd9" /><br>
1. 방의 인원수와 준비된 플레이어의 수가 같으면 방장 클라이언트에는 상호작용이 활성화 되며 상호작용키(F)를 입력시 게임이 시작되며 모든 플레이어의 카메라 시점이 테이블 위로 이동하게 됩니다.
<br>

<img width="400" alt="image" src="https://github.com/user-attachments/assets/2a665537-2c27-4a75-9027-93bbc0c3125b" /><br>
2. 게임 시작 후 해당 턴인 플레이어의 화면 좌측 중앙에 주사위 굴리기 버튼이 활성화 됩니다
<br>

<img width="754" height="418" alt="image" src="https://github.com/user-attachments/assets/bd658df2-c38d-4e89-afe1-e3d6be726712" /><br>
3. 주사위는 한턴당 3번 굴릴 수 있으며 주사위 굴리기 버튼을 클릭 시 하늘에서 주사위가 5개 떨어지며 주사위가 굴려집니다
<br>

<img width="400" alt="image" src="https://github.com/user-attachments/assets/d9088009-b51f-4458-b1b6-f69534475bd3" />
<img width="400" alt="image" src="https://github.com/user-attachments/assets/a53e9949-481d-4f11-be4b-96c3166591e7" /><br>
4. 주사위가 모두 멈췄다고 판단시 카메라의 비율에 맞춰 주사위가 카메라 앞으로 이동합니다. 카메라 앞으로 이동한 주사위를 마우스 좌클릭시 주사위가 빨갛게 변하며 위에 정해진 위치로 이동하여 고정됩니다. 고정된 주사위를 클릭시 원래 자리로 돌아옵니다.
<br>

<img width="240" height="418" alt="image" src="https://github.com/user-attachments/assets/bc2de5cc-2914-44e4-98cd-f8ec30a47029" /><br>
5. 주사위가 위로 올라왔을 때 좌측 점수판에 아직 고르지 않은 점수가 활성화하여 점수를 선택할 수 있습니다. 아직 주사위의 숫자가 마음에 들지 않을 경우 남은 주사위 굴리기 버튼을 클릭 시 고정되지 않은 주사위를 다시 굴리게 됩니다
    - (PlayerInput_Mgr에서 마우스 좌클릭 입력신호를 확인 -> Table_Mgr.OnMouseClick() 호출, 로컬플레이어가 현재 턴의 플레이어데이터와 같은 경우 -> 화면의 마우스 위치에서 카메라 방향으로 Ray를 쏴 주사위를 맞췄다면 주사위를 선택했다고 판단)
<br>

<img width="753" height="418" alt="image" src="https://github.com/user-attachments/assets/0c786f0c-dd17-4124-b1e3-8a4831c47031" /><br>
6. 주사위를 3번 모두 굴리게 됬을 경우 주사위 굴리기 버튼이 활성화 되지 않으며 점수를 골라 다음 플레이어에게 턴을 넘기게 됩니다
<br>

<img width="752" height="417" alt="image" src="https://github.com/user-attachments/assets/46d55dee-3a13-489b-bc54-21da7411f36a" /><br>
7. 선택했던 점수가 화면 중앙 하단에 시스템 메시지가 나오며 다음 플레이어에게 턴이 넘어가며 점수판도 해당 턴의 플레이어의 점수판으로 변경됩니다
<br>

***

<img width="400" alt="image" src="https://github.com/user-attachments/assets/4e5229c4-8520-4efb-8df1-d2c8b7f083c7" />
<img width="400" alt="image" src="https://github.com/user-attachments/assets/5c04432d-853d-4867-b576-fc696a4117b1" />><br>
- 화면 우측 상단에 점수확인 버튼 클릭시 UI가 변경되며 현재 플레이 중인 플레이어의 목록이 나옵니다. 점수를 확인하고 싶은 플레이어를 클릭시 좌측 점수판이 선택했던 플레이어의 점수로 변경됩니다. 다시 우측상단의 닫기 버튼을 클릭시 점수확인 창을 닫을 수 있습니다
<br>

<img width="400" alt="image" src="https://github.com/user-attachments/assets/120b3978-3161-48bd-8a78-27593cfb02c6" />
<img width="400" alt="image" src="https://github.com/user-attachments/assets/29e5671c-0e08-4337-9713-ea619a35b4c0" /><br>
- 모든 라운드 종료 후 가장 점수가 높은 유저의 이름이 출력되며 밑에 확인 버튼 클릭 시 게임 시작 전의 상태로 돌아갑니다.
<br>



</details>


## 5. 카메라 관리
<details open>

  <summary>카메라 관리 (접기 / 펼치기)</summary>

- 주사위 야추 프로젝트에서의 카메라는 Cinemachine의 Virtual Camera를 이용하여 카메라를 이동시키며 Camera_Mgr를 이용하여 제어하고 있습니다.
  
- 보드게임에 집중 할 수 있도록 Virtual Camera의 Noise는 제거하여 사용했습니다.

- 플레이어 캐릭터 프리팹에 카메라를 붙일 CameraTargetRoot오브젝트를 생성후 캐릭터 매쉬 목부분에 Y값을 설정 후 Virtual Camera의 Follow를 CameraTargetRoot에 적용시켜 플레이어를 따라가도록 설정하였습니다. 게임 시작 후 카메라 이동도 동일한 방법으로 Table.TableCameraRoot로 설정하여 카메라 이동을 구연했습니다

- 카메라의 회전의 경우 PlayerInput_Mgr의 마우스의 이동값을 vector2로 변환 후 CameraSpeed의 설정값의 속도로 CameraTargetRoot를 회전시켜 마우스에 의한 카메라 회전을 구연하였습니다

</details>


## 6. UI
<details open>
  <summary>UI (접기 / 펼치기)</summary>


<img width="749" height="420" alt="image" src="https://github.com/user-attachments/assets/b022b12e-3836-4bdc-a6d9-c902d5d500b7" />

- LoadingPanel
    - 방 입장을 위해 서버로비에서 출력되는 UI
    - 우측 패널에서 이름, 입장버튼, 방생성, 방이름, 방 최대인원 버튼과 좌측에 현재 열려있는 방이 있다면 동기화 하여 방이름과 현재인원/최대인원이 출력된 버튼이 활성화됩니다



<img width="748" height="416" alt="image" src="https://github.com/user-attachments/assets/5ceb9460-062f-48d4-9670-784b2f2da493" />

- PlayPanel
    - 방에 입장하면 가장 먼저 보이는 UI로 3D 캐릭터를 조작할 때 사용되는 UI입니다
    - 상호작용 오브젝트와 충돌중 일때 플레이어에게 알려주기 위한 상호작용 UI가 포함되어 있습니다



<img width="749" height="409" alt="image" src="https://github.com/user-attachments/assets/1fbb1b81-b2df-42b0-a43e-ede1482e9139" />

- ReadyPanel
    - 플레이어가 의자에 앉으며 준비완료 상태가 된다면 출력되는 UI입니다
    - 준비 취소, 시작하기 상호작용 UI가 보이며 우측 상단에 방의 플레이어인원, 준비 상태, 방장여부를 확인할 수 있습니다
      


<img width="750" height="426" alt="image" src="https://github.com/user-attachments/assets/f23847fe-2592-436b-a87a-8a3ec0dc5be2" />

- GamePanel
    - 보드게임이 시작되면 출력되는 UI입니다
    - 좌측에 현재 턴이 진행중인 플레이어의 점수판과 우측에 주사위 굴림 버튼이 활성화됩니다



<img width="754" height="416" alt="image" src="https://github.com/user-attachments/assets/e17324ed-a3e6-40c6-88bd-e5b754887e0f" />

- GameEndPenel
    - 모든 라운드가 종료하며 출력되는 UI입니다
    - 게임에 참여했던 플레이어의 점수를 종합하여 가장 점수가 높은 플레이어의 이름을 출력합니다.
    - 하단의 버튼으로 게임 중 상태에서 준비 완료 상태로 변경되며 ReadyPanel로 변경됩니다.



<img width="751" height="416" alt="image" src="https://github.com/user-attachments/assets/937bbec9-6d0a-4bbd-8bfc-cddace89821d" />

- ScorePanel
    - GamePanel과 GameEndPenel 상태에서 우측 상단에 점수확인 버튼을 클릭시 활성화 되는 UI입니다.
    - 현재 게임을 진행중인 플레이어의 목록을 보여주며 클릭시 해당 플레이어의 점수를 확인 할 수 있습니다.


<img width="207" height="101" alt="image" src="https://github.com/user-attachments/assets/f25f1248-1c7b-488b-a4bd-ae116ec4a4f4" />

- ChatPanel
    - 채팅을 제공하는 UI로서 화면 우측 하단에 배치합니다
    - PlayerInput_Mgr에서 채팅입력(Enter)키를 입력시 채팅텍스트 박스 하단에 InputField에 마우스가 활성화되며 다시 채팅입력(Enter)키를 입력시 InputField에 입력했던 채팅이 출력됩니다
    - 본인이 입력했던 채팅은 노란색으로 표시되며 다른 유저의 채팅은 흰색으로 확인이 가능합니다
      


<img width="354" height="72" alt="image" src="https://github.com/user-attachments/assets/485233fd-8f46-4fff-b813-3cf72e6b4edc" />

- System Messages Box
    - 모든 패널에서 중앙 하단에 활성화되며 플레이어에게 상태 값을 알려주기 위해 사용되는 UI입니다






</details>


## 7. 멀티 플레이 동기화
<details open>
  <summary>멀티 플레이 동기화 (접기 / 펼치기)</summary>
  
**해당 프로젝트는 Photon PUN2 기반으로 모든 플레이어의 게임 진행을 실시간으로 공유하며 멀티플레이를 구현했습니다**

[Photon_Mgr 소스코드](Unity_Yahtzee/Assets/02.Script/Mgr/Photon_Mgr.cs)


- 턴 관리 동기화
    - 방장(마스터 클라이언트)이/가 턴 순서를 제어하고, 각 클라이언트는 동일한 턴 정보를 공유합니다

- 주사위 상태 동기화
    - 방장의 소유로 스폰하여 모두가 방장 클라이언트가 소유중인 주사위를 통해 같은 주사위 결과 값이 나오도록 설정했습니다
    - 주사위가 멈추면 방장의 Table_Mgr으로 Y값이 가장 높은 주사위 눈의 값을 넘겨주며 결과 값을 얻을 수 있습니다.
      
- 점수판 동기화
    - 방장이 게임을 시작하는 동시에 모든 클라이언트에 게임에 참여한 플레이어의 초기화된 점수판을 배정 후 점수가 결정된다면 모두가 동시에 동기화 하도록 설정하였습니다.

- 캐릭터 동기화
    - 멀티 플레이의 특성 상 로컬 클라이언트의 프레임마다 호출이 어려워 각 플레이어의 위치값, 회전값, 애니메이션 열거형, 이동 속도를 가져와 동기화 하였습니다
      - 위치 : CharacterController.Movw()함수를 이용하여 이동, 이동 속도값이 0이거나 매우 낮은 경우는 바로 위치값 데이터를 받아서 사용
      - 회전 : 현재 회전값에서 받아온 회전 값으로 보간하며 서서히 회전
      - 애니메이션 : 캐릭터의 애니메이션을 열거형 정보로 관리하며 해당 캐릭터의 애니메이션 상태를 바로 바꿀수 있도록 설정



  

</details>



## 포트폴리오 관련 링크

[포트폴리오 영상 보기]()  
[GitHub 소스코드 확인](Unity_Yahtzee/Assets/02.Script)  
[게임 데모 다운로드(구글 드라이브)](https://drive.google.com/drive/folders/1c1mEN12wYT33gJG4vpkChNdLK7aGp-fC?usp=drive_link)  
