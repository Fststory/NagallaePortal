using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class M_DoorButtonSmall : MonoBehaviour
{

    #region 6/26 메모
    // 마우스를 클릭했을 때 상호작용하는 것부터 시작하자
    // 버튼 = true false bool형을 잘 이용해야 함
    // 시간이 지나면 문이 닫혀야 함
    // 버튼이 쏙 들락이기:: ButtonPush가 아래로 갔다가 위로 움직임

    #endregion

    #region 6/28 메모

    //함수화하니까 고장나서 그대로두기로했더염

    //어라...? 가이드라인... 코드를 따로 쓸 필요 있나...?
    //여따가... 처넣으면 안되나?
    // 처넣으니 되더라(7/1)

    #endregion

    public GameObject pressButton;
    public GameObject openDoorLeft;
    public GameObject openDoorRight;

    float pressSpeed = 1f; //버튼 연출 속도
    float openSpeed = 8f; //문 연출 속도

    float countOpenTime; 
    public float countCloseTime = 10.0f;

    #region 버튼 변수
    float buttonMoveTime =0.0f;
    float buttonMoveEndTime=0.2f;

    public bool isDoorOpen = false; //버튼 활성화 여부 변수

    bool isMoving = false; //버튼이 내려가고 문이 열림
    bool isMovingAgain = false; //버튼이 올라가고 문이 닫힘
    #endregion

    #region 가이드라인 머태리얼 변수

    public GameObject guideline;

    public Material activate;
    public Material deactivate;

    private Renderer lineRenderer;

    #endregion

    public bool playerHere;


    void Start()
    {
        #region 타이머리셋
        countOpenTime = 0; //타이머 리셋해 두기
        buttonMoveTime = 0;
        #endregion

        #region 가이드라인 오브젝트의 Renderer 컴포넌트 가져오기

        if (guideline != null)
        {
            lineRenderer = guideline.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("guideline 설정되지 않았습니다.");
        }

        // 초기 Material 설정
        UpdateMaterial();
        #endregion
    }

    void Update()
    {
        #region 버튼 기본 작동
        // 6.26
        // 클릭하면 버튼을 아래로 내리기
        // true false 명령어를 이용해서 print 문이 열린다 / 문이 닫힌다부터

        #region 7/3 레이 발사 방법 (폐기)
        //if (Input.GetMouseButtonDown(0))    //만약 플레이어가 클릭한다면
        //{
        //    //레이를 카메라 방향으로 발사
        //    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        //    RaycastHit push = new RaycastHit();

        //    if (Physics.Raycast(ray, out push))
        //    {
        //        if (push.collider != null && push.collider.CompareTag("smallButton"))
        //        {
        //            //isDoorOpen = !isDoorOpen; //이건 누를때마다 토글
        //            isDoorOpen = true; //길이 활성화.
        //            if (countOpenTime == 0 && buttonMoveTime == 0)
        //            {
        //                isMoving = true; //버튼이랑 문이 움직이는 연출
        //            }
        //        }

        //    }           
        //}
        #endregion

        if (playerHere && Input.GetMouseButtonDown(0))
        {
            isDoorOpen = true; //길이 활성화.
            if (countOpenTime == 0 && buttonMoveTime == 0)
            {
                isMoving = true; //버튼이랑 문이 움직이는 연출
            }
        }
        
        if (isDoorOpen) //만약 문이 열린다면
        {
            countOpenTime += Time.deltaTime;
            if (countOpenTime > countCloseTime) //시간이 지나서
            {
                isDoorOpen = false; //통로 막기
                isMovingAgain = true; //문 닫히는 연출
                countOpenTime = 0; //타임리셋 또까먹었지!!!!!!!!!
            }
        }

        if (isMoving) //버튼이랑 문이 내려가고 있다면
        {
            //버튼이 아래로... 버튼의 로컬 방향으로 하는 게 좋겠다
            Vector3 pressing = transform.up * -1;
            pressButton.transform.position += pressing* pressSpeed * Time.deltaTime;
            //왼쪽문이 왼쪽으로...
            Vector3 sliding = transform.right * -1;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            //오른문이 오른쪽으로...
            Vector3 slidding = transform.right;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isMoving = false;
            }
        }

        if (isMovingAgain)//버튼이 올라가고 있다면
        {
            Vector3 pressDown = transform.up;
            pressButton.transform.position += pressDown * pressSpeed * Time.deltaTime;

            //왼쪽문이 오른쪽으로...
            Vector3 sliding = transform.right;
            openDoorLeft.transform.position += sliding * openSpeed * Time.deltaTime;
            //오른문이 왼쪽으로...
            Vector3 slidding = transform.right * -1;
            openDoorRight.transform.position += slidding * openSpeed * Time.deltaTime;

            buttonMoveTime += Time.deltaTime;
            if (buttonMoveTime > buttonMoveEndTime)
            {
                buttonMoveTime = 0;
                isMovingAgain = false;
            }
        }
        #endregion

        UpdateMaterial();

    }

    private void UpdateMaterial() //머태리얼 바꿔주는 함수 (출처: GPT)
    {
        if (lineRenderer != null)
        {
            if (isDoorOpen)
            {
                lineRenderer.material = activate;
            }
            else
            {
                lineRenderer.material = deactivate;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHere = true;
        }
    }

    

}
