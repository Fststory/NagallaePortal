using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public Y_Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Y_Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
