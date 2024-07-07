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
        if(Input.GetKeyDown(KeyCode.F4))            // F4 누르면...
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
    }

    //(7/4) 만들고 보니 아직 이걸 만들 필요가 없어서 기본적인 뼈대만 세워두고 방치중!

    void ScenePortal(int num)      // 씬 이동 함수
    {
        // 현재 씬 인덱스를 얻은 뒤
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6을 통해 해당 씬 불러오기
        SceneManager.LoadScene(currentSceneindex + num);

        // 커서 원래대로 (해당 씬에서 시작할 때 커서 안보이게 설정돼 있으면 그대로 반영됨)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // 마우스 감도 조절 함수(미구현)
    {

    }
}
