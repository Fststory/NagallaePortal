using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
    // ī�޶� �� ��ǥ GameObject
    public GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = target.transform.position;
    }
}
