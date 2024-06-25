using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5;     // 속도는 임시 지정값임.

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
        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

    }
}
