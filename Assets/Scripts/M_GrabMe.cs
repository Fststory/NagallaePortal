using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_GrabMe : MonoBehaviour
{
    #region �׷� ������(~7/14)
    //public GameObject grabObj;
    //public Transform PlayerGrabPoint;

    //public bool detec;

    //public bool imGrapping;
    #endregion

    public Transform hand; // �������� ��� ��ġ(�÷��̾��� �� ��ġ)
    public float grabRange = 2.0f; // �������� ���� �� �ִ� �Ÿ�
    private Transform grabbedObject; // ���� ��� �ִ� �������� Transform
    private bool isGrabbing = false; // �������� ��� �ִ��� ����
    private Rigidbody grabbedRigidbody; // ���� �������� Rigidbody
    private Camera mainCamera; // Main Camera�� ����

    void Start()
    {
        mainCamera = Camera.main; // Main Camera�� ������ ������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrabbing)
            {
                // �������� ����߸���
                DropItem();
            }
            else
            {
                // �������� ��´�
                TryGrabItem();
            }
        }
    }
    #region �׷� ������(~7/14)
    //if (imGrapping)
    //{
    //    if (Input.GetKeyDown(KeyCode.R)) //[7/12]Ŭ������ ��ȣ�ۿ� E-RŰ�� ����!!
    //    {
    //        imGrapping = false;
    //        grabObj.transform.SetParent(null); //�θ� ������Ʈ ����
    //        Rigidbody rb = grabObj.GetComponent<Rigidbody>();
    //        rb.useGravity = true;

    //        //gameObject.AddComponent<Rigidbody>();

    //    }
    //}
    #endregion

    void TryGrabItem()
    {
        RaycastHit hit;
        Vector3 rayOrigin = mainCamera.transform.position;
        Vector3 rayDirection = mainCamera.transform.forward;
        Debug.DrawRay(rayOrigin, rayDirection * grabRange, Color.green, 1.0f); // ����׿� Ray �׸���

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, grabRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // ����� �޽���

            if (hit.collider != null && hit.collider.CompareTag("LabObject"))
            {
                // �������� ��´�
                grabbedObject = hit.transform;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.isKinematic = true; // ������ ��ȣ�ۿ��� ��Ȱ��ȭ
                    grabbedObject.SetParent(hand); // �������� �÷��̾��� �� ��ġ�� ����
                    grabbedObject.localPosition = Vector3.zero; // �� ��ġ�� ��Ȯ�� ��ġ
                    isGrabbing = true;
                }
            }
        }
    }

    void DropItem()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null); // �θ� ���� ����
            grabbedRigidbody.isKinematic = false; // ������ ��ȣ�ۿ��� Ȱ��ȭ
            grabbedObject = null;
            grabbedRigidbody = null;
        }
        isGrabbing = false;
    }

}

    #region �׷� ������(~7/14)
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "LabObject")
    //    {
    //        detec = true;

    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            grabObj = (GameObject)other.gameObject; //���ӿ�����Ʈ ������ �߰� (�־ȵǳ�?)

    //            grabObj.transform.position = PlayerGrabPoint.position;
    //            //grabObj.transform.SetParent(gameObject.transform); //�θ� ������Ʈ ����
    //            grabObj.transform.SetParent(PlayerGrabPoint);        //��7.7 �θ� ������Ʈ ����

    //            Rigidbody rb = grabObj.GetComponent<Rigidbody>();
    //            rb.useGravity = false;

    //            //Destroy(rb);

    //            imGrapping = true;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "LabObject")
    //    {
    //        detec=false;
    //    }
    //}
    #endregion

