using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Elevator : MonoBehaviour
{
    //public Camera cam;
    Vector3 ElevatorOriginalPos;
    //ī�޶� ���� ���������͸� �����

    public Animator AnimController; //�ִϸ����� ����
    public GameObject frontCollider; //��¦ �ݶ��̴�

    bool alreadyDoit = false;

    public bool letsgoNext = false;

    //float spd = 0.7f;
    public float countShakeTime = 1.0f;
    public float countOpentime = 1.7f;

    public float elevatorstartMoveTime = 2.0f; //���������� �������
    bool justonce = false;
    float nextlevel = 7.0f; //���� ������ ī��Ʈ

    public bool playerGoal = false; //�÷��̾� Ž�� ���� �ݶ��̴����� ��ũ��Ʈ �־ �����ϱ�

    public float waitForplayer = 1.0f;

    #region ����
    public AudioSource audioSourse;

    public AudioClip[] Elevator;
    #endregion


    void Start()
    {
        //cam = Camera.main;
        //cameraOriginalPos = cam.transform.position;

        ElevatorOriginalPos = gameObject.transform.position; //���� �ڸ� �����ϱ�
        print(ElevatorOriginalPos);

        AnimController.enabled = false;

        audioSourse = transform.GetComponent<AudioSource>(); //����� ������Ʈ
    }

    void Update()
    {
        #region �����Ҷ� ���������� ����
        if (!alreadyDoit && countOpentime > 0)
        {
            countOpentime -= Time.deltaTime;
        }
        else if (!alreadyDoit && countOpentime < 0)
        {
            StartCoroutine(CameraShake(0.3f, 0.3f)); //���鼭
            AnimController.enabled = true; //�������ְ�
            frontCollider.SetActive(false); //�ݶ��̴� ����
            audioSourse.PlayOneShot(Elevator[0]); //ù ���� �ѹ���
            alreadyDoit = true;
        }
        #endregion

        if (playerGoal)
        {
            if (waitForplayer > 0)
            {
                waitForplayer -= Time.deltaTime; //�� 1�� ��ٷ��ش�.
            }
            else
            {
                frontCollider.SetActive(true); //���� ����������.
                StartCoroutine(CameraShake(0.3f, 0.2f)); //���鼭
                AnimController.SetTrigger("PlayerGoal");
                CloseDoor(); //�� �ݴ� �Ҹ�;
                letsgoNext = true;
                playerGoal = false;
            }
        }

        if (letsgoNext)
        {
            elevatorstartMoveTime -= Time.deltaTime; //���� ������ Ʋ�����...
            nextlevel -= Time.deltaTime; //���� ������ �������...

            if (elevatorstartMoveTime < 0 && !justonce)
            {
                StartCoroutine(CameraShake(0.7f, 0.2f)); //��? �����
                StartMoving(); //���� Ʋ�鼭...
                justonce = true; //�� ����.
            }

            if (nextlevel < 0)
            {
                print("���� ��!");
                GameManager.gm.Ending();
                //���� ���ӸŴ������� ���� �� �ҷ�����.
                letsgoNext = false;
            }

        }

    }

    //void OpenTheDoor()
    //{
    //    RightDoor.transform.eulerAngles = Vector3.Lerp(RightDoor.transform.eulerAngles, RightDoorOpen.transform.eulerAngles, spd * Time.deltaTime);
    //    LeftDoor.transform.eulerAngles = Vector3.Lerp(LeftDoor.transform.eulerAngles, LeftDoorOpen.transform.eulerAngles, spd * Time.deltaTime);
    //}


    IEnumerator CameraShake(float duration, float magnitude) //ī�޶� ��鸮�� �ϴ� �ڷ�ƾ, float �۵��ð�, float �۵�����
    {
        float timer = 0;

        while (timer <= duration)
        {
            gameObject.transform.position = Random.insideUnitSphere * magnitude + ElevatorOriginalPos;
            timer += Time.deltaTime; //�ð� �帣��
            yield return null;
        }

        gameObject.transform.position = ElevatorOriginalPos; //ī�޶� ����ġ
    }

    public void CloseDoor()
    {
        audioSourse.clip = Elevator[1];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }
    public void StartMoving()
    {
        audioSourse.clip = Elevator[2];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }

}
