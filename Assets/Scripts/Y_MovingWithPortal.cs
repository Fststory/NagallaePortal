using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Y_MovingWithPortal : MonoBehaviour
{
    // 이 스크립트를 가진 게임오브젝트는 포탈 간 이동이 가능합니다.
    // 필요 변수 : 서로 연결 될 Red & Blue Portal, 포탈 간 이동 적용 가능 대상

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
        // 만약 "RedPortal" 태그를 가진 GameObject와 닿았다면
        if (Portal.gameObject.CompareTag("RedPortal"))
        {
            // 씬에 생성돼있는 BluePortal로 이동된다.
            transform.position = bPortal.transform.position;
        }
        //else if (Portal.gameObject.CompareTag("BluePortal"))
        //{
        //    transform.position = rPortal.transform.position;
        //}
    }    

}
