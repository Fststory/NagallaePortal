using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : MonoBehaviour, IInteractable
{
    public GameObject hand;

    private void Update()
    {
        Interact();
    }

    public void Interact()
    {
        //Debug.Log(Random.Range(0, 100));
        Vector3 offset = hand.transform.forward * 5;
        transform.position = hand.transform.position + offset;
    }
}