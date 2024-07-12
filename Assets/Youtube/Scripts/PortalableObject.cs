using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PortalableObject : MonoBehaviour
{
    private GameObject cloneObject;

    private int inPortalCount = 0;
    
    private Portal inPortal;
    private Portal outPortal;

    private new Rigidbody rigidbody;
    protected new Collider collider;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);     // y축 기준 180도 회전을 담은 변수(halfTurn)

    protected virtual void Awake()
    {
        cloneObject = new GameObject();                                     // 클론 오브젝트 생성
        cloneObject.SetActive(false);                                       // 클론 오브젝트 비활성화
        var meshFilter = cloneObject.AddComponent<MeshFilter>();            // 클론 오브젝트에 MeshFilter 컴포넌트 추가
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();        // 클론 오브젝트에 MeshRenderer 컴포넌트 추가

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;                  // 겜오브젝의 MeshFilter의 mesh 캐싱
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;    // 겜오브젝의 MeshRenderer의 materials 캐싱
        cloneObject.transform.localScale = transform.localScale;            // 클론 오브젝트의 크기(localScale)는 겜오브젝 본체의 크기와 같다.

        rigidbody = GetComponent<Rigidbody>();                              // 겜오브젝의 리지드바디 컴포넌트 캐싱
        collider = GetComponent<Collider>();                                // 겜오브젝의 콜라이더 컴포넌트 캐싱
    }

    private void LateUpdate()
    {
        if(inPortal == null || outPortal == null)                           // 만약 inPortal과 outPortal 중 하나라도 비어있다면..
        {
            return;     // 아무 값도 반환하지 않는다.
        }

        // 만약 클론 오브젝트, inPortal, outPortal이 모두 활성화 돼있다면..
        if(cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)   
        {
            var inTransform = inPortal.transform;           // inPortal의 위치, 회전 정보
            var outTransform = outPortal.transform;         // outPortal의 위치, 회전 정보

            // Update position of clone.
            // 본체의 월드 좌표를 inPortal에서의 로컬 좌표로 계산해 relativePos에 담는다.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            // inPortal 기준으로 y축으로 180도 회전시킨다.
            relativePos = halfTurn * relativePos;
            // 클론의 위치를 outPortal 기준에서 relativePos 만큼의 위치에 둔다.
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;           // 인 포탈 설정
        this.outPortal = outPortal;         // 아웃 포탓 설정

        Physics.IgnoreCollision(collider, wallCollider);    // 겜오브젝과 포탈이 설치된 벽 사이의 충돌을 무시한다.(벽 뚫기)

        cloneObject.SetActive(false);

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

    public virtual void Warp()
    {
        var inTransform = inPortal.transform;                       // 인포탈의 트랜스폼을 담는 변수(inTransform)
        var outTransform = outPortal.transform;                     // 아웃포탈의 트랜스폼을 담는 변수(outTransform)

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        // Update velocity of rigidbody.
        Vector3 relativeVel = inTransform.InverseTransformDirection(rigidbody.velocity);
        relativeVel = halfTurn * relativeVel;
        rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // Swap portal references.
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}
