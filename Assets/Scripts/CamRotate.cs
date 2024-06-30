using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CamRotate : MonoBehaviour
{
    public float rotSpeed = 5;

    float mx = 0;
    float my = 0;

    void Start()
    {
        // ���콺 Ŀ���� ȭ�� ������ ������ �ʵ��� �߾ӿ� �����Ѵ�.(���콺 Ŀ���� ��ġ�� ���ֹ��� �ʰ� �÷��� ����!)
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // �÷��̾��� ���콺 �Է¿� ���� ���� ��ȭ(�����¿�)�� ����
        
        // ���콺 �Է��� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ȸ�� ������ ���� ������Ų��.
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        // ���� ȸ������ �����Ѵ�.
        my = Mathf.Clamp(my, -90f, 90f);

        // ȸ���������� ������ ȸ����Ų��.
        transform.eulerAngles = new Vector3(-my, mx, 0);

    }
}
