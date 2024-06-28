using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class M_DoorButtonBig : MonoBehaviour
{
    //6.27
    // ��... ����.
    // ���� ��ư�� �� �ո����� ��� �ؾ� �ϴ��� �˾ƺ���.


    //�÷��̾ ������Ʈ, �浹ü�� ��ư�� ���� �ö����
    //��ư�� ��������
    //���� ������

    //��ư�� �浹 ���°� �ƴ� ����
    //���� ������


    public GameObject pressButton;
    public GameObject BodyButton;
    public GameObject floorMesh;

    public GameObject openDoorLeft;
    public GameObject openDoorRight;


    public GameObject Obj;


    public bool isActivate = false; //��� Ȱ��ȭ ��
    public bool isOpen = false; //�� ������ ���� ��?
    public bool isClose = false; //�� ������ ���� ��?

    public float pressSpeed = 0.3f; //��ư ���� �ӵ�
    public float openSpeed = 8; //�� ���� �ӵ�


    public float buttonMoveTime = 0.0f;
    public float buttonMoveEndTime = 0.5f;


    void Start()
    {

    }

    void Update()
    {

        if (isOpen)
        {
            Vector3 pressing = transform.up * -1;
            pressButton.transform.position += pressing * pressSpeed * Time.deltaTime;
            Vector3 sliding = transform.right * -1;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            Vector3 slidding = transform.right;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isOpen = false;
            }
        }

        if (isClose)
        {
            Vector3 pressDown = transform.up;
            pressButton.transform.position += pressDown * pressSpeed * Time.deltaTime;
            Vector3 sliding = transform.right;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            Vector3 slidding = transform.right * -1;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isClose = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject == pressButton || other.gameObject == BodyButton)
        //{
        //    isOpen = true;
        //}
        if (other.gameObject == Obj)
        {
            isOpen = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject == pressButton || other.gameObject == BodyButton || other.gameObject == floorMesh)
        //{
        //    isActivate = true;
        //}

        if (other.gameObject == Obj)
        {
            isActivate = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject == pressButton || other.gameObject == BodyButton)
        //{
        //    isClose = true;
        //}
        if (other.gameObject == Obj)
        {
            isClose = true;
        }
    }

}
