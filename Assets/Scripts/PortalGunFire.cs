using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunFire : MonoBehaviour
{
    // ��Ŭ��, ��Ŭ�� �� ������ ��Ż �Ѿ��� �ٸ��� �����Ѵ�.

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
