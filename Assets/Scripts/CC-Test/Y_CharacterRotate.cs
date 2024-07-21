using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CharacterRotate : MonoBehaviour
{
    private const float cameraSpeed = 3.0f;
    public Quaternion TargetRotation { private set; get; }

    private void Awake()
    {
        TargetRotation = transform.rotation;            // ī�޶��� ���� ȸ�� ���ʹϾ��� ����(TargetRotation)�� ����
    }
    private void LateUpdate()
    {
        var targetEuler = TargetRotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0) * cameraSpeed;     // ���콺 �̵� ��� & ���� => ���� �ٶ󺸴� ����. (Vector3)
        TargetRotation = Quaternion.Euler(targetEuler);         // (���� �ٶ󺸴� ����)����3 => ���ʹϾ����� ��ȯ

        // ī�޶� ���콺 �Է¿� ���� �̵��ϴ� ���� �����ϴ� �κ�
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
    }
}
