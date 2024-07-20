using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class M_BulletSound : MonoBehaviour
{
    public AudioSource bulletSource;
    public AudioClip[] turretfire;

    public M_Turret m_Turret;

    public AudioMixer audioMixer;//오디오믹서

    void Start()
    {
        bulletSource = transform.GetComponentInChildren<AudioSource>(); //오디오 컴포넌트 캐싱
    }

    private void Update()
    {
        if (GameManager.gm.isitOver) //게임 오버시 총소리 먹먹하게 들리기
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
            audioMixer.SetFloat("ExposePara", 500); // 500Hz로 낮추어 먹먹한 효과
                                                    //불렛 사운드 오디오믹서도 같이
        }
        else
        {
            audioMixer.SetFloat("ExposePara", 22000); // 원래대로 돌려놓음
        }
    }
}
