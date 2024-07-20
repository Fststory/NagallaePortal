using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_DoorButtonBig_another : MonoBehaviour
{
    public bool isdooropen;

    //Lerp�� ����� ����.

    //�� ������Ʈ
    public GameObject DoorLeft;
    public GameObject DoorRight;

    //Door Lerp 
    public Transform leftOpen;
    public Transform rightOpen;
    public Transform leftClose;
    public Transform rightClose;

    //��ư ������Ʈ
    public GameObject buttonPart;

    //button Lerp
    public Transform buttonDeactive;
    public Transform buttonActive;

    public float spd = 3.0f;

    #region ���̵���� ���¸��� ����

    public GameObject guideline;

    public Material activate;
    public Material deactivate;

    private Renderer lineRenderer;

    #endregion

    #region ����� ����
    public AudioSource audioSourse;
    public AudioClip[] buttonSounds;
    #endregion

    bool activateSound = true;

    bool deactiveSound = false;


    void Start()
    {
        audioSourse = transform.GetComponent<AudioSource>(); //����� ������Ʈ ĳ��
        #region ���̵���� ������Ʈ�� Renderer ������Ʈ ��������

        if (guideline != null)
        {
            lineRenderer = guideline.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("guideline �������� �ʾҽ��ϴ�.");
        }

        // �ʱ� Material ����
        UpdateMaterial();
        #endregion
    }

    void Update()
    {

        if (isdooropen)
        {
            ButtonDOWN(); //��ư ������
            Open(); //�� ����
            
            if (activateSound) //���尡 �� ���� �� �����ٸ�
            {
                ButActivate();//���� �Ѱ�
                activateSound = false; //�״ٰ� ǥ���ϰ�
                deactiveSound = true; //�̷��� �ؼ� ���� ���带 ����ϱ�
            }
        }
        else
        {
            ButtonUP(); //��ư ������
            Close(); //�� ����

            if (deactiveSound)
            {
                ButDeactivate();
                deactiveSound=false;
                activateSound = true;
            }
        }

        UpdateMaterial(); //���¸��� ����
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
           isdooropen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            isdooropen = false;
        }
    }

    private void UpdateMaterial() //���¸��� �ٲ��ִ� �Լ� (��ó: GPT)
    {
        if (lineRenderer != null)
        {
            if (isdooropen)
            {
                lineRenderer.material = activate;
            }
            else
            {
                lineRenderer.material = deactivate;
            }
        }
    }

    void ButtonUP()
    {
        buttonPart.transform.position = Vector3.Lerp(buttonPart.transform.position, buttonActive.position, spd * Time.deltaTime);
    }
    void ButtonDOWN()
    {
        buttonPart.transform.position = Vector3.Lerp(buttonPart.transform.position, buttonDeactive.position, spd * Time.deltaTime);
    }
    void Close()
    {
        
        DoorLeft.transform.position = Vector3.Lerp(DoorLeft.transform.position, leftClose.position, spd*Time.deltaTime);
        DoorRight.transform.position = Vector3.Lerp(DoorRight.transform.position, rightClose.position, spd * Time.deltaTime);
    }
    void Open()
    {
        DoorLeft.transform.position = Vector3.Lerp(DoorLeft.transform.position, leftOpen.position, spd * Time.deltaTime);
        DoorRight.transform.position = Vector3.Lerp(DoorRight.transform.position, rightOpen.position, spd * Time.deltaTime);
    }

    public void ButActivate()
    {
        audioSourse.clip = buttonSounds[0];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }

    public void ButDeactivate()
    {
        audioSourse.clip = buttonSounds[1];
        audioSourse.volume = 0.7f;
        audioSourse.Play();
    }


}
