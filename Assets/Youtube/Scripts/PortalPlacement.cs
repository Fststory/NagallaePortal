using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraMove))]
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;             // 

    [SerializeField]
    private LayerMask layerMask;            // 포탈 생성 가능 유무를 구분할 레이어

    [SerializeField]
    private Crosshair crosshair;            //

    private CameraMove cameraMove;          //

    private void Awake()
    {
        // CameraMove 컴포넌트 캐싱
        cameraMove = GetComponent<CameraMove>();
    }

    private void Update()
    {
        // 좌클릭 시..
        if(Input.GetButtonDown("Fire1"))
        {
            // 현재 위치에서 바라보는(커서가 위치하는) 곳에 포탈을 생성한다. [포탈 번호는 0번, 최대 250미터까지 생성 가능]
            FirePortal(0, transform.position, transform.forward, 250.0f);
        }
        // 우클릭 시..
        else if (Input.GetButtonDown("Fire2"))
        {            
            // 현재 위치에서 바라보는(커서가 위치하는) 곳에 포탈을 생성한다. [포탈 번호는 1번, 최대 250미터까지 생성 가능]
            FirePortal(1, transform.position, transform.forward, 250.0f);
        }
    }

    // 포탈을 발사하는 함수
    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        // 좌,우클릭으로 발사된 레이(Ray)에 닿은 물체에 관한 정보를 담을 변수 hit
        RaycastHit hit;
        
        // 레이 발사
        Physics.Raycast(pos, dir, out hit, distance, layerMask);

        // 만약 레이가 콜라이더를 가진 물체와 닿는다면...
        if(hit.collider != null)
        {

            #region
            // If we shoot a portal, recursively fire through the portal.
            //if (hit.collider.tag == "Portal")
            //{
            //    var inPortal = hit.collider.GetComponent<Portal>();

            //    if(inPortal == null)
            //    {
            //        return;
            //    }

            //    var outPortal = inPortal.OtherPortal;

            //    // Update position of raycast origin with small offset.
            //    Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
            //    relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            //    pos = outPortal.transform.TransformPoint(relativePos);

            //    // Update direction of raycast.
            //    Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
            //    relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
            //    dir = outPortal.transform.TransformDirection(relativeDir);

            //    distance -= Vector3.Distance(pos, hit.point);

            //    FirePortal(portalID, pos, dir, distance);

            //    return;
            //}
            #endregion

            // Orient the portal according to camera look direction and surface direction.(원문)
            // 포탈이 설치될 때 방향을 잡아준다. (같은 곳에 포탈을 생성하더라도 어디서 쐈냐에 따라 포탈의 상하좌우가 결정된다.
            var cameraRotation = cameraMove.TargetRotation;                                         // 카메라의 시점 쿼터니언을 담는 변수(cameraRotation)
            var portalRight = cameraRotation * Vector3.right;                                       // 카메라의 시점이 바뀔 때 카메라의 오른쪽 벡터3 변수(portalRight)
            
            // 포탈 생성 지점 기준으로 카메라의 위치와 시점에 따라 포탈의 오른쪽 방향이 결정된다.
            if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))                                // 
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;                // 
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;            // 
            }

            var portalForward = -hit.normal;                                                        // 포탈의 앞은 생성되는 벽면의 법선과 반대 방향이다.(벽으로 깊숙히 들어가는 방향)
            var portalUp = -Vector3.Cross(portalRight, portalForward);                              // 포탈의 오른쪽, 앞쪽이 결정되면 자연스레 위의 방향이 결정된다.

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);                  // 계산 결과를 바탕으로 정렬한 포탈의 쿼터니언 변수(portalRotation)
            // Attempt to place the portal.
            // 
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);    // 특정 포탈(빨강,파랑)이 설치됐는지 담는 bool 변수(wasPlaced)

            if(wasPlaced)                                           // 만약 포탈이 설치됐으면...
            {
                crosshair.SetPortalPlaced(portalID, true);          // 해당 포탈과 연결된 크로스헤어를 활성화한다.
            }
        }
    }
}
