using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Y_Crosshair : MonoBehaviour
{
    [SerializeField]
    private Y_PortalPair portalPair;      // 크로스헤어가 설치 여부를 판단할 포탈 듀오

    [SerializeField]
    private Image inPortalImg;          // 인 포탈(0번,파랑) 설치 여부를 알려주는 UI 이미지

    [SerializeField]
    private Image outPortalImg;         // 아웃 포탈(1번,주황) 설치 여부를 알려주는 UI 이미지

    private void Start()
    {
        var portals = portalPair.Portals;

        // 각 포탈의 색깔을 변수에 담는다.
        inPortalImg.color = portals[0].PortalColor;
        outPortalImg.color = portals[1].PortalColor;

        // 포탈 생성 전에는 비활성화
        inPortalImg.gameObject.SetActive(false);
        outPortalImg.gameObject.SetActive(false);
    }

    public void SetPortalPlaced(int portalNum, bool isPlaced)
    {
        // 0번 포탈이 설치되어 있다면
        if (portalNum == 0)
        {
            // 파랑 포탈 생성 여부 UI 이미지를 활성화한다. 
            inPortalImg.gameObject.SetActive(isPlaced);
        }
        // 1번 포탈이 설치되어 있다면
        else
        {
            // 주황 포탈 생성 여부 UI 이미지를 활성화한다. 
            outPortalImg.gameObject.SetActive(isPlaced);
        }
    }
}
