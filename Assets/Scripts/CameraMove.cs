using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public float mouseSpeed = 5;
    void Start()
    {
        
    }

    void Update()
    {
        // �÷��̾��� ���콺 �Է¿� ���� ���� ��ȭ(�����¿�)�� ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 mouseDir = new Vector3(-mouseY, mouseX, 0);
        transform.eulerAngles += mouseDir * mouseSpeed * Time.deltaTime;
    }
}
