using UnityEngine;

public class BlockBot : EnemyAI
{
    public override void GizmosLogic()
    {
        Gizmos.color = Color.blue;

        base.GizmosLogic();
    }
}
