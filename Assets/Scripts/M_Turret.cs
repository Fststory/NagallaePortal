using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Video;

public class M_Turret : MonoBehaviour
{
    public GameObject turretbullet;
    public GameObject fireposition;
    public GameObject spotlight;
    public Collider playercol;

    public float warningTime = 1.0f;
    
    public bool warning;
    public bool shooting;
    public bool noshoot;
    
    public float shootTime = 0.0f;
    public float shootDelay = 1.5f;


    void Start()
    {
        
    }

    void Update()
    {
        if(transform.parent == null)
        {
            if (shooting)
            {
                if (shootTime > 0)
                {
                    shootTime -= Time.deltaTime;

                    if (shootDelay > 0)
                    {
                        shootDelay -= Time.deltaTime;
                    }
                    else
                    {
                        GameObject fir = Instantiate(turretbullet);
                        fir.transform.position = fireposition.transform.position;
                        fir.transform.rotation = fireposition.transform.rotation;

                        fir.GetComponent<BulletMove>().turret = gameObject;

                        shootDelay = 0.3f;
                    }

                }
                else
                {
                    shooting = false; //사격 종료
                    warning = false; //사격이 완전히 끝났으니 경고도 종료
                    warningTime = 1.0f; //사격이 완전히 끝났으니 경고시간 재설정
                }
            }
        }
        else
        {
            spotlight.SetActive(false);
            noshoot = true;
        }

        #region 총알 테스트 코드
        //총알 테스트 코드
        //if (Input.GetMouseButtonDown(0))
        //{
        //        GameObject fir = Instantiate(turretbullet);
        //        fir.transform.position = fireposition.transform.position;
        //        fir.transform.rotation = fireposition.transform.rotation;

        //        fir.GetComponent<BulletMove>().turret = gameObject;
        //}
        #endregion


    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other == playercol)
    //    {

    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        if (!noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                if (warningTime == 1.0f) //첫 경고인가?
                {
                    warning = true; //경고 시작
                    warningTime -= Time.deltaTime; //타이머 시작
                }
                else if (warningTime > 0) //경고 중인가?
                {
                    warningTime -= Time.deltaTime; //타이머 계속
                }
                else if (warningTime < 0) //경고시간이 지났다면
                {
                    shooting = true;
                    shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
                }

                //shooting = true;
                //shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                //사격 이전에 나갔을 때 발동될 명령어
                if (!shooting && warningTime < 1.0f)
                {
                    warningTime += Time.deltaTime;
                }
            }
        }
    }

}
