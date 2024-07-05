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
                    shooting = false; //��� ����
                    warning = false; //����� ������ �������� ��� ����
                    warningTime = 1.0f; //����� ������ �������� ���ð� �缳��
                }
            }
        }
        else
        {
            spotlight.SetActive(false);
            noshoot = true;
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

}
