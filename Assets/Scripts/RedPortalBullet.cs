using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPortalBullet : MonoBehaviour
{
    // ������ ����ؼ� ���ư��� �ʹ�
    // 1) Portal ���� ���� ��("CanPortalWall"Tag�� ����)�� �ε����� �ش� ��ġ�� RedPortal�� �����ϰ� �ڽ��� �������.
    // �� ������ ������ RedPortal�� ���� �� ���� RedPortal ���ſ� ���ÿ� 1)�� �����Ѵ�.
    // �� "CanPortalWall"Tag �̿��� ��� ��ü�� �浹 �� �������.

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
