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


    #region ����� ����
    public AudioSource audioSourse;
    public AudioClip[] buttonSound;
    #endregion


    void Start()
    {
        audioSourse = transform.GetComponent<AudioSource>(); //����� ������Ʈ ĳ��
    }

    void Update()
    {
        if (!alreadySpawn) //���� �� ���� �������� �ʾ��� ���
        {
            if (playerHere && Input.GetKeyDown(KeyCode.E))
            {
                alreadySpawn = true;
                GameObject Cube = Instantiate(cube);
                Cube.transform.position = spawnPoint.transform.position;
                audioSourse.clip = buttonSound[0]; //����Ŭ��
                audioSourse.volume = 0.7f;//����Ŭ��
                audioSourse.Play();//����Ŭ��
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
