using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GladosTalk : MonoBehaviour
{
    public AudioSource glados;

    bool talked = false;


    void Start()
    {
        glados = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!talked && other.gameObject.name == "Player")
        {
            glados.Play();
            glados.volume = 0.7f;
            talked = true;
        }
    }

}
