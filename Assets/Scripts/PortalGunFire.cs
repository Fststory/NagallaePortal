using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunFire : MonoBehaviour
{
    // 좌클릭, 우클릭 시 나가는 포탈 총알을 다르게 설정한다.

    public GameObject bluePortalPrefab;
    public GameObject redPortalPrefab;
    public GameObject firePosition;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bp = Instantiate(bluePortalPrefab,firePosition.transform.position,firePosition.transform.rotation);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject rp = Instantiate(redPortalPrefab,firePosition.transform.position,firePosition.transform.rotation);
        }
    }
}
