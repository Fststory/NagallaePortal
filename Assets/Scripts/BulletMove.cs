using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    //플레이어를 향해 날아가기 :: 수업시간에 했던거랑 똑같이

    public float movespeed;
    Vector3 dir;

    public GameObject turret;

    //늙어쥑이는것도 똑같이
    public float lifespan = 0.7f;

    void Start()
    {
        dir = transform.forward;
    }

    void Update()
    {
        transform.position += dir*movespeed*Time.deltaTime;

        //늙어쥑이기
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            Destroy(gameObject);
        }

    }
}
