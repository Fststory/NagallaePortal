using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }

    //(7/4) ����� ���� ���� �̰� ���� �ʿ䰡 ��� �⺻���� ���븸 �����ΰ� ��ġ��!

}
