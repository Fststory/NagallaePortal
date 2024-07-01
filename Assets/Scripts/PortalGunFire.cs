using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunFire : MonoBehaviour
{
    // 좌클릭, 우클릭 시 나가는 포탈 총알을 다르게 설정한다.
    // 포탈 총알이 향하는 방향 : 포탈건의 입구를 기준으로 정면

    public GameObject bluePTBulletPrefab;
    public GameObject redPTBulletPrefab;
    public GameObject firePosition;

    // 만약 총알이 씬에 존재하는 중이면 해당 색깔의 총알을 추가로 발사할 수 없다.
    // ㄴ activeInHierarchy 사용 총알을 ObjectPool에 넣어놓고 구현해보자.
    // 포탈 역시 같은 색의 포탈은 하나씩만 존재해야 하므로 하나가 존재할 때 추가로 발사한 해당 색깔의 포탈 생성조건을 만족시키면 기존의 포탈을 지우고 새로운 포탈을 생성할 것이다.


    void Start()
    {
        
    }

    void Update()
    {
        // 마우스 좌클릭 시 파랑 포탈 총알이 나간다.
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bPB = Instantiate(bluePTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        }
        // 마우스 우클릭 시 빨강 포탈 총알이 나간다.
        if (Input.GetMouseButtonDown(1))
        {
            GameObject rPB = Instantiate(redPTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        }
    }
}
