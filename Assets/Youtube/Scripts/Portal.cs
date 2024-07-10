using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [field: SerializeField]
    public Portal OtherPortal { get; private set; }             // 접촉하는 포탈의 짝꿍 포탈

    [SerializeField]
    private Renderer outlineRenderer;                           // 외곽선 렌더링 담당 변수

    [field: SerializeField]
    public Color PortalColour { get; private set; }             // 포탈 색깔

    [SerializeField]
    private LayerMask placementMask;                            // 설치 가능 레이어 목록

    [SerializeField]
    private Transform testTransform;                            // 

    private List<PortalableObject> portalObjects = new List<PortalableObject>();        // 
    public bool IsPlaced { get; private set; } = false;         // 설치 여부: 시작부터 설치돼 있지 않기에 false
    private Collider wallCollider;                              // 포탈이 설치되는 벽의 콜라이더

    // Components.
    public Renderer Renderer { get; private set; }              // 포탈 렌더링 담당 변수
    private new BoxCollider collider;                           // 포탈 콜라이더 담당 변수

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();                 // 박스형 콜라이더
        Renderer = GetComponent<Renderer>();                    // 렌더러
    }

    private void Start()
    {
        outlineRenderer.material.SetColor("_OutlineColour", PortalColour);      // 외곽선을 PortalColour에 담은 색으로 칠한다.
        
        gameObject.SetActive(false);        // 포탈의 초기 상태는 비활성화
    }

    private void Update()
    {
        Renderer.enabled = OtherPortal.IsPlaced;            // 짝꿍 포탈의 설치 여부가 렌더링 여부를 책임짐

        for (int i = 0; i < portalObjects.Count; ++i)       // 포탈 오브젝트 리스트 길이만큼 반복
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);      // 

            if (objPos.z > 0.0f)
            {
                portalObjects[i].Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)         // 포탈의 트리거에 other가 닿는다면..
    {
        var obj = other.GetComponent<PortalableObject>();           // other가 PortalableObject 컴포넌트의 보유 여부를 obj에 담는다.
        if (obj != null)                                            // 만약 other가 PortalableObject 컴포넌트를 갖고 있다면..
        {
            portalObjects.Add(obj);                                 // 
            obj.SetIsInPortal(this, OtherPortal, wallCollider);     // 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if(portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;
        testTransform.rotation = rot;
        testTransform.position -= testTransform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();

        if (CheckOverlap())
        {
            this.wallCollider = wallCollider;
            transform.position = testTransform.position;
            transform.rotation = testTransform.rotation;

            gameObject.SetActive(true);
            IsPlaced = true;
            return true;
        }

        return false;
    }

    // Ensure the portal cannot extend past the edge of a surface.
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

        for(int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if(Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if(Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // Ensure the portal cannot intersect a section of wall.
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

    // Once positioning has taken place, ensure the portal isn't intersecting anything.
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

        // Ensure the portal does not intersect walls.
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, testTransform.rotation, placementMask);

        if(intersections.Length > 1)
        {
            return false;
        }
        else if(intersections.Length == 1) 
        {
            // We are allowed to intersect the old portal position.
            if (intersections[0] != collider)
            {
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.
        bool isOverlapping = true;

        for(int i = 1; i < checkPositions.Length - 1; ++i)
        {
            isOverlapping &= Physics.Linecast(checkPositions[i], 
                checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        }

        return isOverlapping;
    }

    // 포탈을 제거하는 함수
    public void RemovePortal()
    {
        // 포탈을 비활성화 후..
        gameObject.SetActive(false);
        // 설치되어 있지 않다고 변수에 저장
        IsPlaced = false;
    }
}
