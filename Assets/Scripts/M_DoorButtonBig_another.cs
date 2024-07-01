using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorButtonBig_another : MonoBehaviour
{
    public GameObject Button;

    public Transform ButtonTarget;

    public float Speed = 0.3f;


    public bool activate = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (activate)
        {
            Button.transform.position = Vector3.MoveTowards(transform.position, ButtonTarget.position, Speed * Time.deltaTime);
        }
    }
}
