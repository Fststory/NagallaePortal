using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PortalPair : MonoBehaviour
{
    public Y_Portal[] Portals { private set; get; }       // 포탈 [0],[1] 배열

    private void Awake()
    {
        Portals = GetComponentsInChildren<Y_Portal>();    // 포탈 [0],[1]을 PortalPair 오브젝트의 자식으로 둔다.

        if (Portals.Length != 2)     // 만약 포탈의 갯수가 2개가 아니라면
        {
            // 콘솔에 로그를 띄워 디버그 할 것이다.
            Debug.LogError("PortalPair children must contain exactly two Y_Portal components in total.");
        }
    }
}
