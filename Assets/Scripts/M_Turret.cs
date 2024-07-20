

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class M_Turret : MonoBehaviour
{

    //public GameObject turretbullet;  //Ray ������� �����ϸ� ����
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

    #region ���ʹϾ� ������ ����
    //êGPT
    public Transform laser; // �ڽ� ������ Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 60f;
    #endregion

    #region �����
    //�����
    public int randomrate2; //0~1���� ����
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

    public AudioMixer audioMixer; //������ͼ�
    //bool isMuffled = false; //����ȿ�� ���� //�Լ�ȭ�ؼ� ����
    bool letshoot = true;
    float shootsounddeltime = 0.6f;

    bool ahhh = false; //�������� �Ҹ������� �����

    #endregion

    public M_BulletSound m_BulletSound;

    void Start()
    {
        //��ƼŬ ����
        Particle.SetActive(false);

        //ê GPT
        if (playercol.gameObject != null)
        {
            playerTransform = playercol.gameObject.transform;
        }
        // �ڽ� �������� ���� ȸ�� ���� ����
        originalRotation = laser.rotation;

        audioSourse = transform.GetComponent<AudioSource>(); //����� ������Ʈ
       

    }

    void Update()
    {
        //�ͷ��� x��� z�� ������ ��� �����Ѵ�.
        rotX = gameObject.transform.eulerAngles.x; //���Ϸ� �ޱ�!!!!
        rotZ = gameObject.transform.eulerAngles.z;


        if ((Mathf.Abs(rotX) > 300 || Mathf.Abs(rotX) < 60) && (Mathf.Abs(rotZ) > 300 || Mathf.Abs(rotZ) < 60) && soundcount) //Ư�� ���� ���϶�... Ȱ��ȭ ���·� ģ��.
                                                                                                                              //������ ���� 60���� �ʰ��� ��. (�� 300�� �̻��̰ų� 60�� ���Ͽ��� �Ѵ�... �׷��� �̷� �Ⱬ��)
                                                                                                                              //[������� �̰� ��] if (transform.parent == null) //�θ� ������Ʈ�� ���� �� (�÷��̾ �� ������) Ȱ��ȭ ���·� ģ��.
        {
            if (shooting) // Ȱ��ȭ ���¿��� ��� ���ΰ�?
            {
                Particle.SetActive(true); //����ҰŸ� ��ƼŬ on
                turretAnim.SetTrigger("Attack");//����ҰŸ� ��� ��� //Shooting true �����ϴ� ������ ����.
                if (shootTime > 0) //0.3�ʸ��� �Ѿ� �ϳ��� ����ϵ��� �ϴ� ��
                {
                    shootTime -= Time.deltaTime;
                    if (shootDelay > 0)
                    {
                        shootDelay -= Time.deltaTime;
                    }
                    else
                    {
                        #region �Ѿ� ��ȯ -���Ž�-
                        //GameObject fir = Instantiate(turretbullet);
                        //fir.transform.position = fireposition.transform.position;
                        //fir.transform.rotation = fireposition.transform.rotation;

                        //fir.GetComponent<M_BulletMove>().turret = gameObject;
                        #endregion

                        ShootingTurret();
                        m_BulletSound.FireSound(); //�Ҹ�
                        shootDelay = 0.3f; //0.3�ʸ��� ���̸� ó��.
                    }

                }
                else
                {
                    shooting = false; //��� ����
                    LostSound(); //���� ���
                    turretAnim.SetTrigger("SetIdle");//��� ��� ����
                    turretAnim.ResetTrigger("Attack"); // SetIdle Ʈ���Ÿ� ������ �� Attack Ʈ���Ÿ� �����մϴ�.
                    Particle.SetActive(false); //��ƼŬ ����
                    warning = false; //����� ������ �������� ��� ����
                    warningTime = 1.0f; //����� ������ �������� ���ð� �缳��
                }
            }
            if (gameObject.transform.parent != null) //Ȱ��ȭ ���¿��� ������ ��
            {
                turretAnim.SetTrigger("Grapped");//�ִϸ����� ���հŸ��� ����
                if (grabsoundcount)
                {
                    PickSound();//����� �ֱ� //�������� ����
                    grabsoundcount = false;
                }
            }
            else
            {
                turretAnim.SetTrigger("UnGrapped"); //�ִϸ�����
            }
        }
        else if (soundcount) //Ư�� ���� ���ε� ��� ����ī��Ʈ�� ���� �� �Ǿ��� ��
        {
            turreteyeoff = true;
            //bool letshoot = true;
            dieaudioCount -= Time.deltaTime; //����� Ʈ�� �ð� ī��Ʈ
            if (dieaudioCount > 0 && letshoot) //0.3�ʸ��� ��¥ ���
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
                        m_BulletSound.FireSound(); //�Ҹ�
                        shootsounddeltime = 0.3f; //0.3�ʸ���
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
        else //Ư�� ���� ���̰� ����ī��Ʈ ������ ��
        {
            spotlight.SetActive(false);
            noshoot = true;
        }

            //������ ���󰡰� �ϱ�
            if (warning && playerTransform != null) //��� ���϶�
            {
                LaserGotoPlayer();
            }
            else
            {
                RotateLaserToOriginal();
            }

            #region �Ѿ� �׽�Ʈ �ڵ�
            //�Ѿ� �׽�Ʈ �ڵ�
            //if (Input.GetMouseButtonDown(0))
            //{
            //        GameObject fir = Instantiate(turretbullet);
            //        fir.transform.position = fireposition.transform.position;
            //        fir.transform.rotation = fireposition.transform.rotation;

            //        fir.GetComponent<BulletMove>().turret = gameObject;
            //}
            #endregion

            if (GameManager.gm.isitOver) //���� ������ �ѼҸ� �Ը��ϰ� �鸮��
            {
                MuffleSound(true);
            }
            else
            {
                MuffleSound(false);
            }

        
    }

        #region �ͷ� �ݶ��̴� �и�-�ش� �ڵ� TurretCollider�� ����
        //private void OnTriggerStay(Collider other)
        //{
        //    if (!noshoot)
        //    {
        //        if (other.gameObject.name == "Player")
        //        {
        //            if (warningTime == 1.0f) //ù ����ΰ�?
        //            {
        //                warning = true; //��� ����
        //                warningTime -= Time.deltaTime; //Ÿ�̸� ����
        //            }
        //            else if (warningTime > 0) //��� ���ΰ�?
        //            {
        //                warningTime -= Time.deltaTime; //Ÿ�̸� ���
        //            }
        //            else if (warningTime < 0) //���ð��� �����ٸ�
        //            {
        //                shooting = true;
        //                shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
        //            }

        //            //shooting = true;
        //            //shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
        //        }
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (!noshoot)
        //    {
        //        if (other.gameObject.name == "Player")
        //        {
        //            //��� ������ ������ �� �ߵ��� ��ɾ�
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
            //turretAnim.SetTrigger("Attack");//����ҰŸ� ��� ���
            //����ĳ��Ʈ ���
            RaycastHit hit;
            Vector3 rayOrigin = fireposition.transform.position;
            Vector3 rayDirection = fireposition.transform.forward; //�⺻ ���� ����

            //X�� ������ �������� �ο��ؼ� �Ѿ��� ��ѷ����� ����.
            float randomx = Random.Range(-6f, 6f); //������ ����
            float randomy = Random.Range(-2f, 2f);
            Quaternion randomRot = Quaternion.Euler(randomx, randomy, 0f); //������ ����
            Vector3 randomRayLego = randomRot * rayDirection; //���ؼ� ����

            //print("���� ��! =" + randomRayLego); //����� �α�

            //Debug.DrawRay(rayOrigin, randomRayLego, Color.red, 5.0f); //����׿� ���� �˵�

            if (Physics.Raycast(rayOrigin, randomRayLego, out hit, 50f, ~turretEye))
            {
                //Debug.Log("turret hit: " + hit.collider.gameObject.name); // ����� �޽���

                if (hit.collider != null && hit.collider.gameObject.name == "Player")
                {
                    GameManager.gm.AddDamage(1); //ó�±�
                }
                else if (hit.collider.gameObject.name.Contains("Window"))
                {
                    //�Ѿ� ��ƼŬ �ֱ�(�ҷ�Ȧ�� ���� �ɷ�)
                    GameObject hole = Instantiate(bulletHoleparticle); //�ν��Ͻ�ȭ

                    //���� ���� ��ġ�� �ֱ�
                    hole.transform.position = hit.point;
                    hole.transform.rotation = hit.collider.transform.rotation;

                    //���� �и��� ���� //���ϰڴ���
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
                    //�Ѿ� ��ƼŬ �ֱ�(�ҷ�Ȧ�� ���� �ɷ�)
                    GameObject hole = Instantiate(bulletHoleparticle); //�ν��Ͻ�ȭ

                    //���� ���� ��ġ�� �ֱ�
                    hole.transform.position = hit.point;

                    //���ʹϾ� ������ ���� ����
                    Quaternion yRotation = Quaternion.Euler(0, 180, 0);

                    //180�� ����� �ֱ�
                    hole.transform.rotation = hit.collider.transform.rotation * yRotation;

                    Vector3 normal = hit.normal;

                    //���� �и��� ���� //���ϰڴ�
                }
            }
        }

    #region ������ ����
    private void LaserGotoPlayer()
        {
            // �÷��̾� ���������� ���� ���
            Vector3 directionToPlayer = playerTransform.position - laser.position;
            directionToPlayer.y = 0; // y�� ������ ���� y���� 0���� ����

            // ��ǥ ȸ�� ���� ���
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // ���� ���� ����
            Quaternion clampedRotation = ClampRotation(targetRotation);

            // ���� ȸ������ ��ǥ ȸ������ ���������� ȸ��
            laser.rotation = Quaternion.Slerp(laser.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
        }

        void RotateLaserToOriginal()
        {
            // ���� ȸ�� ������ ���ư����� ����
            Quaternion clampedRotation = ClampRotation(originalRotation);

            // ���� ȸ������ ���� ȸ������ ���������� ȸ��
            laser.rotation = Quaternion.Slerp(laser.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
        }

        Quaternion ClampRotation(Quaternion targetRotation)
        {
            // ���� ȸ�� ������ ��ǥ ȸ�� ���� ���� ���� ���� ���
            float angleDifference = Quaternion.Angle(originalRotation, targetRotation);

            if (angleDifference > maxRotationAngle)
            {
                // ���� ���̰� �ִ� ȸ�� ������ �ʰ��ϸ�, �ִ� ȸ�� ������ ����
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


    //public void FireSound() //���� �и���
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
            //print(randomrate2); //����� ����Ʈ
        }

        void MuffleSound(bool muffled)
        {
            if (muffled)
            {
                audioMixer.SetFloat("ExposePara", 500); // 500Hz�� ���߾� �Ը��� ȿ��
                //�ҷ� ���� ������ͼ��� ����
            }
            else
            {
                audioMixer.SetFloat("ExposePara", 22000); // ������� ��������
            }
        }
    
}




