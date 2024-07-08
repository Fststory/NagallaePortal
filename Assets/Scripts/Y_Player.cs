using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Player : MonoBehaviour
{
    // Player�� �����ӿ� ���� Script �Դϴ�.
    // �� [ w,a,s,d ](�̵�), [ space ](����), [ ctrl ](�ɱ�)

    #region PlayerMove() ����
    // Player�� �̵� �ӵ� ����
    public float moveSpeed = 5;
    
    // Rigidbody ���� �� ������ ����
    public Rigidbody playerBody;
    public float jumpForce;
    // �� ��ġ, �ٴ� ���̾� ����;
    public Transform feetTransform;
    public LayerMask floorMask;
    #endregion

    #region PlayerRotate() ����
    // ȸ�� �ӵ� ����
    public float rotSpeed = 200f;
    // ���콺 �¿� �̵�(y�� ȸ�� ��) ����
    float mx = 0;
    #endregion

    #region PlayerHealth() ����
    // ü�� ����
    public float currentHp = 100.0f;
    public M_BulletMove bullet;
    #endregion


    void Update()
    {
        PlayerMove();
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

        // ���� ī�޶� �������� ������ ��ȯ�Ѵ�. (���� ���� ����)
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

        // [space]�� ������ ���� �Ұǵ�..
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� ���� ���� ��� �ִٸ�..
            if (Physics.CheckSphere(feetTransform.position, 0.5f, floorMask))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    private void OnTriggerEnter(Collider something)                         // Player�� ���𰡿� �浹���� �� ȣ��Ǵ� �Լ�
    {
        M_BulletMove bullet = something.GetComponent<M_BulletMove>();       // �浹ü�� M_BulletMove ������Ʈ�� ���� �ִ��� Ȯ��

        if (bullet != null)                                                 // ���� �浹ü�� �Ѿ��̶��..
        {            
            float damage = bullet.damage;                                   // M_BulletMove �Ѿ��� �������� ���� �ͼ�
            currentHp -= damage;                                            // ���� ü�¿��� ��´�.         
        }
    }
    void PlayerHealth()
    {
        if (currentHp < 0.0f)                                               // ���� ü���� �� ��������..
        {
            Destroy(gameObject);                                            // �÷��̾��� ���ӿ�����Ʈ�� �ı��Ѵ�.
        }
        if (currentHp < 100.0f)                                             // Ǯ�ǰ� �ƴϸ�...
        {
            currentHp += Time.deltaTime;                                    // ü���� ���ݾ� ȸ���ȴ�.
        }
    }
}
