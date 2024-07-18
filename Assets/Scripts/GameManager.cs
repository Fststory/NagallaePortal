using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public M_Lab16UI m_Lab16UI; //디폴트 UI 스크립트
    public GameObject gameoverUI; //게임오버 UI
    public GameObject lab16UI; //피격UI
    //public GameObject loadUI; //게임 불러오는 중 UI //잘 안돼서 삭제
    public GameObject pressESC; //ESC 눌렀을 때 나오는 설정 UI
    public GameObject portalUI; //포탈 UI

    #region M_플레이어 사망 제어 변수
    public GameObject player; //사망시 플레이어 움직임을 제어하기 위해 컴포넌트 제작
    public GameObject playerCam; //카메라도 뺏어봐 아오
    public GameObject portalGun; //포탈건 뺏어 뺏어
    public Transform deadposition; //플레이어 묫자리

    Y_Player _Y_Player; //플레이어 스크립트 제어용 
    Y_CanUsePortal _Y_CanUsePortal; //플레이어 스크립트 제어용 
    M_GrabMe _M_GrabMe; //플레이어 스크립트 제어용
    Y_CamRotate _Y_CamRotate; //카메라 스크립트 제어용

    public float collapsetime = 0.7f; //플레이어 쓰러지는 시간
    float currenttime = 0;
    float rotZ; //쓰러지는 각도(Z축)
    #endregion

    public int playerHP = 10; //총알 10대 맞으면 사망
    //int turretHP = 5; //총알 5대 맞으면 비활성화
    public bool isitOver = false; //플레이어 사망 확인 bool
    public bool escOn = false; //ESC 활성화 확인 bool


    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        Scene currentscene = SceneManager.GetActiveScene();

        gameoverUI.SetActive(false); //게임오버UI 비활성화
        pressESC.SetActive(false); //ESC_UI 비활성화

        #region 플레이어 쓰러지기용
        _Y_Player = player.gameObject.GetComponent<Y_Player>(); //플레이어 스크립트 캐싱
        _Y_CanUsePortal = player.gameObject.GetComponent<Y_CanUsePortal>();
        _M_GrabMe = player.gameObject.GetComponent<M_GrabMe>();
        _Y_CamRotate = playerCam.gameObject.GetComponent<Y_CamRotate>();
        #endregion

        #region 알파용 코드
        //if (currentscene.name == "LAB16_ver.build_PlayerHP100") //알파용 씬 HP재설정
        //{
        //    playerHP = 100;
        //}
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))            // F4 누르면...
        {
            ScenePortal(-1);                        // 이전 씬 스타트
        }
        if(Input.GetKeyDown(KeyCode.F5))            // F5 누르면...
        {
            ScenePortal(0);                         // 현재 씬 스타트
        }
        else if(Input.GetKeyDown(KeyCode.F6))       // F6 누르면...
        {
            ScenePortal(1);                         // 다음 씬 스타트
        }

        #region 게임오버 UI

        //플레이어의 HP가 0이 되면 게임오버 UI

        if (playerHP <= 0)
        {
            ShowGameOverUI();
        }

        //게임 오버 상태에서 좌클릭했을때 현재 씬을 다시 시작하기
        if (isitOver && Input.GetMouseButtonDown(0)) //isitover상태에서 좌클릭시
        {
            isitOver = false; //bool부터 풀어주고
            RestartGame(); //다시 시작
            //HP100Mode(); //베타용
        }

        #endregion

        if (!escOn && Input.GetKeyDown(KeyCode.Escape))
        {
            PressESC();
        }
        else if (escOn && Input.GetKeyDown(KeyCode.Escape)) //ESC UI 켜진 상태에서 한번 더 누르면?
        {
            Continue(); //계속하기 판정
        }
    }



    void ScenePortal(int num)      // 씬 이동 기능 구현!
    {
        // 현재 씬 인덱스(순서)를 확인한 뒤
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6을 통해 해당 씬 불러오기
        // F4(이전), F5(현재), F6(다음)
        SceneManager.LoadScene(currentSceneindex + num);

        // 커서 원상복구 (만약 해당 씬에서 시작할 때 커서 안보이게 설정돼 있으면 그대로 반영됨)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // 마우스 감도 조절 함수(미구현)
    {

    }

    public void AddDamage(int hit) //처맞을때마다 피 까이고 눈앞이 빨개지기
    {
        playerHP -= hit;
        StartCoroutine(m_Lab16UI.FadeOut());
    }

    public void ShowGameOverUI() //게임 오버!
    {
        //플레이어 쓰러지기

        //일단 조작 불가하게 스크립트 압수
        _Y_Player.enabled = false;
        _Y_CanUsePortal.enabled = false;
        _M_GrabMe.enabled = false;
        _Y_CamRotate.enabled = false;

        //포탈건 오브젝트째로 압수
        portalGun.SetActive(false);

        //피격 UI 압수
        lab16UI.SetActive(false);
        //포탈 UI 압수
        portalUI.SetActive(false);

        #region 실패했지만 나중에 찾을 것 같은 코드
        //이건 ShowGameOverUI에서 실행시 토글함수라 계속 깜빡거리면서 활성화 비활성화를 반복한다. 버튼 누를때나 가능할듯...
        //_Y_Player.enabled = !_Y_Player.enabled; //활성화 여부 토글
        //_Y_CanUsePortal.enabled = !_Y_CanUsePortal.enabled;
        //_M_GrabMe.enabled = !_M_GrabMe.enabled;
        #endregion

        //이제 쓰러지게 하면 되는데...
        deadposition.SetParent(null); //자식 오브젝트 안 떼어내면 누워서 뱅글뱅글 돈다.
        Vector3 startpos = player.transform.position; //현재 플레이어 위치를 startpos에 지정. 각도는 어차피 서있으니 생략.

        gameoverUI.SetActive(true);
        
        if ( currenttime < collapsetime) //쓰러지는 시간동안!
        {
            currenttime += Time.deltaTime; //타이머 흐르고
            float t = currenttime/collapsetime; //비율계산1
            t = Mathf.Pow(t, 2); //비율에 계속 제곱을 해서 점점 빨라지도록 한다.
            player.transform.position = Vector3.Lerp(startpos, deadposition.transform.position, t); //위치이동

            rotZ = Mathf.Lerp(rotZ, -90, t); //z축만 각도 계산
            player.transform.eulerAngles = new Vector3(0, 0, rotZ); //각도 이동
           
            
            Rigidbody PlayerRigid = player.gameObject.GetComponent<Rigidbody>(); //물리컴포넌트 갖고와봐 왜미끄러짐??
            PlayerRigid.isKinematic = true; //키네틱 켜
        }
        else
        {
            player.transform.position = deadposition.transform.position;
            player.transform.rotation = deadposition.transform.rotation; //눕게 만들기
            //Time.timeScale = 0.0f; //시간이 안 흐르게 하고싶음
        }
        isitOver = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1); //재시작
    }
    
    void PressESC()
    {
        escOn = true; //UI 켜졌으니 bool 참
        Time.timeScale = 0.0f;//시간 멈추기
        #region 압수할 게임오브젝트 목록
        portalUI.SetActive(false); //포탈 UI 압수하기
        //portalGun.SetActive(false); //포탈건 오브젝트째로 압수 //하면 큰일난다!!
        portalGun.GetComponent<Y_PortalGunFire>().enabled = false; //코드만 압수하기
        #endregion
        Cursor.lockState = CursorLockMode.Confined;//마우스 활성화하기
        pressESC.SetActive(true); //ESC UI 불러오기

    }

    public void Continue() //계임 계속
    {
        escOn = false; //UI 꺼졌으니 bool 거짓
        Cursor.lockState = CursorLockMode.Locked; //마우스 다시 잠그기 
        Time.timeScale = 1f; //시간 원상복구하기
        #region 압수한거 돌려줄 목록
        pressESC.SetActive(false); //ESC UI끄고...
        portalUI.SetActive(true); //포탈 UI 돌려주고...
        //portalGun.SetActive(true); //포탈건 오브젝트 돌려주기
        portalGun.GetComponent<Y_PortalGunFire>().enabled = true; //코드 돌려주기
        #endregion
    }

    public void EscToContinue() //ESC 메뉴에서 재시작
    {
        Time.timeScale = 1f; //시간 원상복구하기
        #region 압수한거 돌려줄 목록
        portalUI.SetActive(true); //포탈 UI 돌려주고...
        //portalGun.SetActive(true); //포탈건 오브젝트 돌려주기
        portalGun.GetComponent<Y_PortalGunFire>().enabled = true; //코드 돌려주기
        #endregion
        SceneManager.LoadScene(1); //재시작
    }

    public void GotoMain() //ESC메뉴에서 메인으로
    {
        Time.timeScale = 1f; //시간 원상복구하기
        #region 압수한거 돌려줄 목록
        portalUI.SetActive(true); //포탈 UI 돌려주고...
        //portalGun.SetActive(true); //포탈건 오브젝트 돌려주기
        portalGun.GetComponent<Y_PortalGunFire>().enabled = true; //코드 돌려주기
        #endregion
        SceneManager.LoadScene(0);
    }

    //public void HP100Mode()
    //{
    //    //알파용: 중간에 시작하고 HP가 100인 씬을 실행한다.
    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene("LAB16_ver.build_PlayerHP100");
    //}

}
