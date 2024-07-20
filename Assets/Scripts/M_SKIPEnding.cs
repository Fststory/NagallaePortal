using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_SKIPEnding : MonoBehaviour
{
    public GameObject imeage;
    float time = 2f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene(0);

        }

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            imeage.SetActive(false);
        }

    }
}
