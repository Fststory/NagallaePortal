using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    //�÷��̾ ���� ���ư��� :: �����ð��� �ߴ��Ŷ� �Ȱ���

    public float movespeed;
    Vector3 dir;

    public GameObject turret;

    //�ľ����̴°͵� �Ȱ���
    public float lifespan = 0.7f;

    void Start()
    {
        dir = transform.forward;
    }

    void Update()
    {
        transform.position += dir*movespeed*Time.deltaTime;

        //�ľ����̱�
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            Destroy(gameObject);
        }

    }
}
