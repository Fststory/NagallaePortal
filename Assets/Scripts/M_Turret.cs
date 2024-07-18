

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_Turret : MonoBehaviour
{

    //public GameObject turretbullet;  //Ray ������� �����ϸ� ����
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

    //êGPT
    public Transform laser; // �ڽ� ������ Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 60f;

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
    }

    void Update()
    {
        //�ͷ��� x��� z�� ������ ��� �����Ѵ�.
        rotX = gameObject.transform.eulerAngles.x; //���Ϸ� �ޱ�!!!!
        rotZ = gameObject.transform.eulerAngles.z;
        

        if ((Mathf.Abs(rotX) > 300 || Mathf.Abs(rotX) < 60) && (Mathf.Abs(rotZ) > 300 || Mathf.Abs(rotZ) < 60) ) //Ư�� ���� ���϶�... Ȱ��ȭ ���·� ģ��.
            //������ ���� 60���� �ʰ��� ��. (�� 300�� �̻��̰ų� 60�� ���Ͽ��� �Ѵ�... �׷��� �̷� �Ⱬ��)
            //[������� �̰� ��] if (transform.parent == null) //�θ� ������Ʈ�� ���� �� (�÷��̾ �� ������) Ȱ��ȭ ���·� ģ��.
        {
            if (shooting) // ��� ���ΰ�?
            {
                Particle.SetActive(true); //����ҰŸ� ��ƼŬ on
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
                        shootDelay = 0.3f; //0.3�ʸ��� ���̸� ó��.
                    }

                }
                else
                {
                    shooting = false; //��� ����
                    Particle.SetActive(false); //��ƼŬ ����
                    warning = false; //����� ������ �������� ��� ����
                    warningTime = 1.0f; //����� ������ �������� ���ð� �缳��
                }
            }
        }
        else //Ư�� ���� ���϶�.
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
        //����ĳ��Ʈ ���
        RaycastHit hit;
        Vector3 rayOrigin = fireposition.transform.position;
        Vector3 rayDirection = fireposition.transform.forward; //�⺻ ���� ����

        //X�� ������ �������� �ο��ؼ� �Ѿ��� ��ѷ����� ����.
        float randomx = Random.Range(-6f, 6f); //������ ����
        float randomy = Random.Range(-2f, 2f);
        Quaternion randomRot = Quaternion.Euler(randomx, randomy, 0f); //������ ����
        Vector3 randomRayLego = randomRot * rayDirection; //���ؼ� ����

        print("���� ��! =" + randomRayLego);

        Debug.DrawRay(rayOrigin, randomRayLego, Color.red, 5.0f); //����׿� ���� �˵�

        if (Physics.Raycast(rayOrigin, randomRayLego, out hit, 50f, ~turretEye))
        {
            Debug.Log("turret hit: " + hit.collider.gameObject.name); // ����� �޽���

            if (hit.collider != null && hit.collider.gameObject.name == "Player")
            {
                GameManager.gm.AddDamage(1); //ó�±�
            }
            //else if (hit.collider.gameObject.tag == "LabObject")
            //{
            //    //���� �ٸ��� �ҰŶ� �и� //���� ������ ��ƼŬ Ȯ���Ϸ��� ��� ��Ȱ��ȭ�ص�
            //}
            else if (hit.collider.gameObject.name.Contains("Window"))
            {
                //�Ѿ� ��ƼŬ �ֱ�(�ҷ�Ȧ�� ���� �ɷ�)
                GameObject hole = Instantiate(bulletHoleparticle); //�ν��Ͻ�ȭ

                //���� ���� ��ġ�� �ֱ�
                hole.transform.position = hit.point;
                hole.transform.rotation = hit.collider.transform.rotation;

                //���� �и��� ����
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

                //���� �и��� ����
            }
        }
    }

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
}




