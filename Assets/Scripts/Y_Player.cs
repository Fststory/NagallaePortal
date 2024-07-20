using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Player : MonoBehaviour
{
    // Player의 움직임, 체력에 관한 Script 입니다.
    // ㄴ [ w,a,s,d ](이동), [ space ](점프), [ ctrl ](앉기)

    #region PlayerMove() 변수
    // Player의 이동 속도 변수
    public float moveSpeed = 5;
    Vector3 dir = Vector3.zero;

    // Rigidbody 선언 및 점프력, 땅 위에 있는지 변수
    private Rigidbody playerRB;
    public float jumpForce;
    public bool isGround;


    #endregion

    #region PlayerRotate() 변수
    // 초기 회전 상태 저장
    Quaternion playerRotation;
    // 회전 속도 변수
    public float rotSpeed = 200f;
    // 마우스 좌우 이동(y축 회전 값) 변수
    float mx = 0;
    #endregion

    #region PlayerHealth() 변수 (폐기)
    //// 체력 변수
    //public float currentHp = 100.0f;    // 현재 체력(피해 입기 전 세팅 값: 100)
    //public M_BulletMove bullet;         // 피해를 주는 객체에서 데미지 값을 가져 옴
    #endregion

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();           // 점프 가능 여부 판정 시 사용(OnCollisionEnter 부분)

    }


    void Update()
    {
        // 물리를 이용한 움직임은 FixedUpdate에서..!
        //PlayerMove();
        //PlayerJump();
        PlayerRotate();
        //PlayerHealth();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerJump();
    }    

    void PlayerMove()
    {
        // 플레이어의 키 입력에 따라 움직임(전후좌우)을 구현
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        dir = new Vector3(moveX, 0, moveZ);
        dir.Normalize();

        // 메인 카메라를 기준으로 방향을 변환한다. (카메라가 바라보는 곳이 플레이어의 z축[앞 방향]이 된다.)
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        playerRB.velocity = new Vector3(dir.x * moveSpeed, playerRB.velocity.y, dir.z * moveSpeed);


        // p = p0 + vt
        //transform.position += dir * moveSpeed * Time.deltaTime;
        //playerRB.velocity = dir * moveSpeed;
    }

    void PlayerJump()
    {
        // 플레이어가 땅을 밟고 있을 때 [space]를 누르면...
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);       // 점프력만큼 점프한다.
            isGround = false;                                                   // 땅에서 떨어짐을 표시(점프가 불가능)
        }
    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        playerRotation *= Quaternion.Euler(0, mx, 0);
        transform.rotation = playerRotation;
    }

    private void OnCollisionEnter(Collision something)              // Player의 점프 가능 여부 판단/ 피해를 체력에 적용시키는 기능(폐기)
    {
        if (something.gameObject.CompareTag("Ground"))              // Ground 태그가 있는 물체와 충돌하면
        {
            isGround = true;                                        // 다시 점프할 수 있다.
        }
        #region player 체력 => gamemanager에서 다룸
        //M_BulletMove bullet = something.gameObject.GetComponent<M_BulletMove>();        // 충돌체가 M_BulletMove 컴포넌트를 갖고 있는지 확인

        //if (bullet != null)                                                             // 만약 충돌체가 총알이라면..
        //{            
        //    float damage = bullet.damage;                                               // M_BulletMove 총알의 데미지를 갖고 와서
        //    currentHp -= damage;                                                        // 현재 체력에서 깎는다.         
        //}
        #endregion
    }
    #region player 체력 => gamemanager에서 다룸
    //void PlayerHealth()
    //{
    //    if (currentHp < 0.0f)                   // 만약 체력이 다 떨어지면..
    //    {
    //        Destroy(gameObject);                // 플레이어의 게임오브젝트를 파괴한다.
    //    }
    //    if (currentHp < 100.0f)                 // 풀피가 아니면...
    //    {
    //        currentHp += Time.deltaTime;        // 체력이 조금씩 회복된다.
    //    }
    //}
    #endregion
}