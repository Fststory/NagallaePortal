using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PortalPair : MonoBehaviour
{
    public Y_Portal[] Portals { private set; get; }       // ��Ż [0],[1] �迭

    private void Awake()
    {
        Portals = GetComponentsInChildren<Y_Portal>();    // ��Ż [0],[1]�� PortalPair ������Ʈ�� �ڽ����� �д�.

        if (Portals.Length != 2)     // ���� ��Ż�� ������ 2���� �ƴ϶��
        {
            // �ֿܼ� �α׸� ��� ����� �� ���̴�.
            Debug.LogError("PortalPair children must contain exactly two Y_Portal components in total.");
        }
    }
}
