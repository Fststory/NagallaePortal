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

    //(7/4) 만들고 보니 아직 이걸 만들 필요가 없어서 기본적인 뼈대만 세워두고 방치중!

}
