using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CamRotate : MonoBehaviour
{
    #region ����(���)
    //public float rotSpeed = 5;

    //float mx = 0;
    //float my = 0;

    //void Start()
    //{
    //    // ���콺 Ŀ���� ȭ�� ������ ������ �ʵ��� �߾ӿ� �����Ѵ�.(���콺 Ŀ���� ��ġ�� ���ֹ��� �ʰ� �÷��� ����!)
    //    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    //}

    //void Update()
    //{
    //    // �÷��̾��� ���콺 �Է¿� ���� ���� ��ȭ(�����¿�)�� ����

    //    // ���콺 �Է��� �޴´�.
    //    float mouseX = Input.GetAxis("Mouse X");
    //    float mouseY = Input.GetAxis("Mouse Y");

    //    // ȸ�� ������ ���� ������Ų��.
    //    mx += mouseX * rotSpeed * Time.deltaTime;
    //    my += mouseY * rotSpeed * Time.deltaTime;

    //    // ���� ȸ������ �����Ѵ�.
    //    my = Mathf.Clamp(my, -90f, 90f);

    //    // ȸ���������� ������ ȸ����Ų��.
    //    transform.eulerAngles = new Vector3(-my, mx, 0);

    //}
    #endregion

    private const float cameraSpeed = 3.0f;

    public Quaternion TargetRotation { private set; get; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;       // Ŀ�� ȭ�� �߾� ����

        TargetRotation = transform.rotation;            // ī�޶��� ���� ȸ�� ���ʹϾ��� ����(TargetRotation)�� ����
    }

    private void LateUpdate()
    {
        // ī�޶� ȸ�� [���콺 �̵�]
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));    // ���콺 x,y�� �̵�(�����¿�)�� ������ ��´�.(Vector2)
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;     // ���콺 �̵� ��� & ���� => ���� �ٶ󺸴� ����. (Vector3)
                                                                                            // v = v0 + delta.v
        // ���� �þ߰� ���� 
        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);         // (���� �ٶ󺸴� ����)����3 => ���ʹϾ����� ��ȯ

        // ī�޶� ���콺 �Է¿� ���� �̵��ϴ� ���� �����ϴ� �κ�
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
    }

    // ������ ���!!! (�÷��̾� ������Ʈ���� ����ž� ��, �ٸ� ��ü[�ͷ�,�����ڽ� ��]�� �÷��̾ ���еǴ� ���)
    // ��Ż �̵����� ������ ������ ȸ���� �ٽ� �������� ������ �� (�׻� ���� �� ������ ���� ��ǥ���� ������ ���� ������ �ǰԲ�)
    // (���� ��� ���� �� xz��鿡 ������ ������ �������� �ٶ�����
    // ��Ż���� ���� �� yz����� �� �ؿ� �дٸ� yz��鿡 ������ ������ �������� �ٶ󺸰� �ȴ�. �̶� �ٽ� ó���� ���·� ���ư��� ��)
    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
