using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorButtonSmall : MonoBehaviour
{
    //�÷��̾�� ��ư�� ���� �κ��� ������

    //��ư�� �� ���� ���󺹱��Ѵ�.
    //���� ������.

    //�ð��� ������ ���� ������.

    #region 6/26 �޸�
    // ���콺�� Ŭ������ �� ��ȣ�ۿ��ϴ� �ͺ��� ��������
    // ��ư = true false bool���� �� �̿��ؾ� ��
    // �ð��� ������ ���� ������ ��
    // ��ư�� �� ����̱�:: ButtonPush�� �Ʒ��� ���ٰ� ���� ������

    #endregion

    public GameObject pressButton;
    public GameObject openDoorLeft;
    public GameObject openDoorRight;

    public float pressSpeed = 1f; //��ư ���� �ӵ�
    public float openSpeed = 5f; //�� ���� �ӵ�

    public float countOpenTime; 
    public float countCloseTime = 10.0f;

    public float buttonMoveTime=0.0f;
    public float buttonMoveEndTime=0.5f;


    bool isDoorOpen = false; //��ư Ȱ��ȭ ���� ����

    public bool isMoving = false; //��ư�� �������� ���� ����
    public bool isMovingAgain = false; //��ư�� �ö󰡰� ���� ����



    void Start()
    {
        countOpenTime = 0; //Ÿ�̸� ������ �α�
        buttonMoveTime = 0;
    }

    void Update()
    {
        // 6.26
        // Ŭ���ϸ� ��ư�� �Ʒ��� ������
        // true false ��ɾ �̿��ؼ� print ���� ������ / ���� �����ٺ���

        if (Input.GetMouseButtonDown(0))    //���� �÷��̾ Ŭ���Ѵٸ�
        {
            //isDoorOpen = !isDoorOpen; //�̰� ���������� ���
            isDoorOpen = true; //���� Ȱ��ȭ.
            isMoving = true; //��ư�̶� ���� �����̴� ����
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

    }

}
