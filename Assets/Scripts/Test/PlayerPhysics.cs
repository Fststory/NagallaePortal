using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // 플레이어의 물리 상태 체크(?)를 도와줄 리지드바디와 관련된 스크립트입니다.

    Rigidbody rb;   // 리지드바디를 캐싱할 변수
    public bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();     // 리지드바디 캐싱
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
