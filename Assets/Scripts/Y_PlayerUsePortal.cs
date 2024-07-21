using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlayerUsePortal : Y_CanUsePortal
{
    private Y_CamRotate camRotate;


    protected override void Awake()
    {
        base.Awake();

        camRotate = GetComponent<Y_CamRotate>();
    }

    public override void Warp()
    {
        base.Warp();
        camRotate.ResetTargetRotation();
    }
}
