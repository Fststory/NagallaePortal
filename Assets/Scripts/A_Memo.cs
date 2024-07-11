using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Memo : MonoBehaviour
{
    /* 구현한 스크립트, 태그, 레이어 등 프로젝트에 대한 메모장!


                                                                [태그]
    smallButton
    LabObject
    Bullet
    Portal
    RedPortal
    BluePortal
    Ground : 닿아 있으면 점프 가능(바닥에 사용할 목적)


                                                                [레이어]
    CanMakePortal : 사용 시 포탈 생성 허용



                                                                [스크립트]

    <공동>

    GameManager : 각종 이벤트 구현 ( 씬 이동,



    <민경>

    Autodoor : 
    BulletMove : 
    DoorButtonBig :
    DoorButtonBig_another :
    DoorButtonSmall :
    GrabMe : 
    LobbyUI :
    Turret :



    <영호>

    Player : 플레이어의 전후좌우 이동 및 점프, 총알을 맞았을 때 체력 계산
    Portal : 포탈 기능 ( 빨강, 파랑 포탈이 갖는 컴포넌트)
    CanUsePortal : 포탈 간 이동할 때 처리해야 되는 것을 담음 ( 이 컴포넌트를 가진 오브젝트만 포탈 이용 가능, 포탈 이동 시 시점 변환 등)
    CamRotate : 마우스 이동에 따른 오브젝트의 회전
    PortalGunFire : 포탈 설치 기능

    


     */
}
