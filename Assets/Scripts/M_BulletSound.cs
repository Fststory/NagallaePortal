using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class M_BulletSound : MonoBehaviour
{
    public AudioSource bulletSource;
    public AudioClip[] turretfire;

    public M_Turret m_Turret;

    public AudioMixer audioMixer;//������ͼ�

    void Start()
    {
        bulletSource = transform.GetComponentInChildren<AudioSource>(); //����� ������Ʈ ĳ��
    }

    private void Update()
    {
        if (GameManager.gm.isitOver) //���� ������ �ѼҸ� �Ը��ϰ� �鸮��
        {
            MuffleSound(true);
        }
        else
        {
            MuffleSound(false);
        }
    }

    public void FireSound()
    {
        m_Turret.Randompick();
        bulletSource.clip = turretfire[m_Turret.randomrate2];
        bulletSource.volume = 0.7f;
        bulletSource.Play();
    }

    void MuffleSound(bool muffled)
    {
        if (muffled)
        {
            audioMixer.SetFloat("ExposePara", 500); // 500Hz�� ���߾� �Ը��� ȿ��
                                                    //�ҷ� ���� ������ͼ��� ����
        }
        else
        {
            audioMixer.SetFloat("ExposePara", 22000); // ������� ��������
        }
    }
}
