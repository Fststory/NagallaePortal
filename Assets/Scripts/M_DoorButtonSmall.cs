using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class M_DoorButtonSmall : MonoBehaviour
{

    #region 6/26 �޸�
    // ���콺�� Ŭ������ �� ��ȣ�ۿ��ϴ� �ͺ��� ��������
    // ��ư = true false bool���� �� �̿��ؾ� ��
    // �ð��� ������ ���� ������ ��
    // ��ư�� �� ����̱�:: ButtonPush�� �Ʒ��� ���ٰ� ���� ������

    #endregion

    #region 6/28 �޸�

    //�Լ�ȭ�ϴϱ� ���峪�� �״�εα���ߴ���

    //���...? ���̵����... �ڵ带 ���� �� �ʿ� �ֳ�...?
    //������... ó������ �ȵǳ�?
    // ó������ �Ǵ���(7/1)

    #endregion

    public GameObject pressButton;
    public GameObject openDoorLeft;
    public GameObject openDoorRight;

    float pressSpeed = 1f; //��ư ���� �ӵ�
    float openSpeed = 8f; //�� ���� �ӵ�

    float countOpenTime; 
    public float countCloseTime = 10.0f;

    #region ��ư ����
    float buttonMoveTime =0.0f;
    float buttonMoveEndTime=0.2f;

    public bool isDoorOpen = false; //��ư Ȱ��ȭ ���� ����

    bool isMoving = false; //��ư�� �������� ���� ����
    bool isMovingAgain = false; //��ư�� �ö󰡰� ���� ����
    #endregion

    #region ���̵���� ���¸��� ����

    public GameObject guideline;

    public Material activate;
    public Material deactivate;

    private Renderer lineRenderer;

    #endregion

    public bool playerHere;


    void Start()
    {
        #region Ÿ�̸Ӹ���
        countOpenTime = 0; //Ÿ�̸� ������ �α�
        buttonMoveTime = 0;
        #endregion

        #region ���̵���� ������Ʈ�� Renderer ������Ʈ ��������

        if (guideline != null)
        {
            lineRenderer = guideline.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("guideline �������� �ʾҽ��ϴ�.");
        }

        // �ʱ� Material ����
        UpdateMaterial();
        #endregion
    }

    void Update()
    {
        #region ��ư �⺻ �۵�
        // 6.26
        // Ŭ���ϸ� ��ư�� �Ʒ��� ������
        // true false ��ɾ �̿��ؼ� print ���� ������ / ���� �����ٺ���

        #region 7/3 ���� �߻� ��� (���)
        //if (Input.GetMouseButtonDown(0))    //���� �÷��̾ Ŭ���Ѵٸ�
        //{
        //    //���̸� ī�޶� �������� �߻�
        //    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        //    RaycastHit push = new RaycastHit();

        //    if (Physics.Raycast(ray, out push))
        //    {
        //        if (push.collider != null && push.collider.CompareTag("smallButton"))
        //        {
        //            //isDoorOpen = !isDoorOpen; //�̰� ���������� ���
        //            isDoorOpen = true; //���� Ȱ��ȭ.
        //            if (countOpenTime == 0 && buttonMoveTime == 0)
        //            {
        //                isMoving = true; //��ư�̶� ���� �����̴� ����
        //            }
        //        }

        //    }           
        //}
        #endregion

        if (playerHere && Input.GetMouseButtonDown(0))
        {
            isDoorOpen = true; //���� Ȱ��ȭ.
            if (countOpenTime == 0 && buttonMoveTime == 0)
            {
                isMoving = true; //��ư�̶� ���� �����̴� ����
            }
        }
        
        if (isDoorOpen) //���� ���� �����ٸ�
        {
            countOpenTime += Time.deltaTime;
            if (countOpenTime > countCloseTime) //�ð��� ������
            {
                isDoorOpen = false; //��� ����
                isMovingAgain = true; //�� ������ ����
                countOpenTime = 0; //Ÿ�Ӹ��� �Ǳ�Ծ���!!!!!!!!!
            }
        }

        if (isMoving) //��ư�̶� ���� �������� �ִٸ�
        {
            //��ư�� �Ʒ���... ��ư�� ���� �������� �ϴ� �� ���ڴ�
            Vector3 pressing = transform.up * -1;
            pressButton.transform.position += pressing* pressSpeed * Time.deltaTime;
            //���ʹ��� ��������...
            Vector3 sliding = transform.right * -1;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            //�������� ����������...
            Vector3 slidding = transform.right;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isMoving = false;
            }
        }

        if (isMovingAgain)//��ư�� �ö󰡰� �ִٸ�
        {
            Vector3 pressDown = transform.up;
            pressButton.transform.position += pressDown * pressSpeed * Time.deltaTime;

            //���ʹ��� ����������...
            Vector3 sliding = transform.right;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            //�������� ��������...
            Vector3 slidding = transform.right * -1;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isMovingAgain = false;
            }
        }
        #endregion

        UpdateMaterial();

    }

    private void UpdateMaterial() //���¸��� �ٲ��ִ� �Լ� (��ó: GPT)
    {
        if (lineRenderer != null)
        {
            if (isDoorOpen)
            {
                lineRenderer.material = activate;
            }
            else
            {
                lineRenderer.material = deactivate;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHere = true;
        }
    }

    

}
