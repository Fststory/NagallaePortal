using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPortalBullet : MonoBehaviour
{
    // 1) �߻� ��� ���� �ִ� �������� ����ؼ� ���ư���.
    // 2) Portal ���� ���� ��("CanPortalWall"Tag�� ����)�� �ε����� �ش� ��ġ�� RedPortal�� �����ϰ� �ڽ��� �������.
    // �� ������ ������ RedPortal�� ���� �� ���� RedPortal ���ſ� ���ÿ� 1)�� �����Ѵ�.
    // �� "CanPortalWall"Tag �̿��� ��� ��ü�� �浹 �� �������.

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
        // "CanPortalWall" �±׸� ���� ��ü�� �浹�ߴٸ�...
        if (col.gameObject.CompareTag("CanPortalWall"))
        {
            // �浹�� ��ü�� ��ġ�� ��Ż�� �����Ѵ�.
            GameObject redportal = Instantiate(redPortalPrefab, transform.position, col.transform.rotation);

            // �ڱ�� �������.
            Destroy(gameObject);
        }
        else // "CanPortalWall" �±� �̿��� ��ü�� �浹�ߴٸ�...
        {
            // �ڱ⸸ �������.
            Destroy(gameObject);
        }
    }
}
