using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunFire : MonoBehaviour
{
    #region ver.��Ż �Ѿ� �߻�(���)
    // ��Ŭ��, ��Ŭ�� �� ������ ��Ż �Ѿ��� �ٸ��� �����Ѵ�.
    // ��Ż �Ѿ��� ���ϴ� ���� : ��Ż���� �Ա��� �������� ����

    //public GameObject bluePTBulletPrefab;
    //public GameObject redPTBulletPrefab;
    //public GameObject firePosition;

    // ���� �Ѿ��� ���� �����ϴ� ���̸� �ش� ������ �Ѿ��� �߰��� �߻��� �� ����.
    // �� activeInHierarchy ��� �Ѿ��� ObjectPool�� �־���� �����غ���.
    // ��Ż ���� ���� ���� ��Ż�� �ϳ����� �����ؾ� �ϹǷ� �ϳ��� ������ �� �߰��� �߻��� �ش� ������ ��Ż ���������� ������Ű�� ������ ��Ż�� ����� ���ο� ��Ż�� ������ ���̴�.
    #endregion

    [SerializeField]
    private GameObject redPortal;
    [SerializeField]
    private GameObject bluePortal;
    GameObject[] portals;


    void Start()
    {
        
    }

    void Update()
    {
        #region ver.��Ż �Ѿ� �߻�(���)
        //// ���콺 ��Ŭ�� �� �Ķ� ��Ż �Ѿ��� ������.
        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject bPB = Instantiate(bluePTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        //}
        //// ���콺 ��Ŭ�� �� ���� ��Ż �Ѿ��� ������.
        //if (Input.GetMouseButtonDown(1))
        //{
        //    GameObject rPB = Instantiate(redPTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        //}
        #endregion
        if (Input.GetMouseButtonDown(0))
        {
            FirePortal(0,transform.position, transform.forward, 250.0f);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FirePortal(1, transform.position, transform.forward, 250.0f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemovePortal();
        }

    }

    void FirePortal(int portalId, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(pos, dir, out hit, distance);
               
        Quaternion portalRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        if (hit.collider.CompareTag("CanPortalWall"))
        {
            if (portalId == 0)
            {
                portals[0] = Instantiate(redPortal, hit.point, portalRotation);
            }
            else if (portalId == 1)
            {
                portals[1] = Instantiate(bluePortal, hit.point, portalRotation);
            }
        }
        
    }

    void RemovePortal()
    {
        Destroy(portals[0]);
        Destroy(portals[1]);
    }
}
