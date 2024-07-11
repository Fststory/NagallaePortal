using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{
    private const float moveSpeed = 7.5f;
    private const float cameraSpeed = 3.0f;

    public Quaternion TargetRotation { private set; get; }
    
    private Vector3 moveVector;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();          // 리지드바디 컴포넌트를 변수에 저장
        Cursor.lockState = CursorLockMode.Locked;       // 커서 화면 중앙 고정

        TargetRotation = transform.rotation;            // 카메라의 현재 회전 정보를 변수에 저장
    }

    private void Update()
    {
        // 카메라 회전 [마우스 이동]
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));    // 마우스 x,y축 이동(상하좌우)을 값으로 담는다.(Vector2)
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;     // 마우스 이동이 계산 & 적용된 바라보는 방향. (Vector3)
        if(targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 
            Time.deltaTime * 15.0f);

        // 카메라 움직이기 [w,a,s,d]
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0.0f, z);
        dir.Normalize();
        moveVector = dir * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = transform.TransformDirection(moveVector);
        rigidbody.velocity = newVelocity;
    }


    // 오뚜기 기능!!!
    // 포탈 이동으로 변형된 시점의 회전을 다시 정상으로 돌리는 것 (항상 나의 위 방향이 월드 좌표에서 정해진 위의 방향이 되게끔)
    // (땅을 밟고 있을 땐 xz평면에 평행한 방향을 정면으로 바라보지만
    // 포탈에서 나올 때 yz평면을 발 밑에 둔다면 yz평면에 평행한 방향을 정면으로 바라보게 된다. 이때 다시 처음의 상태로 돌아가는 것)
    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
