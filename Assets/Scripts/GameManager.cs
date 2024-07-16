using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public M_Lab16UI m_Lab16UI; //����Ʈ UI ��ũ��Ʈ
    public GameObject gameoverUI; //���ӿ��� UI
    public GameObject lab16UI; //�ǰ�UI

    #region M_�÷��̾� ��� ���� ����
    public GameObject player; //����� �÷��̾� �������� �����ϱ� ���� ������Ʈ ����
    public GameObject playerCam; //ī�޶� ����� �ƿ�
    public GameObject portalGun; //��Ż�� ���� ����
    public Transform deadposition; //�÷��̾� ���ڸ�

    Y_Player _Y_Player; //�÷��̾� ��ũ��Ʈ ����� 
    Y_CanUsePortal _Y_CanUsePortal; //�÷��̾� ��ũ��Ʈ ����� 
    M_GrabMe _M_GrabMe; //�÷��̾� ��ũ��Ʈ �����
    Y_CamRotate _Y_CamRotate; //ī�޶� ��ũ��Ʈ �����

    public float collapsetime = 0.7f; //�÷��̾� �������� �ð�
    float currenttime = 0;
    float rotZ; //�������� ����(Z��)
    #endregion

    public int playerHP = 10; //�Ѿ� 10�� ������ ���
    int turretHP = 5; //�Ѿ� 5�� ������ ��Ȱ��ȭ
    public bool isitOver = false;


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

        gameoverUI.SetActive(false); //���ӿ����� ��Ȱ��ȭ

        #region �÷��̾� ���������
        _Y_Player = player.gameObject.GetComponent<Y_Player>(); //�÷��̾� ��ũ��Ʈ ĳ��
        _Y_CanUsePortal = player.gameObject.GetComponent<Y_CanUsePortal>();
        _M_GrabMe = player.gameObject.GetComponent<M_GrabMe>();
        _Y_CamRotate = playerCam.gameObject.GetComponent<Y_CamRotate>();
        #endregion

        #region ���Ŀ� �ڵ�
        if (currentscene.name == "LAB16_ver.build_PlayerHP100") //���Ŀ� �� HP�缳��
        {
            playerHP = 100;
        }
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))            // F4 ������...
        {
            ScenePortal(-1);                        // ���� �� ��ŸƮ
        }
        if(Input.GetKeyDown(KeyCode.F5))            // F5 ������...
        {
            ScenePortal(0);                         // ���� �� ��ŸƮ
        }
        else if(Input.GetKeyDown(KeyCode.F6))       // F6 ������...
        {
            ScenePortal(1);                         // ���� �� ��ŸƮ
        }

        //�÷��̾��� HP�� 0�� �Ǹ� ���ӿ��� UI

        if (playerHP <= 0)
        {
            ShowGameOverUI();
        }

        //���� ���� ���¿��� ��Ŭ�������� ���� ���� �ٽ� �����ϱ�
        if (isitOver && Input.GetMouseButtonDown(0)) //isitover���¿��� ��Ŭ����
        {
            isitOver = false; //bool���� Ǯ���ְ�
            RestartGame(); //�ٽ� ����
            //HP100Mode(); //��Ÿ��!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

    }

    

    void ScenePortal(int num)      // �� �̵� ��� ����!
    {
        // ���� �� �ε���(����)�� Ȯ���� ��
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        
        // F4 ~ F6�� ���� �ش� �� �ҷ�����
        // F4(����), F5(����), F6(����)
        SceneManager.LoadScene(currentSceneindex + num);

        // Ŀ�� ���󺹱� (���� �ش� ������ ������ �� Ŀ�� �Ⱥ��̰� ������ ������ �״�� �ݿ���)
        Cursor.lockState = CursorLockMode.Confined;
    }

    void MouseSensitivity()         // ���콺 ���� ���� �Լ�(�̱���)
    {

    }

    public void AddDamage(int hit) //ó���������� �� ���̰� ������ ��������
    {
        playerHP -= hit;
        StartCoroutine(m_Lab16UI.FadeOut());
    }

    public void ShowGameOverUI() //���� ����!
    {
        //�÷��̾� ��������

        //�ϴ� ���� �Ұ��ϰ� ��ũ��Ʈ �м�
        _Y_Player.enabled = false;
        _Y_CanUsePortal.enabled = false;
        _M_GrabMe.enabled = false;
        _Y_CamRotate.enabled = false;

        //��Ż�� ������Ʈ°�� �м�
        portalGun.SetActive(false);

        //�ǰ� UI �м�
        lab16UI.SetActive(false);

        #region ���������� ���߿� ã�� �� ���� �ڵ�
        //�̰� ShowGameOverUI���� ����� ����Լ��� ��� �����Ÿ��鼭 Ȱ��ȭ ��Ȱ��ȭ�� �ݺ��Ѵ�. ��ư �������� �����ҵ�...
        //_Y_Player.enabled = !_Y_Player.enabled; //Ȱ��ȭ ���� ���
        //_Y_CanUsePortal.enabled = !_Y_CanUsePortal.enabled;
        //_M_GrabMe.enabled = !_M_GrabMe.enabled;
        #endregion

        //���� �������� �ϸ� �Ǵµ�...
        deadposition.SetParent(null); //�ڽ� ������Ʈ �� ����� ������ ��۹�� ����.
        Vector3 startpos = player.transform.position; //���� �÷��̾� ��ġ�� startpos�� ����. ������ ������ �������� ����.

        gameoverUI.SetActive(true);
        
        if ( currenttime < collapsetime) //�������� �ð�����!
        {
            currenttime += Time.deltaTime; //Ÿ�̸� �帣��
            float t = currenttime/collapsetime; //�������1
            t = Mathf.Pow(t, 2); //������ ��� ������ �ؼ� ���� ���������� �Ѵ�.
            player.transform.position = Vector3.Lerp(startpos, deadposition.transform.position, t); //��ġ�̵�

            rotZ = Mathf.Lerp(rotZ, -90, t); //z�ุ ���� ���
            player.transform.eulerAngles = new Vector3(0, 0, rotZ); //���� �̵�
           
            
            Rigidbody PlayerRigid = player.gameObject.GetComponent<Rigidbody>(); //����������Ʈ ����ͺ� �ֹ̲�����??
            PlayerRigid.isKinematic = true; //Ű��ƽ ��
        }
        else
        {
            player.transform.position = deadposition.transform.position;
            player.transform.rotation = deadposition.transform.rotation; //���� �����
            //Time.timeScale = 0.0f; //�ð��� �� �帣�� �ϰ����
        }

        isitOver = true;
        
    }

    public void RestartGame()
    {
        //���� ���� �ٽ� �����Ѵ�.
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        
    }
    
    

    public void HP100Mode()
    {
        //���Ŀ�: �߰��� �����ϰ� HP�� 100�� ���� �����Ѵ�.
        Time.timeScale = 1f;
        SceneManager.LoadScene("LAB16_ver.build_PlayerHP100");

    }

}
