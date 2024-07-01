using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPortalBullet : MonoBehaviour
{
    // 1) 발사 당시 보고 있던 방향으로 계속해서 나아간다.
    // 2) Portal 생성 가능 벽("CanPortalWall"Tag로 구분)에 부딪히면 해당 위치에 RedPortal을 생성하고 자신은 사라진다.
    // ㄴ 기존에 생성된 RedPortal이 있을 시 기존 RedPortal 제거와 동시에 1)을 실행한다.
    // ㄴ "CanPortalWall"Tag 이외의 모든 물체와 충돌 시 사라진다.

    public float bulletSpeed = 10.0f;

    public GameObject redPortalPrefab;


    void Start()
    {
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    
    private void OnCollisionEnter(Collision col)
    {
        // "CanPortalWall" 태그를 가진 물체와 충돌했다면...
        if (col.gameObject.CompareTag("CanPortalWall"))
        {
            // 충돌한 물체의 위치에 포탈을 생성한다.
            GameObject redportal = Instantiate(redPortalPrefab, transform.position, col.transform.rotation);

            // 자기는 사라진다.
            Destroy(gameObject);
        }
        else // "CanPortalWall" 태그 이외의 물체와 충돌했다면...
        {
            // 자기만 사라진다.
            Destroy(gameObject);
        }
    }
}
