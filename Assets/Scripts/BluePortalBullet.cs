using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortalBullet : MonoBehaviour
{
    // ������ ����ؼ� ���ư��� �ʹ� V
    // Portal ���� ���� ���� �ε����� �ش� ��ġ�� �����ϰ� RedPortal�� �����Ѵ�.
    // �� ������ ������ RedPortal�� ���� �� ���ſ� ���ÿ� RedPortal�� �����Ѵ�.

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
