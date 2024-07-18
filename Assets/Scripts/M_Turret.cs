

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_Turret : MonoBehaviour
{

    //public GameObject turretbullet;  //Ray 방식으로 변경하며 삭제
    public GameObject fireposition;
    public GameObject spotlight;
    public GameObject Particle;
    public GameObject bulletHoleparticle;
    public Collider playercol;
    public LayerMask turretEye;

    public float warningTime = 1.0f;
    public float rotationSpeed = 3.0f;

    public float rotX;
    public float rotZ;

    public bool warning;
    public bool shooting;
    public bool noshoot;
    
    public float shootTime = 0.0f;
    public float shootDelay = 1.5f;

    //챗GPT
    public Transform laser; // 자식 레이저 Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 60f;

    void Start()
    {
        //파티클 설정
        Particle.SetActive(false);

        //챗 GPT
        if (playercol.gameObject != null)
        {
            playerTransform = playercol.gameObject.transform;
        }
        // 자식 레이저의 원래 회전 각도 저장
        originalRotation = laser.rotation;
    }

    void Update()
    {
        //터렛의 x축과 z축 각도를 계속 측정한다.
        rotX = gameObject.transform.eulerAngles.x; //오일러 앵글!!!!
        rotZ = gameObject.transform.eulerAngles.z;
        

        if ((Mathf.Abs(rotX) > 300 || Mathf.Abs(rotX) < 60) && (Mathf.Abs(rotZ) > 300 || Mathf.Abs(rotZ) < 60) ) //특정 각도 안일때... 활성화 상태로 친다.
            //세워진 기준 60도를 초과일 때. (즉 300도 이상이거나 60도 이하여야 한다... 그래서 이런 기괴한)
            //[프로토는 이거 씀] if (transform.parent == null) //부모 컴포넌트가 없을 때 (플레이어가 안 집으면) 활성화 상태로 친다.
        {
            if (shooting) // 사격 중인가?
            {
                Particle.SetActive(true); //사격할거면 파티클 on
                if (shootTime > 0) //0.3초마다 총알 하나씩 사격하도록 하는 중
                {
                    shootTime -= Time.deltaTime;

                    if (shootDelay > 0)
                    {
                        shootDelay -= Time.deltaTime;
                    }
                    else
                    {
                        #region 총알 소환 -레거시-
                        //GameObject fir = Instantiate(turretbullet);
                        //fir.transform.position = fireposition.transform.position;
                        //fir.transform.rotation = fireposition.transform.rotation;

                        //fir.GetComponent<M_BulletMove>().turret = gameObject;
                        #endregion

                        ShootingTurret();
                        shootDelay = 0.3f; //0.3초마다 레이를 처쏴.
                    }

                }
                else
                {
                    shooting = false; //사격 종료
                    Particle.SetActive(false); //파티클 종료
                    warning = false; //사격이 완전히 끝났으니 경고도 종료
                    warningTime = 1.0f; //사격이 완전히 끝났으니 경고시간 재설정
                }
            }
        }
        else //특정 각도 밖일때.
        {
            spotlight.SetActive(false);
            noshoot = true;
        }

        //레이저 따라가게 하기
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

    #region 터렛 콜라이더 분리-해당 코드 TurretCollider에 있음
    //private void OnTriggerStay(Collider other)
    //{
    //    if (!noshoot)
    //    {
    //        if (other.gameObject.name == "Player")
    //        {
    //            if (warningTime == 1.0f) //첫 경고인가?
    //            {
    //                warning = true; //경고 시작
    //                warningTime -= Time.deltaTime; //타이머 시작
    //            }
    //            else if (warningTime > 0) //경고 중인가?
    //            {
    //                warningTime -= Time.deltaTime; //타이머 계속
    //            }
    //            else if (warningTime < 0) //경고시간이 지났다면
    //            {
    //                shooting = true;
    //                shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
    //            }

    //            //shooting = true;
    //            //shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!noshoot)
    //    {
    //        if (other.gameObject.name == "Player")
    //        {
    //            //사격 이전에 나갔을 때 발동될 명령어
    //            if (!shooting && warningTime < 1.0f)
    //            {
    //                warningTime += Time.deltaTime;
    //            }
    //        }
    //    }
    //}
    #endregion

    void ShootingTurret()
    {
        //레이캐스트 방식
        RaycastHit hit;
        Vector3 rayOrigin = fireposition.transform.position;
        Vector3 rayDirection = fireposition.transform.forward; //기본 레이 방향

        //X축 각도에 랜덤값을 부여해서 총알이 흩뿌려지는 연출.
        float randomx = Random.Range(-6f, 6f); //랜덤값 변수
        float randomy = Random.Range(-2f, 2f);
        Quaternion randomRot = Quaternion.Euler(randomx, randomy, 0f); //랜덤값 각도
        Vector3 randomRayLego = randomRot * rayDirection; //곱해서 적용

        print("랜덤 값! =" + randomRayLego);

        Debug.DrawRay(rayOrigin, randomRayLego, Color.red, 5.0f); //디버그용 레이 궤도

        if (Physics.Raycast(rayOrigin, randomRayLego, out hit, 50f, ~turretEye))
        {
            Debug.Log("turret hit: " + hit.collider.gameObject.name); // 디버그 메시지

            if (hit.collider != null && hit.collider.gameObject.name == "Player")
            {
                GameManager.gm.AddDamage(1); //처맞기
            }
            //else if (hit.collider.gameObject.tag == "LabObject")
            //{
            //    //사운드 다르게 할거라서 분리 //레이 오류로 파티클 확인하려고 잠시 비활성화해둠
            //}
            else if (hit.collider.gameObject.name.Contains("Window"))
            {
                //총알 파티클 넣기(불렛홀이 남는 걸로)
                GameObject hole = Instantiate(bulletHoleparticle); //인스턴스화

                //레이 맞은 위치에 넣기
                hole.transform.position = hit.point;
                hole.transform.rotation = hit.collider.transform.rotation;

                //사운드 분리할 예정
            }
            else
            {
                //총알 파티클 넣기(불렛홀이 남는 걸로)
                GameObject hole = Instantiate(bulletHoleparticle); //인스턴스화

                //레이 맞은 위치에 넣기
                hole.transform.position = hit.point;

                //쿼터니언 뒤집기 변수 선언
                Quaternion yRotation = Quaternion.Euler(0, 180, 0);

                //180도 뒤집어서 넣기
                hole.transform.rotation = hit.collider.transform.rotation * yRotation;

                Vector3 normal = hit.normal;

                //사운드 분리할 예정
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
        laser.rotation = Quaternion.Slerp(laser.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
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




