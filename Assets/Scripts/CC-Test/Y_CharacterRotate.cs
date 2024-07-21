using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CharacterRotate : MonoBehaviour
{
    private const float cameraSpeed = 3.0f;
    public Quaternion TargetRotation { private set; get; }

    private void Awake()
    {
        TargetRotation = transform.rotation;            // 카메라의 현재 회전 쿼터니언을 변수(TargetRotation)에 저장
    }
    private void LateUpdate()
    {
        var targetEuler = TargetRotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0) * cameraSpeed;     // 마우스 이동 계산 & 적용 => 현재 바라보는 방향. (Vector3)
        TargetRotation = Quaternion.Euler(targetEuler);         // (현재 바라보는 방향)벡터3 => 쿼터니언으로 변환

        // 카메라가 마우스 입력에 따라 이동하는 것을 실행하는 부분
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
    }
}
