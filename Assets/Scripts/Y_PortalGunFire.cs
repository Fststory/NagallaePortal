using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PortalGunFire : MonoBehaviour
{
    #region ver.��Ż �Ѿ� �߻�(���) => ����ĳ��Ʈ �̿�
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
    private GameObject redPortalPrefab;         // ���� ��Ż ������ ���� ����
    [SerializeField]
    private GameObject bluePortalPrefab;        // �Ķ� ��Ż ������ ���� ����
    GameObject[] portals;                       // ��Ż ��


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

        if (Input.GetMouseButtonDown(0))                                        // ��Ŭ�� ��
        {
            FirePortal(0,transform.position, transform.forward, 250.0f);        // ���� ��ġ���� �ٶ󺸴� �������� �ִ� 250m���� ���� Ray �߻�(0�� ��Ż ����)
        }
        else if (Input.GetMouseButtonDown(1))                                   // ��Ŭ�� ��
        {
            FirePortal(1, transform.position, transform.forward, 250.0f);       // ���� ��ġ���� �ٶ󺸴� �������� �ִ� 250m���� ���� Ray �߻�(1�� ��Ż ����)
        }
        #region ��Ż�� ����� ���(�̱���)
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RemovePortal();
        //}
        #endregion
    }

    // ��Ż �߻縦 ����ϴ� �Լ�
    void FirePortal(int portalNum, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(pos, dir, out hit, distance);
               
        Quaternion portalRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        if (hit.collider.CompareTag("CanPortalWall"))
        {
            if (portalNum == 0)
            {
                portals[0] = Instantiate(redPortalPrefab, hit.point, portalRotation);
            }
            else if (portalNum == 1)
            {
                portals[1] = Instantiate(bluePortalPrefab, hit.point, portalRotation);
            }
        }
        
    }

    void RemovePortal()
    {
        Destroy(portals[0]);
        Destroy(portals[1]);
    }
}
