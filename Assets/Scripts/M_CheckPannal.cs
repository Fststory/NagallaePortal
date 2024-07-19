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
            Debug.LogError("guideline 설정되지 않았습니다.");
        }

        // 초기 Material 설정
        UpdateMaterial();

    }

    void Update()
    {
        UpdateMaterial(); //머태리얼 갈기
    }

    private void UpdateMaterial() //머태리얼 바꿔주는 함수 (출처: GPT)
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
