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
    public float rotationSpeed = 3.0f;

    public bool warning;
    public bool shooting;
    public bool noshoot;
    
    public float shootTime = 0.0f;
    public float shootDelay = 1.5f;

    //챗GPT
    public Transform laser; // 자식 레이저 Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 90f;

    void Start()
    {
        //챗 GPT
        if (playercol.gameObject != null)
        {
            playerTransform = playercol.gameObject.transform;
        }
        // 자식 레이저의 원래 회전 각도 저장
        originalRotation = laser.localRotation;
    }

    void Update()
    {
        
        if (transform.parent == null) //부모 컴포넌트가 없을 때 (플레이어가 안 집으면) 활성화 상태로 친다.
        {
            if (shooting) // 사격 중인가?
            {
                if (shootTime > 0) //0.3초마다 총알 하나씩 사격하도록 하는 중
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

                        fir.GetComponent<M_BulletMove>().turret = gameObject;

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
        else //플레이어가 집어버린 순간
        {
            spotlight.SetActive(false);
            noshoot = true;
        }


        //레이저 따라가게 하기
        //lerp를 쓰는게 맞을 것 같다.
        //근데 일단 챗 GPT좀

        if (warning && playerTransform != null) //경고 중일때
        {
            LaserGotoPlayer();
        }
        else
        {
            RotateLaserToOriginal();
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

    private void LaserGotoPlayer()
    {
        // 플레이어 방향으로의 벡터 계산
        Vector3 directionToPlayer = playerTransform.position - laser.position;
        directionToPlayer.y = 0; // y축 고정을 위해 y값을 0으로 설정

        // 목표 회전 각도 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // 각도 제한 적용
        Quaternion clampedRotation = ClampRotation(targetRotation);

        // 현재 회전에서 목표 회전으로 점진적으로 회전
        laser.rotation = Quaternion.Slerp(laser.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateLaserToOriginal()
    {
        // 원래 회전 각도로 돌아가도록 설정
        Quaternion clampedRotation = ClampRotation(originalRotation);

        // 현재 회전에서 원래 회전으로 점진적으로 회전
        laser.localRotation = Quaternion.Slerp(laser.localRotation, clampedRotation, rotationSpeed * Time.deltaTime);
    }

    Quaternion ClampRotation(Quaternion targetRotation)
    {
        // 원래 회전 각도와 목표 회전 각도 간의 각도 차이 계산
        float angleDifference = Quaternion.Angle(originalRotation, targetRotation);

        if (angleDifference > maxRotationAngle)
        {
            // 각도 차이가 최대 회전 각도를 초과하면, 최대 회전 각도로 제한
            targetRotation = Quaternion.RotateTowards(originalRotation, targetRotation, maxRotationAngle);
        }
        return targetRotation;
    }
}




