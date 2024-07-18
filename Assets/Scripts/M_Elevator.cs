using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Elevator : MonoBehaviour
{
    //public Camera cam;
    Vector3 ElevatorOriginalPos;
    //카메라 말고 엘레베이터를 흔들자

    public Animator AnimController; //애니메이터 제어
    public GameObject frontCollider; //문짝 콜라이더

    bool alreadyDoit = false;

    public bool letsgoNext = false;

    float spd = 0.7f;
    public float countShakeTime = 1.0f;
    public float countOpentime = 1.5f;

    float nextlevel = 4.0f;

    public bool playerGoal = false; //플레이어 탐지 영역 콜라이더에서 스크립트 넣어서 제어하기

    public float waitForplayer = 1.0f;


    void Start()
    {
        //cam = Camera.main;
        //cameraOriginalPos = cam.transform.position;

        ElevatorOriginalPos = gameObject.transform.position; //원래 자리 저장하기
        print(ElevatorOriginalPos);

        AnimController.enabled = false;
    
    }

    void Update()
    {
        #region 시작할때 엘레베이터 열기
        if (!alreadyDoit && countOpentime > 0)
        {
            countOpentime -= Time.deltaTime;
        }
        else if (!alreadyDoit && countOpentime < 0)
        {
            StartCoroutine(CameraShake(0.3f, 0.3f)); //흔들면서
            AnimController.enabled = true; //문열어주고
            frontCollider.SetActive(false); //콜라이더 제거
            alreadyDoit = true;
        }
        #endregion

        if (playerGoal)
        {
            if (waitForplayer > 0)
            {
                waitForplayer -= Time.deltaTime; //딱 1초 기다려준다.
            }
            else
            {
                frontCollider.SetActive(true); //이제 못지나간다.
                StartCoroutine(CameraShake(0.3f, 0.3f)); //흔들면서
                AnimController.SetTrigger("PlayerGoal");
                letsgoNext = true;
                playerGoal = false;
            }
        }

        if (letsgoNext)
        {
            nextlevel -= Time.deltaTime;
            if (nextlevel < 0)
            {
                print("다음 씬!");
                //이제 게임매니저에서 다음 씬 불러오기.
                letsgoNext = false;
            }

        }

    }

    //void OpenTheDoor()
    //{
    //    RightDoor.transform.eulerAngles = Vector3.Lerp(RightDoor.transform.eulerAngles, RightDoorOpen.transform.eulerAngles, spd * Time.deltaTime);
    //    LeftDoor.transform.eulerAngles = Vector3.Lerp(LeftDoor.transform.eulerAngles, LeftDoorOpen.transform.eulerAngles, spd * Time.deltaTime);
    //}


    IEnumerator CameraShake(float duration, float magnitude) //카메라 흔들리게 하는 코루틴, float 작동시간, float 작동범위
    {
        float timer = 0;

        while (timer <= duration)
        {
            gameObject.transform.position = Random.insideUnitSphere * magnitude + ElevatorOriginalPos;
            timer += Time.deltaTime; //시간 흐르기
            yield return null;
        }

        gameObject.transform.position = ElevatorOriginalPos; //카메라 원위치
    }

}
