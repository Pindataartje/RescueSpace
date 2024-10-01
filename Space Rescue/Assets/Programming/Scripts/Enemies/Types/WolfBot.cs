using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfBot : EnemyAI
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void GizmosLogic()
    {
        Gizmos.color = Color.red;
        base.GizmosLogic();
    }
}
