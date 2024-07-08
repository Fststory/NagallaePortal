using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Y_MovingWithPortal : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� ��Ż �� �̵��� �����մϴ�.
    // �ʿ� ���� : ���� ���� �� Red & Blue Portal, ��Ż �� �̵� ���� ���� ���

    public GameObject rPortal;
    public GameObject bPortal;


    void Start()
    {
        
    }

    void Update()
    {
        bPortal = GameObject.FindGameObjectWithTag("BluePortal");
        rPortal = GameObject.FindGameObjectWithTag("RedPortal");
    }


    
    private void OnTriggerStay(Collider Portal)
    {
        // ���� "RedPortal" �±׸� ���� GameObject�� ��Ҵٸ�
        if (Portal.gameObject.CompareTag("RedPortal"))
        {
            // ���� �������ִ� BluePortal�� �̵��ȴ�.
            transform.position = bPortal.transform.position;
        }
        //else if (Portal.gameObject.CompareTag("BluePortal"))
        //{
        //    transform.position = rPortal.transform.position;
        //}
    }    

}
