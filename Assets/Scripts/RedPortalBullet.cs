using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPortalBullet : MonoBehaviour
{
    // 앞으로 계속해서 나아가고 싶다
    // 1) Portal 생성 가능 벽("CanPortalWall"Tag로 구분)에 부딪히면 해당 위치에 RedPortal을 생성하고 자신은 사라진다.
    // ㄴ 기존에 생성된 RedPortal이 있을 시 기존 RedPortal 제거와 동시에 1)을 실행한다.
    // ㄴ "CanPortalWall"Tag 이외의 모든 물체와 충돌 시 사라진다.

    public float bulletSpeed = 10.0f;

    public GameObject redPortalPrefab;


    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.forward * bulletSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("CanPortalWall"))
        {
            GameObject redportal = Instantiate(redPortalPrefab, col.transform.position, col.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
