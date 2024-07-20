using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorButtonBig_another : MonoBehaviour
{
    public bool isdooropen;

    //Lerp를 사용해 보자.

    //문 오브젝트
    public GameObject DoorLeft;
    public GameObject DoorRight;

    //Door Lerp 
    public Transform leftOpen;
    public Transform rightOpen;
    public Transform leftClose;
    public Transform rightClose;

    //버튼 오브젝트
    public GameObject buttonPart;

    //button Lerp
    public Transform buttonDeactive;
    public Transform buttonActive;

    public float spd = 3.0f;

    #region 가이드라인 머태리얼 변수

    public GameObject guideline;

    public Material activate;
    public Material deactivate;

    private Renderer lineRenderer;

    #endregion

    #region 오디오 변수
    public AudioSource audioSourse;
    public AudioClip[] buttonSounds;
    #endregion

    bool activateSound = true;

    bool deactiveSound = false;


    void Start()
    {
        audioSourse = transform.GetComponent<AudioSource>(); //오디오 컴포넌트 캐싱
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

        if (isdooropen)
        {
            ButtonDOWN(); //버튼 움직임
            Open(); //문 열림
            
            if (activateSound) //사운드가 한 번도 안 켜졌다면
            {
                ButActivate();//사운드 켜고
                activateSound = false; //켰다고 표시하고
                deactiveSound = true; //이렇게 해서 꺼짐 사운드를 대비하기
            }
        }
        else
        {
            ButtonUP(); //버튼 움직임
            Close(); //문 열림

            if (deactiveSound)
            {
                ButDeactivate();
                deactiveSound=false;
                activateSound = true;
            }
        }

        UpdateMaterial(); //머태리얼 갈기
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
           isdooropen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            isdooropen = false;
        }
    }

    private void UpdateMaterial() //머태리얼 바꿔주는 함수 (출처: GPT)
    {
        if (lineRenderer != null)
        {
            if (isdooropen)
            {
                lineRenderer.material = activate;
            }
            else
            {
                lineRenderer.material = deactivate;
            }
        }
    }

    void ButtonUP()
    {
        buttonPart.transform.position = Vector3.Lerp(buttonPart.transform.position, buttonActive.position, spd * Time.deltaTime);
    }
    void ButtonDOWN()
    {
        buttonPart.transform.position = Vector3.Lerp(buttonPart.transform.position, buttonDeactive.position, spd * Time.deltaTime);
    }
    void Close()
    {
        
        DoorLeft.transform.position = Vector3.Lerp(DoorLeft.transform.position, leftClose.position, spd*Time.deltaTime);
        DoorRight.transform.position = Vector3.Lerp(DoorRight.transform.position, rightClose.position, spd * Time.deltaTime);
    }
    void Open()
    {
        DoorLeft.transform.position = Vector3.Lerp(DoorLeft.transform.position, leftOpen.position, spd * Time.deltaTime);
        DoorRight.transform.position = Vector3.Lerp(DoorRight.transform.position, rightOpen.position, spd * Time.deltaTime);
    }

    public void ButActivate()
    {
        audioSourse.clip = buttonSounds[0];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }

    public void ButDeactivate()
    {
        audioSourse.clip = buttonSounds[1];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }


}
