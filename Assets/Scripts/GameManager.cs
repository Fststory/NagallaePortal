using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public M_Lab16UI m_Lab16UI; //16���� �Լ� �޾ƿ��� ���ؼ� ȣ��
    public GameObject gameoverUI;

    public int playerHP = 10; //�Ѿ� 10�� ������ ���
    int turretHP = 5; //�Ѿ� 5�� ������ ��Ȱ��ȭ
    public bool isitOver = false;

    private void Awake()
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
    void Start()
    {
        gameoverUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))            // F4 ������...
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

        //�÷��̾��� HP�� 0�� �Ǹ� ���ӿ��� UI

        if (playerHP <= 0)
        {
            ShowGameOverUI();
        }

        //���� ���� ���¿��� ��Ŭ�������� ���� ���� �ٽ� �����ϱ�
        if (isitOver && Input.GetMouseButtonDown(0))
        {
            isitOver = false;
            RestartGame();
        }
    }

    

    void ScenePortal(int num)      // �� �̵� ��� ����!
    {
        // ���� �� �ε���(����)�� Ȯ���� ��
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6�� ���� �ش� �� �ҷ�����
        // F4(����), F5(����), F6(����)
        SceneManager.LoadScene(currentSceneindex + num);

        // Ŀ�� ���󺹱� (���� �ش� ������ ������ �� Ŀ�� �Ⱥ��̰� ������ ������ �״�� �ݿ���)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // ���콺 ���� ���� �Լ�(�̱���)
    {

    }

    public void AddDamage(int hit) //ó���������� �� ���̰� ������ ��������
    {
        playerHP -= hit;
        StartCoroutine(m_Lab16UI.FadeOut());
    }

    public void ShowGameOverUI() //���� ����!
    {
        gameoverUI.SetActive(true);
        isitOver = true;
        Time.timeScale = 0.0f;
    }

    public void RestartGame()
    {
        //���� ���� �ٽ� �����Ѵ�.
        Time.timeScale = 1f;
        SceneManager.LoadScene("LAB16");
    }

}
