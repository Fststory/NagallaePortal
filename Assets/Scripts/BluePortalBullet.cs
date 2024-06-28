using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortalBullet : MonoBehaviour
{
    // 앞으로 계속해서 나아가고 싶다 V
    // Portal 생성 가능 벽에 부딪히면 해당 위치에 평평하게 RedPortal을 생성한다.
    // ㄴ 기존에 생성된 RedPortal이 있을 시 제거와 동시에 RedPortal을 생성한다.

    public float bulletSpeed = 10.0f;

    public GameObject bluePortalPrefab;


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
            GameObject blueportal = Instantiate(bluePortalPrefab, col.transform.position, col.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
