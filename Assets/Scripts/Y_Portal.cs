using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Portal : MonoBehaviour
{
    // 포탈의 기능을 담당하는 클래스입니다.
    [field: SerializeField]
    public Y_Portal OtherPortal { get; private set; }             // 접촉하는 포탈의 짝꿍 포탈

    [SerializeField]
    private Renderer outlineRenderer;                           // 외곽선 렌더링 담당 변수

    [field: SerializeField]
    public Color PortalColour { get; private set; }             // 포탈 색깔

    [SerializeField]
    private LayerMask placementMask;                            // 설치 가능 레이어 목록

    [SerializeField]
    private Transform testTransform;                            // 

    private List<Y_CanUsePortal> portalObjects = new List<Y_CanUsePortal>();        // 
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
        var obj = other.GetComponent<Y_CanUsePortal>();           // obj에 other의 Y_CanUsePortal 컴포넌트를 캐싱
        if (obj != null)                                            // 만약 other가 Y_CanUsePortal 컴포넌트를 갖고 있다면..
        {
            portalObjects.Add(obj);                                 // Y_CanUsePortals 리스트에 obj를 추가한다.
            obj.SetIsInPortal(this, OtherPortal, wallCollider);     // 현재 맞닿아있는 포탈이 inPortal, 짝꿍이 outPortal이 된다. other과 포탈 사이의 충돌을 무시한다.
        }
    }

    private void OnTriggerExit(Collider other)          // other이 포탈의 트리거에서 벗어나면..
    {
        var obj = other.GetComponent<Y_CanUsePortal>();           // obj에 other의 Y_CanUsePortal 컴포넌트를 캐싱

        if (portalObjects.Contains(obj))                             // 만약 other가 Y_CanUsePortal 컴포넌트를 갖고 있다면
        {
            portalObjects.Remove(obj);                              // Y_CanUsePortals 리스트에서 obj를 제거한다.
            obj.ExitPortal(wallCollider);                           // other과 포탈 사이의 충돌을 다시 활성화한다.
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;                                   // pos에 들어온 위치를 포지션으로 저장
        testTransform.rotation = rot;                                   // rot에 들어온 회전을 로테이션으로 저장
        testTransform.position -= testTransform.forward * 0.001f;       // 벽에서 살짝 튀어 나오게

        FixOverhangs();                 // 포탈이 벽의 경계(모서리)를 벗어나지 않도록 함.
        FixIntersects();                // 

        if (CheckOverlap())             // 
        {
            this.wallCollider = wallCollider;               // 포탈의 콜라이더를 포탈이 설치되는 벽의 콜라이더로 지정
            transform.position = testTransform.position;    // 
            transform.rotation = testTransform.rotation;    // 

            gameObject.SetActive(true);                     // 포탈을 활성화
            IsPlaced = true;                                // IsPlaced를 true로 설정
            return true;                                    // true를 반환하며 종료
        }

        return false;                   // false를 반환하며 종료
    }

    // Ensure the portal cannot extend past the edge of a surface.(원문)
    // 포탈이 벽의 경계를 벗어나지 않도록 함.
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

    // Ensure the portal cannot intersect a section of wall.(원문)
    // 포탈이 벽에 끼지 않게 함
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

    // Once positioning has taken place, ensure the portal isn't intersecting anything.(원문)
    // 포탈이 다른 물체와 겹치거나 충돌하지 않게 해줌
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

        // Ensure the portal does not intersect walls.(원문)
        // 포탈이 벽과 겹치지 않도록 해줌
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, testTransform.rotation, placementMask);

        if (intersections.Length > 1)
        {
            return false;
        }
        else if (intersections.Length == 1)
        {
            // We are allowed to intersect the old portal position.(원문)
            // 이전에 설치한 포탈과는 교차할 수 있다.(어차피 새로 생성되면서 이전의 포탈은 사라질 테니까)
            if (intersections[0] != collider)
            {
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.(원문)
        // 벽 위에 포탈이 생성될 수 있게 해줌
        bool isOverlapping = true;

        for (int i = 1; i < checkPositions.Length - 1; ++i)
        {
            isOverlapping &= Physics.Linecast(checkPositions[i],
                checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        }

        return isOverlapping;
    }

    // 포탈을 제거하는 함수 (사용하지 않는 듯? 여기 말고 함수 선언해놓은 거 빼곤 안 보임)
    public void RemovePortal()
    {
        // 포탈을 비활성화 후..
        gameObject.SetActive(false);
        // 설치되어 있지 않다고 변수에 저장
        IsPlaced = false;
    }
}
