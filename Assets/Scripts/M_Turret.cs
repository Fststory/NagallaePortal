

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class M_Turret : MonoBehaviour
{

    //public GameObject turretbullet;  //Ray 방식으로 변경하며 삭제
    public GameObject fireposition;
    public GameObject spotlight;
    public GameObject Particle;
    public GameObject bulletHoleparticle;
    public Collider playercol;
    public LayerMask turretEye;

    public Animator turretAnim;

    public float warningTime = 1.0f;
    public float rotationSpeed = 3.0f;

    public float rotX;
    public float rotZ;

    public bool warning;
    public bool shooting;
    public bool noshoot;

    public float shootTime = 0.0f;
    public float shootDelay = 0.3f;

    public bool turreteyeoff;

    public bool itsmesoundon = false;

    #region 쿼터니언 레이저 기울기
    //챗GPT
    public Transform laser; // 자식 레이저 Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 60f;
    #endregion

    #region 오디오
    //오디오
    public int randomrate2; //0~1랜덤 변수
    public AudioSource audioSourse;
    

    public AudioClip[] turretfire;
    public AudioClip[] turretSearching;
    public AudioClip[] turretLosting;
    public AudioClip[] turretgrapping;
    public AudioClip[] turretdie;
    public AudioClip[] turretAhhh;
    public AudioClip[] turretHey;

    bool soundcount = true;
    float dieaudioCount = 1.5f;

    bool grabsoundcount = true;

    public AudioMixer audioMixer; //오디오믹서
    //bool isMuffled = false; //음향효과 변수 //함수화해서 삭제
    bool letshoot = true;
    float shootsounddeltime = 0.6f;

    bool ahhh = false; //쓰러질때 소리지르게 만들기

    #endregion

    public M_BulletSound m_BulletSound;

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

        audioSourse = transform.GetComponent<AudioSource>(); //오디오 컴포넌트
       

    }

    void Update()
    {
        //터렛의 x축과 z축 각도를 계속 측정한다.
        rotX = gameObject.transform.eulerAngles.x; //오일러 앵글!!!!
        rotZ = gameObject.transform.eulerAngles.z;


        if ((Mathf.Abs(rotX) > 300 || Mathf.Abs(rotX) < 60) && (Mathf.Abs(rotZ) > 300 || Mathf.Abs(rotZ) < 60) && soundcount) //특정 각도 안일때... 활성화 상태로 친다.
                                                                                                                              //세워진 기준 60도를 초과일 때. (즉 300도 이상이거나 60도 이하여야 한다... 그래서 이런 기괴한)
                                                                                                                              //[프로토는 이거 씀] if (transform.parent == null) //부모 컴포넌트가 없을 때 (플레이어가 안 집으면) 활성화 상태로 친다.
        {
            if (shooting) // 활성화 상태에서 사격 중인가?
            {
                Particle.SetActive(true); //사격할거면 파티클 on
                turretAnim.SetTrigger("Attack");//사격할거면 사격 모드 //Shooting true 선언하는 곳으로 가자.
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
                        m_BulletSound.FireSound(); //소리
                        shootDelay = 0.3f; //0.3초마다 레이를 처쏴.
                    }

                }
                else
                {
                    shooting = false; //사격 종료
                    LostSound(); //사운드 재생
                    turretAnim.SetTrigger("SetIdle");//사격 모드 종료
                    turretAnim.ResetTrigger("Attack"); // SetIdle 트리거를 설정한 후 Attack 트리거를 리셋합니다.
                    Particle.SetActive(false); //파티클 종료
                    warning = false; //사격이 완전히 끝났으니 경고도 종료
                    warningTime = 1.0f; //사격이 완전히 끝났으니 경고시간 재설정
                }
            }
            if (gameObject.transform.parent != null) //활성화 상태에서 잡혔을 때
            {
                turretAnim.SetTrigger("Grapped");//애니메이터 버둥거리기 삽입
                if (grabsoundcount)
                {
                    PickSound();//오디오 넣기 //누구세요 사운드
                    grabsoundcount = false;
                }
            }
            else
            {
                turretAnim.SetTrigger("UnGrapped"); //애니메이터
            }
        }
        else if (soundcount) //특정 각도 밖인데 사망 사운드카운트가 아직 안 되었을 때
        {
            turreteyeoff = true;
            //bool letshoot = true;
            dieaudioCount -= Time.deltaTime; //오디오 트는 시간 카운트
            if (dieaudioCount > 0 && letshoot) //0.3초마다 가짜 사격
            {
                //shootTime -= Time.deltaTime;
                Particle.SetActive(true);
                
                if (!ahhh)
                {
                    audioSourse.clip = turretAhhh[0];
                    audioSourse.volume = 0.7f;
                    audioSourse.Play();
                    ahhh = true;
                }

                if (letshoot)
                {
                    if (shootsounddeltime > 0)
                    {
                        shootsounddeltime -= Time.deltaTime;
                    }
                    else
                    {
                        m_BulletSound.FireSound(); //소리
                        shootsounddeltime = 0.3f; //0.3초마다
                    }
                }
            }
            if (dieaudioCount < 0)
            {
                soundcount = false;
                letshoot = false;
                DieSound();
            }

        }
        else //특정 각도 밖이고 사운드카운트 끝났을 때
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

            if (GameManager.gm.isitOver) //게임 오버시 총소리 먹먹하게 들리기
            {
                MuffleSound(true);
            }
            else
            {
                MuffleSound(false);
            }

        
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
            //turretAnim.SetTrigger("Attack");//사격할거면 사격 모드
            //레이캐스트 방식
            RaycastHit hit;
            Vector3 rayOrigin = fireposition.transform.position;
            Vector3 rayDirection = fireposition.transform.forward; //기본 레이 방향

            //X축 각도에 랜덤값을 부여해서 총알이 흩뿌려지는 연출.
            float randomx = Random.Range(-6f, 6f); //랜덤값 변수
            float randomy = Random.Range(-2f, 2f);
            Quaternion randomRot = Quaternion.Euler(randomx, randomy, 0f); //랜덤값 각도
            Vector3 randomRayLego = randomRot * rayDirection; //곱해서 적용

            //print("랜덤 값! =" + randomRayLego); //디버그 로그

            //Debug.DrawRay(rayOrigin, randomRayLego, Color.red, 5.0f); //디버그용 레이 궤도

            if (Physics.Raycast(rayOrigin, randomRayLego, out hit, 50f, ~turretEye))
            {
                //Debug.Log("turret hit: " + hit.collider.gameObject.name); // 디버그 메시지

                if (hit.collider != null && hit.collider.gameObject.name == "Player")
                {
                    GameManager.gm.AddDamage(1); //처맞기
                }
                else if (hit.collider.gameObject.name.Contains("Window"))
                {
                    //총알 파티클 넣기(불렛홀이 남는 걸로)
                    GameObject hole = Instantiate(bulletHoleparticle); //인스턴스화

                    //레이 맞은 위치에 넣기
                    hole.transform.position = hit.point;
                    hole.transform.rotation = hit.collider.transform.rotation;

                    //사운드 분리할 예정 //못하겠더염
                }
                else if (hit.collider.gameObject.name.Contains("Turret"))
                {
                    AudioSource heyitsmeSound = hit.collider.gameObject.GetComponent<AudioSource>();
                    heyitsmeSound.clip = turretHey[0];
                    if (!itsmesoundon)
                    {
                        heyitsmeSound.Play();
                        itsmesoundon = true;
                    }
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

                    //사운드 분리할 예정 //못하겠다
                }
            }
        }

    #region 레이저 조작
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
    #endregion

    public void SearchingSound()
    {
        Randompick();
        audioSourse.clip = turretSearching[randomrate2];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }


    //public void FireSound() //사운드 분리함
    //{
    //    Randompick();
    //    bulletSource.clip = turretfire[randomrate2];
    //    bulletSource.volume = 0.7f;
    //    bulletSource.Play();
    //}

    public void LostSound()
        {
            Randompick();
            audioSourse.clip = turretLosting[randomrate2];
            audioSourse.volume = 0.7f;
            audioSourse.Play();

        }

        public void PickSound()
        {
            Randompick();
            audioSourse.clip = turretgrapping[randomrate2];
            audioSourse.volume = 0.7f;
            audioSourse.Play();
        }

        public void DieSound()
        {
            Randompick();
            audioSourse.clip = turretdie[randomrate2];
            audioSourse.volume = 0.7f;
            audioSourse.Play();
        }

        public void Randompick()
        {
            randomrate2 = Random.Range(0, 2);
            //print(randomrate2); //디버그 프린트
        }

        void MuffleSound(bool muffled)
        {
            if (muffled)
            {
                audioMixer.SetFloat("ExposePara", 500); // 500Hz로 낮추어 먹먹한 효과
                //불렛 사운드 오디오믹서도 같이
            }
            else
            {
                audioMixer.SetFloat("ExposePara", 22000); // 원래대로 돌려놓음
            }
        }
    
}




