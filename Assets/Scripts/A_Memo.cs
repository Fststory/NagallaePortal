using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Memo : MonoBehaviour
{
    /* ������ ��ũ��Ʈ, �±�, ���̾� �� ������Ʈ�� ���� �޸���!


                                                                [�±�]
    smallButton
    LabObject
    Bullet
    Portal
    RedPortal
    BluePortal
    Ground : ��� ������ ���� ����(�ٴڿ� ����� ����)


                                                                [���̾�]
    CanMakePortal : ��� �� ��Ż ���� ���



                                                                [��ũ��Ʈ]

    <����>

    GameManager : ���� �̺�Ʈ ���� ( �� �̵�,



    <�ΰ�>

    Autodoor : �ڵ���. Delay �ð� �ڿ� ���� ������. (door - �� GameObject / open - Lerp Transform)
    BulletMove : �ͷ� �Ѿ� �߻� (���� �������� ������ �̵��ϰ� �ð��� ���� �� Distroy���� �Ȱ���)
    DoorButtonBig : Ÿ�̸Ӹ� �̿��� ��ư������ ������ (�� �ϵ� �ȵ� ���ٹ����̴ϴ�)
    DoorButtonBig_another : Lerp�� �̿��� ������(������Ÿ�Կ� �� ����ϴ�)
    DoorButtonSmall : ���� ��ư ������ ������(���ٹ����̴ϴ�)
    M_ButtonSmallspcube: ť�� ������ ���� ���ο� ��ư ��ũ��Ʈ�Դϴ�.
    GrabMe : ���� ���� (grab OBJ�� ���� ���ӿ�����, Point�� �÷��̾� �ڽĿ�����Ʈ�� ������ Transform)
    LobbyUI : �κ� UI
    Turret : �ͷ��� �÷��̾� �ν�, �Ѿ� �߻��� �ð� ��



    <��ȣ>

    Player : �÷��̾��� �����¿� �̵� �� ����, �Ѿ��� �¾��� �� ü�� ���
    Portal : ��Ż ���
    MovingWithPortal : ��Ż �� �̵��� �� ó���ؾ� �Ǵ� ���� ���� ( �̰� �ִ� ������Ʈ�� ��Ż �̿� ����, ��Ż �̵� �� ���� ��ȯ ��)
    CamRotate : ���콺 �̵��� ���� ������Ʈ�� ȸ��
    PortalGunFire : ��Ż ��ġ ���
    


     */
}
