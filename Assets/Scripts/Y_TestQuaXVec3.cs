using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_TestQuaXVec3 : MonoBehaviour
{
    // 쿼터니안과 벡터3의 곱을 눈으로 파악하기 위한 스크립트

    public Vector3 initialVector = new Vector3(1, 0, 0); // 초기 벡터
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // 회전 축
    public float rotationAngle = 45; // 회전 각도

    private Vector3 rotatedVector; // 회전된 벡터

    void Start()
    {
        // 초기 벡터를 큐브의 위치로 설정
        transform.position = initialVector;
    }

    void Update()
    {
        // 사용자가 'R' 키를 누르면 회전 계산
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 회전 축과 각도로 쿼터니언 생성
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);

            // 쿼터니언과 벡터의 곱셈으로 벡터 회전
            rotatedVector = rotation * initialVector;

            // 회전된 벡터를 큐브의 새로운 위치로 설정
            transform.position = rotatedVector;

            Debug.Log("Rotated Vector: " + rotatedVector);
        }
    }

    void OnDrawGizmos()
    {
        // 초기 벡터와 회전된 벡터를 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, initialVector);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, rotatedVector);
    }
}
