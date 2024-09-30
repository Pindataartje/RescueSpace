using UnityEngine;

public class TurtleBot : EnemyAI
{
    [SerializeField] Renderer _ledRenderer;
    [SerializeField] Material _onLed;
    [SerializeField] Material _offLed;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (HasPoweredOn)
        {
            _ledRenderer.material = _onLed;
            CheckState();
        }
        else
        {
            _ledRenderer.material = _offLed;
        }
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    public override void CheckState()
    {
        base.CheckState();
    }

    #region Idle

    public override void StartIdle()
    {
        base.StartIdle();
    }

    public override void Idle()
    {
        base.Idle();
    }

    public override void StopIdle()
    {
        base.StopIdle();
    }

    #endregion

    #region Patrol

    public override void StartPatrol()
    {
        base.StartPatrol();
    }

    public override void Patrol()
    {
        base.Patrol();
    }

    public override void StopPatrol()
    {
        base.StopPatrol();
    }

    #endregion

    #region Chase

    public override void StartChase()
    {
        base.StartChase();
    }

    public override void Chase()
    {
        base.Chase();
    }

    public override void StopChase()
    {
        base.StopChase();
    }

    #endregion

    #region Search

    public override void StartSearch()
    {
        base.StartSearch();
    }

    public override void Search()
    {
        base.Search();
    }

    public override void StopSearch()
    {
        base.StopSearch();
    }

    public override void GetRandomSearchPosition()
    {
        base.GetRandomSearchPosition();
    }

    public override void EnterDetection(Transform newDetected)
    {
        base.EnterDetection(newDetected);

        Animator.SetBool("Power", true);
    }

    public override void Detection()
    {
        base.Detection();
    }

    public override void ExitDetection()
    {
        base.ExitDetection();
    }

    #endregion

    #region Attack

    public override void StartAttack()
    {
        base.StartAttack();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void GeneratePatrol(int patrolPoints)
    {
        base.GeneratePatrol(patrolPoints);
    }

    public override void Regeneration(float amount)
    {
        base.Regeneration(amount);
    }
}
