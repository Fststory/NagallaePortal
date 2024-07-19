using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CamFollow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
