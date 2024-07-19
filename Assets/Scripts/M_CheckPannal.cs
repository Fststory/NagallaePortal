using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CheckPannal : MonoBehaviour
{
    public GameObject BBig;
    M_DoorButtonBig_another m_DoorButtonBig_Another;

    public GameObject pannal;

    public Material activateC;
    public Material deactivateC;

    private Renderer lineRenderer;

    void Start()
    {
        m_DoorButtonBig_Another = BBig.GetComponent<M_DoorButtonBig_another>();

        if (pannal != null)
        {
            lineRenderer = pannal.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("guideline �������� �ʾҽ��ϴ�.");
        }

        // �ʱ� Material ����
        UpdateMaterial();

    }

    void Update()
    {
        UpdateMaterial(); //���¸��� ����
    }

    private void UpdateMaterial() //���¸��� �ٲ��ִ� �Լ� (��ó: GPT)
    {
        if (lineRenderer != null)
        {
            if (m_DoorButtonBig_Another.isdooropen)
            {
                lineRenderer.material = activateC;
            }
            else
            {
                lineRenderer.material = deactivateC;
            }
        }
    }

}
