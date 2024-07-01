using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunFire : MonoBehaviour
{
    // ��Ŭ��, ��Ŭ�� �� ������ ��Ż �Ѿ��� �ٸ��� �����Ѵ�.
    // ��Ż �Ѿ��� ���ϴ� ���� : ��Ż���� �Ա��� �������� ����

    public GameObject bluePTBulletPrefab;
    public GameObject redPTBulletPrefab;
    public GameObject firePosition;

    // ���� �Ѿ��� ���� �����ϴ� ���̸� �ش� ������ �Ѿ��� �߰��� �߻��� �� ����.
    // �� activeInHierarchy ��� �Ѿ��� ObjectPool�� �־���� �����غ���.
    // ��Ż ���� ���� ���� ��Ż�� �ϳ����� �����ؾ� �ϹǷ� �ϳ��� ������ �� �߰��� �߻��� �ش� ������ ��Ż ���������� ������Ű�� ������ ��Ż�� ����� ���ο� ��Ż�� ������ ���̴�.


    void Start()
    {
        
    }

    void Update()
    {
        // ���콺 ��Ŭ�� �� �Ķ� ��Ż �Ѿ��� ������.
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bPB = Instantiate(bluePTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        }
        // ���콺 ��Ŭ�� �� ���� ��Ż �Ѿ��� ������.
        if (Input.GetMouseButtonDown(1))
        {
            GameObject rPB = Instantiate(redPTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        }
    }
}
