using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class Y_PortalGunFire : MonoBehaviour
{
    #region ver.포탈 총알 발사(폐기) => 레이캐스트 이용
    // 좌클릭, 우클릭 시 나가는 포탈 총알을 다르게 설정한다.
    // 포탈 총알이 향하는 방향 : 포탈건의 입구를 기준으로 정면

    //public GameObject bluePTBulletPrefab;
    //public GameObject redPTBulletPrefab;
    //public GameObject firePosition;

    // 만약 총알이 씬에 존재하는 중이면 해당 색깔의 총알을 추가로 발사할 수 없다.
    // ㄴ activeInHierarchy 사용 총알을 ObjectPool에 넣어놓고 구현해보자.
    // 포탈 역시 같은 색의 포탈은 하나씩만 존재해야 하므로 하나가 존재할 때 추가로 발사한 해당 색깔의 포탈 생성조건을 만족시키면 기존의 포탈을 지우고 새로운 포탈을 생성할 것이다.
    #endregion

    #region 기존(폐기)
    //[SerializeField]
    //private GameObject redPortalPrefab;         // 빨간 포탈 프리펩 담을 변수
    //[SerializeField]
    //private GameObject bluePortalPrefab;        // 파랑 포탈 프리펩 담을 변수
    //GameObject[] portals;                       // 포탈 쌍
    //[SerializeField]
    //private LayerMask layer;                    // 생성 가능 구분 레이어 (기존 CanPortalWall 태그 이용 => 레이어로 구분)
    #endregion

    [SerializeField]
    private Y_PortalPair portals;             // 

    [SerializeField]
    private LayerMask layer;            // 포탈 생성 가능 유무를 구분할 레이어

    [SerializeField]
    private Crosshair crosshair;            //

    private Y_CamRotate cameraMove;          //

    private void Awake()
    {
        // Y_CamRotate 컴포넌트 캐싱
        cameraMove = GetComponent<Y_CamRotate>();
    }

    void Start()
    {
        #region 3항 연산자
        /* 받을 변수 = 조건식 ? 참일 때 : 거짓일 때
        조건식이 참이면 받을 변수에 참일 때의 값을, 조건식이 거짓이면 받을 변수에 거짓일 때의 값을 반환한다.
        string result = 35 > 15 ? "앞이 더 크다" : "뒤가 더 크다";
        */
        #endregion
    }

    void Update()
    {
        #region ver.포탈 총알 발사(폐기)
        //// 마우스 좌클릭 시 파랑 포탈 총알이 나간다.
        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject bPB = Instantiate(bluePTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        //}
        //// 마우스 우클릭 시 빨강 포탈 총알이 나간다.
        //if (Input.GetMouseButtonDown(1))
        //{
        //    GameObject rPB = Instantiate(redPTBulletPrefab, firePosition.transform.position, firePosition.transform.rotation);
        //}
        #endregion

        if (Input.GetMouseButtonDown(0))                                        // 좌클릭 시
        {
            FirePortal(0,transform.position, transform.forward, 250.0f);        // 현재 위치에서 바라보는 방향으로 최대 250m까지 가는 Ray 발사(0번 포탈 생성)
        }
        else if (Input.GetMouseButtonDown(1))                                   // 우클릭 시
        {
            FirePortal(1, transform.position, transform.forward, 250.0f);       // 현재 위치에서 바라보는 방향으로 최대 250m까지 가는 Ray 발사(1번 포탈 생성)
        }        
    }

    // 포탈 발사를 담당하는 함수
    void FirePortal(int portalNum, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(pos, dir, out hit, distance,layer);               

        if (hit.collider != null)
        {
            #region 기존(폐기)
            //if (portalNum == 0)
            //{
            //    portals[0] = Instantiate(redPortalPrefab, hit.point, portalRotation);
            //}
            //else if (portalNum == 1)
            //{
            //    portals[1] = Instantiate(bluePortalPrefab, hit.point, portalRotation);
            //}
            #endregion
            // Orient the portal according to camera look direction and surface direction.(원문)
            // 포탈이 설치될 때 방향을 잡아준다. (같은 곳에 포탈을 생성하더라도 어디서 쐈냐에 따라 포탈의 상하좌우가 결정된다.
            var cameraRotation = cameraMove.TargetRotation;                                         // 카메라의 시점 쿼터니언을 담는 변수(cameraRotation)
            var portalRight = cameraRotation * Vector3.right;                                       // 카메라의 시점이 바뀔 때 카메라의 오른쪽 벡터3 변수(portalRight)

            // 포탈 생성 지점 기준으로 카메라의 위치와 시점에 따라 포탈의 오른쪽 방향이 결정된다.
            if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))                                // 
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
            bool wasPlaced = portals.Portals[portalNum].PlacePortal(hit.collider, hit.point, portalRotation);    // 특정 포탈(빨강,파랑)이 설치됐는지 담는 bool 변수(wasPlaced)

            if (wasPlaced)                                           // 만약 포탈이 설치됐으면...
            {
                crosshair.SetPortalPlaced(portalNum, true);          // 해당 포탈과 연결된 크로스헤어를 활성화한다.
            }
        }
    }
}
