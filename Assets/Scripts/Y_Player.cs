using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Player : MonoBehaviour
{
    // Player�� ������, ü�¿� ���� Script �Դϴ�.
    // �� [ w,a,s,d ](�̵�), [ space ](����), [ ctrl ](�ɱ�)

    #region PlayerMove() ����
    // Player�� �̵� �ӵ� ����
    public float moveSpeed = 5;
    Vector3 dir = Vector3.zero;
    

    // Rigidbody ���� �� ������, �� ���� �ִ��� ����
    private Rigidbody playerRB;
    public float jumpForce;
    public bool isGround;


    #endregion

    #region PlayerRotate() ����
    // �ʱ� ȸ�� ���� ����
    Quaternion playerRotation;
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;
    // ���콺 �¿� �̵�(y�� ȸ�� ��) ����
    float mx = 0;
    #endregion

    #region PlayerHealth() ���� (���)
    //// ü�� ����
    //public float currentHp = 100.0f;    // ���� ü��(���� �Ա� �� ���� ��: 100)
    //public M_BulletMove bullet;         // ���ظ� �ִ� ��ü���� ������ ���� ���� ��
    #endregion

    Animator playerAnimation;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();           // ���� ���� ���� ���� �� ���(OnCollisionEnter �κ�)
        playerAnimation = transform.Find("Sparrow_LOD0").gameObject.GetComponent<Animator>();     // �ִϸ����� ĳ��!
    }


    void Update()
    {
        // ������ �̿��� �������� FixedUpdate����..!
        //PlayerMove();
        //PlayerJump();
        PlayerRotate();
        //PlayerHealth();        
        PlayerJump();
        if (playerRB.velocity.y < 0)
        {
            playerAnimation.SetBool("Flying", true);
        }
        else
        {
            playerAnimation.SetBool("Flying", false);
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }    

    void PlayerMove()
    {
        playerAnimation.SetBool("Walking", true);
        // �÷��̾��� Ű �Է¿� ���� ������(�����¿�)�� ����
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        dir = new Vector3(moveX, 0, moveZ);
        dir.Normalize();

        // ���� ī�޶� �������� ������ ��ȯ�Ѵ�. (ī�޶� �ٶ󺸴� ���� �÷��̾��� z��[�� ����]�� �ȴ�.)
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        playerRB.velocity = new Vector3(dir.x * moveSpeed, playerRB.velocity.y, dir.z * moveSpeed);        

        if (moveX == 0 && moveZ == 0)
        {
            playerAnimation.SetBool("Walking", false);
        }
        
        // p = p0 + vt
        //transform.position += dir * moveSpeed * Time.deltaTime;
        //playerRB.velocity = dir * moveSpeed;
    }

    void PlayerJump()
    {
        // �÷��̾ ���� ��� ���� �� [space]�� ������...
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            playerAnimation.SetTrigger("Jumping");
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);       // �����¸�ŭ �����Ѵ�.
            isGround = false;                                                   // ������ �������� ǥ��(������ �Ұ���)
        }
    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        playerRotation *= Quaternion.Euler(0, mx, 0);
        transform.rotation = playerRotation;
    }

    private void OnCollisionEnter(Collision something)              // Player�� ���� ���� ���� �Ǵ�/ ���ظ� ü�¿� �����Ű�� ���(���)
    {
        if (something.gameObject.CompareTag("Ground"))              // Ground �±װ� �ִ� ��ü�� �浹�ϸ�
        {
            isGround = true;                                        // �ٽ� ������ �� �ִ�.
        }
        #region player ü�� => gamemanager���� �ٷ�
        //M_BulletMove bullet = something.gameObject.GetComponent<M_BulletMove>();        // �浹ü�� M_BulletMove ������Ʈ�� ���� �ִ��� Ȯ��

        //if (bullet != null)                                                             // ���� �浹ü�� �Ѿ��̶��..
        //{            
        //    float damage = bullet.damage;                                               // M_BulletMove �Ѿ��� �������� ���� �ͼ�
        //    currentHp -= damage;                                                        // ���� ü�¿��� ��´�.         
        //}
        #endregion
    }
    #region player ü�� => gamemanager���� �ٷ�
    //void PlayerHealth()
    //{
    //    if (currentHp < 0.0f)                   // ���� ü���� �� ��������..
    //    {
    //        Destroy(gameObject);                // �÷��̾��� ���ӿ�����Ʈ�� �ı��Ѵ�.
    //    }
    //    if (currentHp < 100.0f)                 // Ǯ�ǰ� �ƴϸ�...
    //    {
    //        currentHp += Time.deltaTime;        // ü���� ���ݾ� ȸ���ȴ�.
    //    }
    //}
    #endregion
}