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
                if (m_turret.warningTime == 1.0f) //ù ����ΰ�?
                {
                    m_turret.warning = true; //��� ����
                    m_turret.warningTime -= Time.deltaTime; //Ÿ�̸� ����
                }
                else if (m_turret.warningTime > 0) //��� ���ΰ�?
                {
                    m_turret.warningTime -= Time.deltaTime; //Ÿ�̸� ���
                }
                else if (m_turret.warningTime < 0) //���ð��� �����ٸ�
                {
                    m_turret.shooting = true;
                    //turretAnim.SetTrigger("Attack"); //���⼭ �ؾ� �� ���� �ִϸ����� //����¥��������
                    m_turret.shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
                }

                //shooting = true;
                //shootTime = 2.0f; //��ݽð��� ��� ������Ʈ�ϸ鼭 �ѽ�Ը����
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!m_turret.noshoot)
        {
            if (other.gameObject.name == "Player")
            {
                //��� ������ ������ �� �ߵ��� ��ɾ�
                if (!m_turret.shooting && m_turret.warningTime < 1.0f)
                {
                    m_turret.warningTime += Time.deltaTime;
                }
            }
        }
    }
}
