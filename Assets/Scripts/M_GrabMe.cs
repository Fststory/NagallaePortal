using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GrabMe : MonoBehaviour
{
    public GameObject grabObj;
    public Transform PlayerGrabPoint;

    public bool detec;

    public bool imGrapping;

    void Start()
    {
        
    }

    void Update()
    {
        

        if (imGrapping)
        {
            if (Input.GetMouseButtonDown(1))
            {
                imGrapping = false;
                grabObj.transform.SetParent(null); //부모 컴포넌트 해제
                Rigidbody rb = grabObj.GetComponent<Rigidbody>();
                rb.useGravity = true;

                //gameObject.AddComponent<Rigidbody>();

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            detec = true;

            if (Input.GetMouseButtonDown(0))
            {
                grabObj = (GameObject)other.gameObject; //게임오브젝트 변수에 추가 (왜안되냐?)

                grabObj.transform.position = PlayerGrabPoint.position;
                grabObj.transform.SetParent(gameObject.transform); //부모 컴포넌트 지정

                Rigidbody rb = grabObj.GetComponent<Rigidbody>();
                rb.useGravity = false;

                //Destroy(rb);

                imGrapping = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            detec=false;
        }
    }
}
