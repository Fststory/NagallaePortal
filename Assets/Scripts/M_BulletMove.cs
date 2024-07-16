using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BulletMove : MonoBehaviour
{
    //플레이어를 향해 날아가기 :: 수업시간에 했던거랑 똑같이

    public float movespeed;
    Vector3 dir;

    public GameObject turret;

    public float damage = 20.0f;      // 데미지 변수 추가했습니다_0708_YH


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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") //플레이어가 맞았을 때...
        {
            GameManager.gm.AddDamage(1); //게임매니저에서 데미지 1 깎음
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject );
        }
    }
}
