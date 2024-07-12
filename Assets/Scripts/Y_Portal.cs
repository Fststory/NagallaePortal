using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Portal : MonoBehaviour
{
    // ��Ż�� ����� ����ϴ� Ŭ�����Դϴ�.
    [field: SerializeField]
    public Y_Portal OtherPortal { get; private set; }             // �����ϴ� ��Ż�� ¦�� ��Ż

    [SerializeField]
    private Renderer outlineRenderer;                           // �ܰ��� ������ ��� ����

    [field: SerializeField]
    public Color PortalColour { get; private set; }             // ��Ż ����

    [SerializeField]
    private LayerMask placementMask;                            // ��ġ ���� ���̾� ���

    [SerializeField]
    private Transform testTransform;                            // 

    private List<Y_CanUsePortal> portalObjects = new List<Y_CanUsePortal>();        // 
    public bool IsPlaced { get; private set; } = false;         // ��ġ ����: ���ۺ��� ��ġ�� ���� �ʱ⿡ false
    private Collider wallCollider;                              // ��Ż�� ��ġ�Ǵ� ���� �ݶ��̴�

    // Components.
    public Renderer Renderer { get; private set; }              // ��Ż ������ ��� ����
    private new BoxCollider collider;                           // ��Ż �ݶ��̴� ��� ����

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();                 // �ڽ��� �ݶ��̴�
        Renderer = GetComponent<Renderer>();                    // ������
    }

    private void Start()
    {
        outlineRenderer.material.SetColor("_OutlineColour", PortalColour);      // �ܰ����� PortalColour�� ���� ������ ĥ�Ѵ�.

        gameObject.SetActive(false);        // ��Ż�� �ʱ� ���´� ��Ȱ��ȭ
    }

    private void Update()
    {
        Renderer.enabled = OtherPortal.IsPlaced;            // ¦�� ��Ż�� ��ġ ���ΰ� ������ ���θ� å����

        for (int i = 0; i < portalObjects.Count; ++i)       // ��Ż ������Ʈ ����Ʈ ���̸�ŭ �ݺ�
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);      // 

            if (objPos.z > 0.0f)
            {
                portalObjects[i].Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)         // ��Ż�� Ʈ���ſ� other�� ��´ٸ�..
    {
        var obj = other.GetComponent<Y_CanUsePortal>();           // obj�� other�� Y_CanUsePortal ������Ʈ�� ĳ��
        if (obj != null)                                            // ���� other�� Y_CanUsePortal ������Ʈ�� ���� �ִٸ�..
        {
            portalObjects.Add(obj);                                 // Y_CanUsePortals ����Ʈ�� obj�� �߰��Ѵ�.
            obj.SetIsInPortal(this, OtherPortal, wallCollider);     // ���� �´���ִ� ��Ż�� inPortal, ¦���� outPortal�� �ȴ�. other�� ��Ż ������ �浹�� �����Ѵ�.
        }
    }

    private void OnTriggerExit(Collider other)          // other�� ��Ż�� Ʈ���ſ��� �����..
    {
        var obj = other.GetComponent<Y_CanUsePortal>();           // obj�� other�� Y_CanUsePortal ������Ʈ�� ĳ��

        if (portalObjects.Contains(obj))                             // ���� other�� Y_CanUsePortal ������Ʈ�� ���� �ִٸ�
        {
            portalObjects.Remove(obj);                              // Y_CanUsePortals ����Ʈ���� obj�� �����Ѵ�.
            obj.ExitPortal(wallCollider);                           // other�� ��Ż ������ �浹�� �ٽ� Ȱ��ȭ�Ѵ�.
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;                                   // pos�� ���� ��ġ�� ���������� ����
        testTransform.rotation = rot;                                   // rot�� ���� ȸ���� �����̼����� ����
        testTransform.position -= testTransform.forward * 0.001f;       // ������ ��¦ Ƣ�� ������

        FixOverhangs();                 // ��Ż�� ���� ���(�𼭸�)�� ����� �ʵ��� ��.
        FixIntersects();                // 

        if (CheckOverlap())             // 
        {
            this.wallCollider = wallCollider;               // ��Ż�� �ݶ��̴��� ��Ż�� ��ġ�Ǵ� ���� �ݶ��̴��� ����
            transform.position = testTransform.position;    // 
            transform.rotation = testTransform.rotation;    // 

            gameObject.SetActive(true);                     // ��Ż�� Ȱ��ȭ
            IsPlaced = true;                                // IsPlaced�� true�� ����
            return true;                                    // true�� ��ȯ�ϸ� ����
        }

        return false;                   // false�� ��ȯ�ϸ� ����
    }

    // Ensure the portal cannot extend past the edge of a surface.(����)
    // ��Ż�� ���� ��踦 ����� �ʵ��� ��.
    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f,  0.0f, 0.1f),
            new Vector3( 1.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -2.1f, 0.1f),
            new Vector3( 0.0f,  2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // Ensure the portal cannot intersect a section of wall.(����)
    // ��Ż�� ���� ���� �ʰ� ��
    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    // Once positioning has taken place, ensure the portal isn't intersecting anything.(����)
    // ��Ż�� �ٸ� ��ü�� ��ġ�ų� �浹���� �ʰ� ����
    private bool CheckOverlap()
    {
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);

        var checkPositions = new Vector3[]
        {
            testTransform.position + testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f, -2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f,  2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( 1.0f, -2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( 1.0f,  2.0f, -0.1f)),

            testTransform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))
        };

        // Ensure the portal does not intersect walls.(����)
        // ��Ż�� ���� ��ġ�� �ʵ��� ����
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, testTransform.rotation, placementMask);

        if (intersections.Length > 1)
        {
            return false;
        }
        else if (intersections.Length == 1)
        {
            // We are allowed to intersect the old portal position.(����)
            // ������ ��ġ�� ��Ż���� ������ �� �ִ�.(������ ���� �����Ǹ鼭 ������ ��Ż�� ����� �״ϱ�)
            if (intersections[0] != collider)
            {
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.(����)
        // �� ���� ��Ż�� ������ �� �ְ� ����
        bool isOverlapping = true;

        for (int i = 1; i < checkPositions.Length - 1; ++i)
        {
            isOverlapping &= Physics.Linecast(checkPositions[i],
                checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        }

        return isOverlapping;
    }

    // ��Ż�� �����ϴ� �Լ� (������� �ʴ� ��? ���� ���� �Լ� �����س��� �� ���� �� ����)
    public void RemovePortal()
    {
        // ��Ż�� ��Ȱ��ȭ ��..
        gameObject.SetActive(false);
        // ��ġ�Ǿ� ���� �ʴٰ� ������ ����
        IsPlaced = false;
    }
}
