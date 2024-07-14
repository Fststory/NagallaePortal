using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_GrabMe : MonoBehaviour
{
    #region 그랩 구버전(~7/14)
    //public GameObject grabObj;
    //public Transform PlayerGrabPoint;

    //public bool detec;

    //public bool imGrapping;
    #endregion

    public Transform hand; // 아이템을 잡는 위치(플레이어의 손 위치)
    public float grabRange = 2.0f; // 아이템을 잡을 수 있는 거리
    private Transform grabbedObject; // 현재 잡고 있는 아이템의 Transform
    private bool isGrabbing = false; // 아이템을 잡고 있는지 여부
    private Rigidbody grabbedRigidbody; // 잡힌 아이템의 Rigidbody
    private Camera mainCamera; // Main Camera의 참조

    void Start()
    {
        mainCamera = Camera.main; // Main Camera의 참조를 가져옴
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrabbing)
            {
                // 아이템을 떨어뜨린다
                DropItem();
            }
            else
            {
                // 아이템을 잡는다
                TryGrabItem();
            }
        }
    }
    #region 그랩 구버전(~7/14)
    //if (imGrapping)
    //{
    //    if (Input.GetKeyDown(KeyCode.R)) //[7/12]클릭에서 상호작용 E-R키로 변경!!
    //    {
    //        imGrapping = false;
    //        grabObj.transform.SetParent(null); //부모 컴포넌트 해제
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
        Debug.DrawRay(rayOrigin, rayDirection * grabRange, Color.green, 1.0f); // 디버그용 Ray 그리기

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, grabRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // 디버그 메시지

            if (hit.collider != null && hit.collider.CompareTag("LabObject"))
            {
                // 아이템을 잡는다
                grabbedObject = hit.transform;
                grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
                if (grabbedRigidbody != null)
                {
                    grabbedRigidbody.isKinematic = true; // 물리적 상호작용을 비활성화
                    grabbedObject.SetParent(hand); // 아이템을 플레이어의 손 위치로 설정
                    grabbedObject.localPosition = Vector3.zero; // 손 위치에 정확히 배치
                    isGrabbing = true;
                }
            }
        }
    }

    void DropItem()
    {
        if (grabbedObject != null)
        {
            grabbedObject.SetParent(null); // 부모 관계 해제
            grabbedRigidbody.isKinematic = false; // 물리적 상호작용을 활성화
            grabbedObject = null;
            grabbedRigidbody = null;
        }
        isGrabbing = false;
    }

}

    #region 그랩 구버전(~7/14)
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "LabObject")
    //    {
    //        detec = true;

    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            grabObj = (GameObject)other.gameObject; //게임오브젝트 변수에 추가 (왜안되냐?)

    //            grabObj.transform.position = PlayerGrabPoint.position;
    //            //grabObj.transform.SetParent(gameObject.transform); //부모 컴포넌트 지정
    //            grabObj.transform.SetParent(PlayerGrabPoint);        //ㄴ7.7 부모 컴포넌트 변경

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

