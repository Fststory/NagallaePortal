using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Y_CanUsePortal : MonoBehaviour
{
    // 이 스크립트를 가진 게임오브젝트는 포탈 간 이동이 가능합니다.

    #region 기존 버전 (실패)
    //public GameObject rPortal;
    //public GameObject bPortal;


    //void Start()
    //{

    //}

    //void Update()
    //{
    //    bPortal = GameObject.FindGameObjectWithTag("BluePortal");
    //    rPortal = GameObject.FindGameObjectWithTag("RedPortal");
    //}

    //private void OnTriggerEnter(Collider Portal)
    //{
    //    // 만약 "RedPortal" 태그를 가진 GameObject와 닿았다면
    //    if (Portal.gameObject.CompareTag("RedPortal"))
    //    {
    //        // 씬에 생성돼있는 BluePortal로 이동된다.
    //        transform.position = bPortal.transform.position;
    //    }
    //    //else if (Portal.gameObject.CompareTag("BluePortal"))
    //    //{
    //    //    transform.position = rPortal.transform.position;
    //    //}
    //}
    #endregion

    private GameObject cloneObject;     // 포탈 사이에 물체가 있을 때 양 포탈에 상이 맺히게 해줌

    private int inPortalCount = 0;

    private Y_Portal inPortal;
    private Y_Portal outPortal;

    private new Rigidbody rigidbody;
    protected new Collider collider;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);     // y축 기준 180도 회전을 담은 쿼터니언 변수(halfTurn)

    protected virtual void Awake()
    {
        cloneObject = new GameObject();                                     // 클론 오브젝트(이하 "클론") 생성 
        cloneObject.SetActive(false);                                       // 클론 비활성화
        var meshFilter = cloneObject.AddComponent<MeshFilter>();            // 클론에 MeshFilter 컴포넌트 추가
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();        // 클론에 MeshRenderer 컴포넌트 추가

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;                  // 클론의 Mesh = 겜오브젝의 Mesh
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;    // 클론의 Materials = 겜오브젝의 Materials
        cloneObject.transform.localScale = transform.localScale;            // 클론의 크기(localScale)는 겜오브젝 본체의 크기와 같다.

        rigidbody = GetComponent<Rigidbody>();                              // 겜오브젝의 리지드바디 컴포넌트 캐싱
        collider = GetComponent<Collider>();                                // 겜오브젝의 콜라이더 컴포넌트 캐싱
    }

    private void LateUpdate()
    {
        if (inPortal == null || outPortal == null)                           // 만약 inPortal과 outPortal 중 하나라도 비어있다면..
        {
            return;     // 아무 값도 반환하지 않는다.
        }

        // 만약 클론 오브젝트, inPortal, outPortal이 모두 활성화 돼있다면..
        if (cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)
        {
            var inTransform = inPortal.transform;           // inPortal의 위치, 회전 정보
            var outTransform = outPortal.transform;         // outPortal의 위치, 회전 정보

            // Update position of clone.(원문)
            // Warp()의 포지션 바꾸기와 동일
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.(원문)
            // Warp()의 로테이션 바꾸기와 동일
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void SetIsInPortal(Y_Portal inPortal, Y_Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;           // 인 포탈 설정
        this.outPortal = outPortal;         // 아웃 포탓 설정

        Physics.IgnoreCollision(collider, wallCollider);    // 겜오브젝과 포탈이 설치된 벽 사이의 충돌을 무시한다.(벽 뚫기)

        cloneObject.SetActive(true);

        ++inPortalCount;
    }

    public void ExitPortal(Collider wallCollider)
    {
        Physics.IgnoreCollision(collider, wallCollider, false);     // 겜오브젝과 포탈이 설치된 벽 사이의 충돌 무시를 해제한다.(벽 뚫기 불가)
        --inPortalCount;

        if (inPortalCount == 0)
        {
            cloneObject.SetActive(false);
        }
    }
    public virtual void Warp()      // 포탈 간 이동 및 그에 따른 방향, 속도 처리를 담당하는 함수, (말 그대로 워프)
    {
        var inTransform = inPortal.transform;                       // 인포탈의 트랜스폼을 담는 변수(inTransform)
        var outTransform = outPortal.transform;                     // 아웃포탈의 트랜스폼을 담는 변수(outTransform)

        // Quaternion halfTurn의 역할 => 두 포탈 간 이동할 때 포탈 입장에서 봤을 때 이용자의 좌우 방향이 들어갈 때와 나올 때 달라짐을 보완해줌.
        // Quaternion halfTurn이 없으면 포탈에 들어가서 나오질 못 할 것이다. (포탈 이용 간 방향 계산에서 무한 루프할 것임)

        // Update position of object.(원문)
        // 반대편 포탈에서의 위치 계산 및 적용(좌우 보정)
        // 들어갈 때 인 포탈의 오른쪽으로 들어가면 나올 땐 아웃 포탈의 왼쪽으로 나와야 됨.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.(원문)
        // 반대편 포탈에서의 방향 계산 및 적용(좌우 보정)
        // 들어갈 때 인 포탈의 중심을 기준으로 북서쪽 방향으로 들어가면 나올 땐 아웃 포탈의 중심을 기준으로 남동쪽 방향으로 나와야 됨.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;     // 포탈에 들어가는 방향을 담음
        relativeRot = halfTurn * relativeRot;           // 들어가는 방향과 반대 방향 반환
        transform.rotation = outTransform.rotation * relativeRot;       // 포탈에서 나올 때의 방향을 적용한다.

        // Update velocity of rigidbody.(원문)
        // 반대편 포탈에서의 속도 계산 및 적용(좌우 보정)
        //
        Vector3 relativeVel = inTransform.InverseTransformDirection(rigidbody.velocity);
        relativeVel = halfTurn * relativeVel;
        rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // Swap portal references.(원문)
        // 워프가 끝나고 인/아웃 포탈을 서로 교체
        // 아직 용도를 모르겠음. 인포탈과 아웃포탈은 포탈에 들어설 때 매번 결정되는데 이 구문은 어디에 쓰이는 지 모르겠음.
        // 클론 오브젝트하고 연관 있을지도
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}