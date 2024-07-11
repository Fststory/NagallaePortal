using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Autodoor : MonoBehaviour
{
    //문 오브젝트
    public GameObject DoorLeft;
    public GameObject DoorRight;

    //Door Lerp 
    public Transform leftOpen;
    public Transform rightOpen;
    public Transform leftClose;
    public Transform rightClose;

    float spd = 2.0f;

    public float delay = 2.0f;


    void Start()
    {
        
    }

    void Update()
    {
        delay -= Time.deltaTime;
        if (delay < 0)
        {
            Open();
        }
    }

    void Open()
    {
        DoorLeft.transform.position = Vector3.Lerp(DoorLeft.transform.position, leftOpen.position, spd * Time.deltaTime);
        DoorRight.transform.position = Vector3.Lerp(DoorRight.transform.position, rightOpen.position, spd * Time.deltaTime);
    }

}
