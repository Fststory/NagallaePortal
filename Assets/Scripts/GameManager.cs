using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public M_Lab16UI m_Lab16UI; //16번랩 함수 받아오기 위해서 호출
    public GameObject gameoverUI;

    public int playerHP = 10; //총알 10대 맞으면 사망
    int turretHP = 5; //총알 5대 맞으면 비활성화
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
        if (Input.GetKeyDown(KeyCode.F4))            // F4 누르면...
        {
            ScenePortal(-1);                        // 이전 씬 스타트
        }
        if(Input.GetKeyDown(KeyCode.F5))            // F5 누르면...
        {
            ScenePortal(0);                         // 현재 씬 스타트
        }
        else if(Input.GetKeyDown(KeyCode.F6))       // F6 누르면...
        {
            ScenePortal(1);                         // 다음 씬 스타트
        }

        //플레이어의 HP가 0이 되면 게임오버 UI

        if (playerHP <= 0)
        {
            ShowGameOverUI();
        }

        //게임 오버 상태에서 좌클릭했을때 현재 씬을 다시 시작하기
        if (isitOver && Input.GetMouseButtonDown(0))
        {
            isitOver = false;
            RestartGame();
        }
    }

    

    void ScenePortal(int num)      // 씬 이동 기능 구현!
    {
        // 현재 씬 인덱스(순서)를 확인한 뒤
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6을 통해 해당 씬 불러오기
        // F4(이전), F5(현재), F6(다음)
        SceneManager.LoadScene(currentSceneindex + num);

        // 커서 원상복구 (만약 해당 씬에서 시작할 때 커서 안보이게 설정돼 있으면 그대로 반영됨)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // 마우스 감도 조절 함수(미구현)
    {

    }

    public void AddDamage(int hit) //처맞을때마다 피 까이고 눈앞이 빨개지기
    {
        playerHP -= hit;
        StartCoroutine(m_Lab16UI.FadeOut());
    }

    public void ShowGameOverUI() //게임 오버!
    {
        gameoverUI.SetActive(true);
        isitOver = true;
        Time.timeScale = 0.0f;
    }

    public void RestartGame()
    {
        //현재 씬을 다시 시작한다.
        Time.timeScale = 1f;
        SceneManager.LoadScene("LAB16");
    }

}
