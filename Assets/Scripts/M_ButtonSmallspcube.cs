using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ButtonSmallspcube : MonoBehaviour
{
    public GameObject cube;
    public GameObject buttonPart;
    public Transform spawnPoint;

    public Transform buttonActive;

    public bool playerHere;
    public bool alreadySpawn;

    public bool isMoving;

    public float spd = 3.0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!alreadySpawn) //아직 한 번도 스폰되지 않았을 경우
        {
            if (playerHere && Input.GetKeyDown(KeyCode.E))
            {
                alreadySpawn = true;
                GameObject Cube = Instantiate(cube);
                Cube.transform.position = spawnPoint.transform.position;
            }
        }
        else
        {
                ButtonDOWN();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHere = true;
        }
    }

    void ButtonDOWN()
    {
        buttonPart.transform.position = Vector3.Lerp(buttonPart.transform.position, buttonActive.position, spd * Time.deltaTime);
    }

}
