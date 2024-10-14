using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaboomBot : RobotAI
{
    public override void StartAttack()
    {
        base.StartAttack();

        WeaponAnimator.SetTrigger("Ready");
    }

    public override void Attack()
    {
        if (Target != null)
        {
            Agent.SetDestination(Target.position);

            Vector3 targetWithOffset = new Vector3(Target.position.x, transform.position.y, Target.position.z);

            DistanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);
        }

        if (Agent.stoppingDistance > DistanceFromTarget && !IsAttacking)
        {
            // Kaboom rico?

            // Yes rico kaboom
        }
    }
}
