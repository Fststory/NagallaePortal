using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // wasd 입력을 토대로 방향 벡터를 지정
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, z);
        // 방향 벡터를 플레이어의 회전을 기준으로 정렬
        dir = transform.TransformDirection(dir);
        // p = p0 + vt;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
