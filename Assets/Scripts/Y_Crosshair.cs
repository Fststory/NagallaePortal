using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Y_Crosshair : MonoBehaviour
{
    [SerializeField]
    private Y_PortalPair portalPair;      // ũ�ν��� ��ġ ���θ� �Ǵ��� ��Ż ���

    [SerializeField]
    private Image inPortalImg;          // �� ��Ż(0��,�Ķ�) ��ġ ���θ� �˷��ִ� UI �̹���

    [SerializeField]
    private Image outPortalImg;         // �ƿ� ��Ż(1��,��Ȳ) ��ġ ���θ� �˷��ִ� UI �̹���

    private void Start()
    {
        var portals = portalPair.Portals;

        // �� ��Ż�� ������ ������ ��´�.
        inPortalImg.color = portals[0].PortalColor;
        outPortalImg.color = portals[1].PortalColor;

        // ��Ż ���� ������ ��Ȱ��ȭ
        inPortalImg.gameObject.SetActive(false);
        outPortalImg.gameObject.SetActive(false);
    }

    public void SetPortalPlaced(int portalNum, bool isPlaced)
    {
        // 0�� ��Ż�� ��ġ�Ǿ� �ִٸ�
        if (portalNum == 0)
        {
            // �Ķ� ��Ż ���� ���� UI �̹����� Ȱ��ȭ�Ѵ�. 
            inPortalImg.gameObject.SetActive(isPlaced);
        }
        // 1�� ��Ż�� ��ġ�Ǿ� �ִٸ�
        else
        {
            // ��Ȳ ��Ż ���� ���� UI �̹����� Ȱ��ȭ�Ѵ�. 
            outPortalImg.gameObject.SetActive(isPlaced);
        }
    }
}
