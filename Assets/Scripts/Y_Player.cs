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

    // Rigidbody ���� �� ������, �� ���� �ִ��� ����
    private Rigidbody playerRB;
    public float jumpForce;
    public bool isGround;
    
    #endregion

    #region PlayerRotate() ����
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;
    // ���콺 �¿� �̵�(y�� ȸ�� ��) ����
    float mx = 0;
    #endregion

    #region PlayerHealth() ����
    // ü�� ����
    public float currentHp = 100.0f;    // ���� ü��(���� �Ա� �� ���� ��: 100)
    public M_BulletMove bullet;         // ���ظ� �ִ� ��ü���� ������ ���� ���� ��
    #endregion

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();           // ���� ���� ���� ���� �� ���(OnCollisionEnter �κ�)
    }


    void Update()
    {
        PlayerMove();
        PlayerJump();
        PlayerRotate();
        PlayerHealth();
    }

    void PlayerMove()
    {
        // �÷��̾��� Ű �Է¿� ���� ������(�����¿�)�� ����
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(moveX, 0, moveZ);
        dir.Normalize();

        // ���� ī�޶� �������� ������ ��ȯ�Ѵ�. (ī�޶� �ٶ󺸴� ���� �÷��̾��� z��[�� ����]�� �ȴ�.)
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;        
    }

    void PlayerJump()
    {
        // �÷��̾ ���� ��� ���� �� [space]�� ������...
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);       // �����¸�ŭ �����Ѵ�.
            isGround = false;                                                   // ������ �������� ǥ��(������ �Ұ���)
        }
    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    private void OnCollisionEnter(Collision something)              // Player�� ���� ���� ���� �Ǵ�/ ���ظ� ü�¿� �����Ű�� ���
    {
        if (something.gameObject.CompareTag("Ground"))              // Ground �±װ� �ִ� ��ü�� �浹�ϸ�
        {
            isGround = true;                                        // �ٽ� ������ �� �ִ�.
        }

        M_BulletMove bullet = something.gameObject.GetComponent<M_BulletMove>();        // �浹ü�� M_BulletMove ������Ʈ�� ���� �ִ��� Ȯ��

        if (bullet != null)                                                             // ���� �浹ü�� �Ѿ��̶��..
        {            
            float damage = bullet.damage;                                               // M_BulletMove �Ѿ��� �������� ���� �ͼ�
            currentHp -= damage;                                                        // ���� ü�¿��� ��´�.         
        }
    }
    void PlayerHealth()
    {
        if (currentHp < 0.0f)                   // ���� ü���� �� ��������..
        {
            Destroy(gameObject);                // �÷��̾��� ���ӿ�����Ʈ�� �ı��Ѵ�.
        }
        if (currentHp < 100.0f)                 // Ǯ�ǰ� �ƴϸ�...
        {
            currentHp += Time.deltaTime;        // ü���� ���ݾ� ȸ���ȴ�.
        }
    }
}
