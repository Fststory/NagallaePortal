using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BulletMove : MonoBehaviour
{
    //�÷��̾ ���� ���ư��� :: �����ð��� �ߴ��Ŷ� �Ȱ���

    public float movespeed;
    Vector3 dir;

    public GameObject turret;

    public float damage = 20.0f;      // ������ ���� �߰��߽��ϴ�_0708_YH


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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") //�÷��̾ �¾��� ��...
        {
            GameManager.gm.AddDamage(1); //���ӸŴ������� ������ 1 ����
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject );
        }
    }
}
