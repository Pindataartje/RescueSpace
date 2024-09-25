using System.Collections;
using UnityEngine;

public class MeleeBot : RobotAI
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    public override void CheckState()
    {
        base.CheckState();
    }


    public override IEnumerator Attacking()
    {
        if (Target != null)
        {
            Target.GetComponent<Entity>().TakeDamage(RobotInfo.damage); // this still happens with this because it tries to attack the player ( probably )

            WeaponAnimator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(RobotInfo.fireRate);

        if (IsAttacking)
        {
            StartCoroutine(Attacking());
        }
    }

}
