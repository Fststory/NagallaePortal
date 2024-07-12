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

    Autodoor : 자동문. Delay 시간 뒤에 문이 열린다. (door - 문 GameObject / open - Lerp Transform)
    BulletMove : 터렛 총알 발사 (한쪽 방향으로 끝없이 이동하고 시간이 지난 뒤 Distroy까지 똑같음)
    DoorButtonBig : 타이머를 이용한 버튼반응형 문열기 (말 하도 안들어서 갖다버릴겁니다)
    DoorButtonBig_another : Lerp를 이용한 문열기(프로토타입에 얘 썼습니다)
    DoorButtonSmall : 작은 버튼 반응형 문열기(갖다버릴겁니다)
    M_ButtonSmallspcube: 큐브 스폰을 위한 새로운 버튼 스크립트입니다.
    GrabMe : 물건 집기 (grab OBJ에 잡을 게임오브제, Point에 플레이어 자식오브젝트로 만들어둔 Transform)
    LobbyUI : 로비 UI
    Turret : 터렛의 플레이어 인식, 총알 발사할 시간 등



    <영호>

    Player : 플레이어의 전후좌우 이동 및 점프, 총알을 맞았을 때 체력 계산
    Portal : 포탈 기능
    MovingWithPortal : 포탈 간 이동할 때 처리해야 되는 것을 담음 ( 이게 있는 오브젝트만 포탈 이용 가능, 포탈 이동 시 시점 변환 등)
    CamRotate : 마우스 이동에 따른 오브젝트의 회전
    PortalGunFire : 포탈 설치 기능
    


     */
}
