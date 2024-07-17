using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CamRotate : MonoBehaviour
{
    #region 기존(폐기)
    //public float rotSpeed = 5;

    //float mx = 0;
    //float my = 0;

    //void Start()
    //{
    //    // 마우스 커서가 화면 밖으로 나가지 않도록 중앙에 고정한다.(마우스 커서의 위치에 구애받지 않고 플레이 가능!)
    //    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    //}

    //void Update()
    //{
    //    // 플레이어의 마우스 입력에 따라 시점 변화(상하좌우)를 구현

    //    // 마우스 입력을 받는다.
    //    float mouseX = Input.GetAxis("Mouse X");
    //    float mouseY = Input.GetAxis("Mouse Y");

    //    // 회전 변수에 값을 누적시킨다.
    //    mx += mouseX * rotSpeed * Time.deltaTime;
    //    my += mouseY * rotSpeed * Time.deltaTime;

    //    // 상하 회전값을 제한한다.
    //    my = Mathf.Clamp(my, -90f, 90f);

    //    // 회전방향으로 시점을 회전시킨다.
    //    transform.eulerAngles = new Vector3(-my, mx, 0);

    //}
    #endregion

    private const float cameraSpeed = 3.0f;

    public Quaternion TargetRotation { private set; get; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;       // 커서 화면 중앙 고정

        TargetRotation = transform.rotation;            // 카메라의 현재 회전 쿼터니언을 변수(TargetRotation)에 저장
    }

    private void LateUpdate()
    {
        // 카메라 회전 [마우스 이동]
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));    // 마우스 x,y축 이동(상하좌우)을 값으로 담는다.(Vector2)
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;     // 마우스 이동 계산 & 적용 => 현재 바라보는 방향. (Vector3)
                                                                                            // v = v0 + delta.v
        // 상하 시야각 제한 
        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -75.0f, 75.0f);
        TargetRotation = Quaternion.Euler(targetEuler);         // (현재 바라보는 방향)벡터3 => 쿼터니언으로 변환

        // 카메라가 마우스 입력에 따라 이동하는 것을 실행하는 부분
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);
    }

    // 오뚝이 기능!!! (플레이어 오브젝트에서 실행돼야 함, 다른 물체[터렛,우정박스 등]와 플레이어가 구분되는 요소)
    // 포탈 이동으로 변형된 시점의 회전을 다시 정상으로 돌리는 것 (항상 나의 위 방향이 월드 좌표에서 정해진 위의 방향이 되게끔)
    // (땅을 밟고 있을 땐 xz평면에 평행한 방향을 정면으로 바라보지만
    // 포탈에서 나올 때 yz평면을 발 밑에 둔다면 yz평면에 평행한 방향을 정면으로 바라보게 된다. 이때 다시 처음의 상태로 돌아가는 것)
    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
