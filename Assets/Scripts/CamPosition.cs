using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
    // 카메라를 둘 목표 GameObject
    public GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = target.transform.position;
    }
}
