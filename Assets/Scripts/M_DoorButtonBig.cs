using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class M_DoorButtonBig : MonoBehaviour
{
    //6.27
    // 음... ㅇㅋ.
    // 이제 버튼이 안 뚫리려면 어떻게 해야 하는지 알아보자.


    //플레이어나 오브젝트, 충돌체가 버튼의 위로 올라오면
    //버튼이 내려가고
    //문이 열린다

    //버튼이 충돌 상태가 아닐 때는
    //문이 닫힌다


    public GameObject pressButton;
    public GameObject BodyButton;
    public GameObject floorMesh;

    public GameObject openDoorLeft;
    public GameObject openDoorRight;


    public GameObject Obj;


    public bool isActivate = false; //통로 활성화 중
    public bool isOpen = false; //문 열리는 연출 중?
    public bool isClose = false; //문 닫히는 연출 중?

    float pressSpeed = 1f; //버튼 연출 속도
    float openSpeed = 8; //문 연출 속도


    float buttonMoveTime = 0.0f;
    float buttonMoveEndTime = 0.2f;


    #region 가이드라인 머태리얼 변수

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
