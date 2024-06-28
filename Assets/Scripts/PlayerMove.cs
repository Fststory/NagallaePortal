using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Player의 이동 속도 변수
    public float moveSpeed = 5;

    // 중력 변수
    float gravity = -1f;
    // 수직 속력 변수
    float yVelocity = 0;


    void Start()
    {
        
    }

    void Update()
    {
        // 플레이어의 키 입력에 따라 움직임(전후좌우)을 구현
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // 메인 카메라를 기준으로 방향을 변환한다. (로컬 방향 벡터)
        dir = Camera.main.transform.TransformDirection(dir);


        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
    }
}
