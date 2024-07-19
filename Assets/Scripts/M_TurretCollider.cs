using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class M_TurretCollider : MonoBehaviour
{
    public GameObject turret;

    M_Turret m_turret;

    public Animator turretAnim;

    void Start()
    {
        m_turret = GetComponentInParent<M_Turret>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_turret.noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                if (m_turret.warningTime == 1.0f) //첫 경고인가?
                {
                    m_turret.warning = true; //경고 시작
                    m_turret.warningTime -= Time.deltaTime; //타이머 시작
                }
                else if (m_turret.warningTime > 0) //경고 중인가?
                {
                    m_turret.warningTime -= Time.deltaTime; //타이머 계속
                }
                else if (m_turret.warningTime < 0) //경고시간이 지났다면
                {
                    m_turret.shooting = true;
                    //turretAnim.SetTrigger("Attack"); //여기서 해야 한 번만 애니메이팅 //아진짜문제많네
                    m_turret.shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
                }

                //shooting = true;
                //shootTime = 2.0f; //사격시간을 계속 업데이트하면서 총쏘게만들기
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!m_turret.noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                //사격 이전에 나갔을 때 발동될 명령어
                if (!m_turret.shooting && m_turret.warningTime < 1.0f)
                {
                    m_turret.warningTime += Time.deltaTime;
                }
            }
        }
    }
}
