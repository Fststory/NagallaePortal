using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F4))            // F4 ������...
        {
            ScenePortal(-1);                        // ���� �� ��ŸƮ
        }
        if(Input.GetKeyDown(KeyCode.F5))            // F5 ������...
        {
            ScenePortal(0);                         // ���� �� ��ŸƮ
        }
        else if(Input.GetKeyDown(KeyCode.F6))       // F6 ������...
        {
            ScenePortal(1);                         // ���� �� ��ŸƮ
        }
    }

    //(7/4) ����� ���� ���� �̰� ���� �ʿ䰡 ��� �⺻���� ���븸 �����ΰ� ��ġ��!

    void ScenePortal(int num)      // �� �̵� �Լ�
    {
        // ���� �� �ε����� ���� ��
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6�� ���� �ش� �� �ҷ�����
        SceneManager.LoadScene(currentSceneindex + num);

        // Ŀ�� ������� (�ش� ������ ������ �� Ŀ�� �Ⱥ��̰� ������ ������ �״�� �ݿ���)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // ���콺 ���� ���� �Լ�(�̱���)
    {

    }
}
