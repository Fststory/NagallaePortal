using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

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

    #region ����(���)
    //[SerializeField]
    //private GameObject redPortalPrefab;         // ���� ��Ż ������ ���� ����
    //[SerializeField]
    //private GameObject bluePortalPrefab;        // �Ķ� ��Ż ������ ���� ����
    //GameObject[] portals;                       // ��Ż ��
    //[SerializeField]
    //private LayerMask layer;                    // ���� ���� ���� ���̾� (���� CanPortalWall �±� �̿� => ���̾�� ����)
    #endregion

    [SerializeField]
    private Y_PortalPair portals;             // 

    [SerializeField]
    private LayerMask layer;            // ��Ż ���� ���� ������ ������ ���̾�

    [SerializeField]
    private Crosshair crosshair;            //

    private Y_CamRotate cameraMove;          //

    private void Awake()
    {
        // Y_CamRotate ������Ʈ ĳ��
        cameraMove = GetComponent<Y_CamRotate>();
    }

    void Start()
    {
        #region 3�� ������
        /* ���� ���� = ���ǽ� ? ���� �� : ������ ��
        ���ǽ��� ���̸� ���� ������ ���� ���� ����, ���ǽ��� �����̸� ���� ������ ������ ���� ���� ��ȯ�Ѵ�.
        string result = 35 > 15 ? "���� �� ũ��" : "�ڰ� �� ũ��";
        */
        #endregion
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
    }

    // ��Ż �߻縦 ����ϴ� �Լ�
    void FirePortal(int portalNum, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(pos, dir, out hit, distance,layer);               

        if (hit.collider != null)
        {
            #region ����(���)
            //if (portalNum == 0)
            //{
            //    portals[0] = Instantiate(redPortalPrefab, hit.point, portalRotation);
            //}
            //else if (portalNum == 1)
            //{
            //    portals[1] = Instantiate(bluePortalPrefab, hit.point, portalRotation);
            //}
            #endregion
            // Orient the portal according to camera look direction and surface direction.(����)
            // ��Ż�� ��ġ�� �� ������ ����ش�. (���� ���� ��Ż�� �����ϴ��� ��� ���Ŀ� ���� ��Ż�� �����¿찡 �����ȴ�.
            var cameraRotation = cameraMove.TargetRotation;                                         // ī�޶��� ���� ���ʹϾ��� ��� ����(cameraRotation)
            var portalRight = cameraRotation * Vector3.right;                                       // ī�޶��� ������ �ٲ� �� ī�޶��� ������ ����3 ����(portalRight)

            // ��Ż ���� ���� �������� ī�޶��� ��ġ�� ������ ���� ��Ż�� ������ ������ �����ȴ�.
            if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))                                // 
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;                // 
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;            // 
            }

            var portalForward = -hit.normal;                                                        // ��Ż�� ���� �����Ǵ� ������ ������ �ݴ� �����̴�.(������ ����� ���� ����)
            var portalUp = -Vector3.Cross(portalRight, portalForward);                              // ��Ż�� ������, ������ �����Ǹ� �ڿ����� ���� ������ �����ȴ�.

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);                  // ��� ����� �������� ������ ��Ż�� ���ʹϾ� ����(portalRotation)
            // Attempt to place the portal.
            // 
            bool wasPlaced = portals.Portals[portalNum].PlacePortal(hit.collider, hit.point, portalRotation);    // Ư�� ��Ż(����,�Ķ�)�� ��ġ�ƴ��� ��� bool ����(wasPlaced)

            if (wasPlaced)                                           // ���� ��Ż�� ��ġ������...
            {
                crosshair.SetPortalPlaced(portalNum, true);          // �ش� ��Ż�� ����� ũ�ν��� Ȱ��ȭ�Ѵ�.
            }
        }
    }
}
