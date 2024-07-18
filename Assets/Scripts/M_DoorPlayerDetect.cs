using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorPlayerDetect : MonoBehaviour
{
    public GameObject elevator;

    M_Elevator m_Elevator;

    void Start()
    {
        m_Elevator = transform.parent.GetComponent<M_Elevator>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        m_Elevator.playerGoal = true;
    }

}
