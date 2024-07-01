using System;
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

    float pressSpeed = 1f; //��ư ���� �ӵ�
    float openSpeed = 8; //�� ���� �ӵ�


    float buttonMoveTime = 0.0f;
    float buttonMoveEndTime = 0.2f;


    #region ���̵���� ���¸��� ����

    public GameObject guideline;

    public Material activate;
    public Material deactivate;

    private Renderer lineRenderer;

    #endregion


    void Start()
    {
        if (guideline != null)
        {
            lineRenderer = guideline.GetComponent<Renderer>();
        }
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
            lineRenderer.material = activate;
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
            lineRenderer.material = deactivate;
        }
    }


}
