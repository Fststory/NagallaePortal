using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorButtonSmall : MonoBehaviour
{
    //플레이어는 버튼의 붉은 부분을 누른다

    //버튼이 쏙 들어가고 원상복구한다.
    //문이 열린다.

    //시간이 지나면 문이 닫힌다.

    #region 6/26 메모
    // 마우스를 클릭했을 때 상호작용하는 것부터 시작하자
    // 버튼 = true false bool형을 잘 이용해야 함
    // 시간이 지나면 문이 닫혀야 함
    // 버튼이 쏙 들락이기:: ButtonPush가 아래로 갔다가 위로 움직임

    #endregion

    public GameObject pressButton;
    public GameObject openDoorLeft;
    public GameObject openDoorRight;

    public float pressSpeed = 1f; //버튼 연출 속도
    public float openSpeed = 5f; //문 연출 속도

    public float countOpenTime; 
    public float countCloseTime = 10.0f;

    public float buttonMoveTime=0.0f;
    public float buttonMoveEndTime=0.5f;


    bool isDoorOpen = false; //버튼 활성화 여부 변수

    public bool isMoving = false; //버튼이 내려가고 문이 열림
    public bool isMovingAgain = false; //버튼이 올라가고 문이 닫힘



    void Start()
    {
        countOpenTime = 0; //타이머 리셋해 두기
        buttonMoveTime = 0;
    }

    void Update()
    {
        // 6.26
        // 클릭하면 버튼을 아래로 내리기
        // true false 명령어를 이용해서 print 문이 열린다 / 문이 닫힌다부터

        if (Input.GetMouseButtonDown(0))    //만약 플레이어가 클릭한다면
        {
            //isDoorOpen = !isDoorOpen; //이건 누를때마다 토글
            isDoorOpen = true; //길이 활성화.
            isMoving = true; //버튼이랑 문이 움직이는 연출
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

    }

}
