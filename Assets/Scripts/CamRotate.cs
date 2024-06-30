using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CamRotate : MonoBehaviour
{
    public float rotSpeed = 5;

    float mx = 0;
    float my = 0;

    void Start()
    {
        // 마우스 커서가 화면 밖으로 나가지 않도록 중앙에 고정한다.(마우스 커서의 위치에 구애받지 않고 플레이 가능!)
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 플레이어의 마우스 입력에 따라 시점 변화(상하좌우)를 구현
        
        // 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 회전 변수에 값을 누적시킨다.
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        // 상하 회전값을 제한한다.
        my = Mathf.Clamp(my, -90f, 90f);

        // 회전방향으로 시점을 회전시킨다.
        transform.eulerAngles = new Vector3(-my, mx, 0);

    }
}
