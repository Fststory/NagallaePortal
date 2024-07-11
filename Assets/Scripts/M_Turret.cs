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

    //êGPT
    public Transform laser; // �ڽ� ������ Transform

    private Transform playerTransform;
    private Quaternion originalRotation;
    private float maxRotationAngle = 90f;

    void Start()
    {
        //ê GPT
        if (playercol.gameObject != null)
        {
            playerTransform = playercol.gameObject.transform;
        }
        // �ڽ� �������� ���� ȸ�� ���� ����
        originalRotation = laser.localRotation;
    }

    void Update()
    {
        
        if (transform.parent == null) //�θ� ������Ʈ�� ���� �� (�÷��̾ �� ������) Ȱ��ȭ ���·� ģ��.
        {
            if (shooting) // ��� ���ΰ�?
            {
                if (shootTime > 0) //0.3�ʸ��� �Ѿ� �ϳ��� ����ϵ��� �ϴ� ��
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
                    shooting = false; //��� ����
                    warning = false; //����� ������ �������� ��� ����
                    warningTime = 1.0f; //����� ������ �������� ���ð� �缳��
                }
            }
        }
        else //�÷��̾ ������� ����
        {
            spotlight.SetActive(false);
            noshoot = true;
        }


        //������ ���󰡰� �ϱ�
        //lerp�� ���°� ���� �� ����.
        //�ٵ� �ϴ� ê GPT��

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

    private void OnTriggerStay(Collider other)
    {
        if (!noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                if (warningTime == 1.0f) //ù ����ΰ�?
                {
                    warning = true; //��� ����
                    warningTime -= Time.deltaTime; //Ÿ�̸� ����
                }
                else if (warningTime > 0) //��� ���ΰ�?
                {
                    warningTime -= Time.deltaTime; //Ÿ�̸� ���
                }
                else if (warningTime < 0) //���ð��� �����ٸ�
                {
                    shooting = true;
                    shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
                }

                //shooting = true;
                //shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                //��� ������ ������ �� �ߵ��� ��ɾ�
                if (!shooting && warningTime < 1.0f)
                {
                    warningTime += Time.deltaTime;
                }
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
        laser.localRotation = Quaternion.Slerp(laser.localRotation, clampedRotation, rotationSpeed * Time.deltaTime);
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




