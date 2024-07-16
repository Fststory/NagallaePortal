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
    private Transform testTransform;                            // ��Ż ��ġ�� �������� ���� �����ϴ� ����

    private List<Y_CanUsePortal> portalObjects = new List<Y_CanUsePortal>();    
    // ��Ż ��� ���� ������Ʈ("���� �����")�� ����Ʈ, ��Ż�� �̿��� �� �ִ� �ڰ���
    // ��Ż ��� �ÿ��� �� ���� ��� Ư�� ����� �����Ѵ�

    public bool IsPlaced { get; private set; } = false;         // ��ġ ����: ���ۺ��� ��ġ�� ���� �ʱ⿡ false
    private Collider wallCollider;                              // ��Ż�� ��ġ�Ǵ� ���� �ݶ��̴�

    // Components.
    public Renderer Renderer { get; private set; }              // ��Ż ������ ��� ����
    private new BoxCollider collider;                           // ��Ż �ݶ��̴� ��� ���� (��Ż ��Ʋ X)

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();                 // �ڽ��� �ݶ��̴� ĳ��
        Renderer = GetComponent<Renderer>();                    // ������ ĳ��
    }

    private void Start()
    {
        outlineRenderer.material.SetColor("_OutlineColour", PortalColour);      // �ܰ����� PortalColour�� ���� ������ ĥ�Ѵ�.

        gameObject.SetActive(false);        // ��Ż�� �ʱ� ���´� ��Ȱ��ȭ
    }

    private void Update()
    {
        Renderer.enabled = OtherPortal.IsPlaced;            // ¦�� ��Ż�� ��ġ ���ΰ� ������ ���θ� å����

        for (int i = 0; i < portalObjects.Count; ++i)       // ����� ����Ʈ ���̸�ŭ �ݺ�
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);      // ��Ż�� �������� �� ������� ���� ��ġ

            if (objPos.z > 0.0f)            // ��Ż �߽ɿ��� �����̶� ������ ���ٸ�
            {
                portalObjects[i].Warp();    // �ش� ������� ������ �����Ѵ�.
            }
        }
    }

    private void OnTriggerEnter(Collider other)                     // other�� ��Ż�� Ʈ���ſ� ��´ٸ�..
    {
        var obj = other.GetComponent<Y_CanUsePortal>();             // obj�� other�� Y_CanUsePortal ������Ʈ�� ĳ��
        if (obj != null)                                            // ���� other�� Y_CanUsePortal ������Ʈ�� ���� �ִٸ�..
        {
            portalObjects.Add(obj);                                 // ����� ����Ʈ�� obj�� �߰��Ѵ�.
            obj.SetIsInPortal(this, OtherPortal, wallCollider);     // ���� �´���ִ� ��Ż�� inPortal, ¦���� outPortal�� �ȴ�. other�� ��Ż ������ �浹�� �����Ѵ�.
        }
    }

    private void OnTriggerExit(Collider other)                      // other�� ��Ż�� Ʈ���ſ��� �����..
    {
        var obj = other.GetComponent<Y_CanUsePortal>();             // obj�� other�� Y_CanUsePortal ������Ʈ�� ĳ��

        if (portalObjects.Contains(obj))                            // ���� other�� Y_CanUsePortal ������Ʈ�� ���� �ִٸ�..
        {
            portalObjects.Remove(obj);                              // ����� ����Ʈ���� obj�� �����Ѵ�.
            obj.ExitPortal(wallCollider);                           // other�� ��Ż ������ �浹�� �ٽ� Ȱ��ȭ�Ѵ�.
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;                                   // testTransform�� ��Ż Ray�� ���� ��ġ�� ��´�.
        testTransform.rotation = rot;                                   // testTransform�� ��Ż�� ȸ�� ������ ��´�.
        testTransform.position -= testTransform.forward * 0.001f;       // ������ ��¦ Ƣ�� ������

        FixOverhangs();                 // ��Ż�� ���� ���(�𼭸�)�� ����� �ʵ��� ��.
        FixIntersects();                // 

        if (CheckOverlap())             // 
        {
            this.wallCollider = wallCollider;               // ��Ż�� �ݶ��̴��� ��Ż�� ��ġ�Ǵ� ���� �ݶ��̴��� ����
            transform.position = testTransform.position;    // �׽�Ʈ ���� �հ��� ���� ��ġ�� ��Ż�� ��ġ�� �ȴ�
            transform.rotation = testTransform.rotation;    // �׽�Ʈ ���� �հ��� ���� ȸ���� ��Ż�� ȸ���� �ȴ�.

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

        var checkPositions = new Vector3[]      // ��Ż ��ġ �� ���� üũ�ϴ� ����3�� �迭
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
        // �߽��� checkPositions[0], x/y/z���� ������?�� checkExtents, ȸ�� ���ʹϾ��� testTransform.rotation�� �ڽ� �ȿ� �ִ� ��ü ��
        // Ư�� ���̾ ���� ��ü�� �ݶ��̴��� �迭�� ��ȯ

        if (intersections.Length > 1)   // ��Ż�� ������ ��ġ�� �� �� �̻��� ��Ż ���� ���� ��ü(���̾ ���� ��)�� �����ȴٸ�
        {
            return false;               // ��Ż�� ������ �� ����.
        }
        else if (intersections.Length == 1)     // �װ� �ƴ϶� ���� ��Ż ���� üũ �ڽ� �ȿ� �ϳ��� ��ü�� �����ȴٸ�
        {
            // We are allowed to intersect the old portal position.(����)
            // 
            if (intersections[0] != collider)   // ���� ��Ż ���� �� �� �ƴ϶��
            {
                return false;   //��Ż�� ������ �� ����.
            }
        }

        // Ensure the portal corners overlap a surface.(����)
        // ray�� ���� ������ �������� ��Ż�� ����ٸ� ��Ż�� �� �𼭸��� �� ���� ������ �� ���� �� �Ǵ���.
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
