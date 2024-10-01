using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] EnemyAI _enemyAi;

    [SerializeField] ShockWaveController _shockWaveController;

    [SerializeField] EnemyAttackCollider _1attackCollider;

    [SerializeField] Animator _animator;


    private void Start()
    {
        _enemyAi = GetComponentInParent<EnemyAI>();

        if (_1attackCollider != null)
        {
            _1attackCollider.InitialzeAttack(_enemyAi.damage);
        }
    }

    public void PowerOn()
    {
        _enemyAi.HasPoweredOn = true;
    }

    public void DoRandomAttack()
    {
        _animator.SetTrigger("Attack"); // stomp
    }

    public void CrushStart()
    {
        _1attackCollider.DoAttack();
    }

    public void CrushStop()
    {
        _1attackCollider.StopAttack();
    }

    public void StartNeck()
    {
        _1attackCollider.DoAttack();
    }

    public void StopNeck()
    {
        _1attackCollider.StopAttack();
        _enemyAi.IsAttacking = false;
    }

    public void StartStomp()
    {
        _1attackCollider.DoAttack();

        _shockWaveController.Shockwave();
    }

    public void StopStomp()
    {
        _1attackCollider.StopAttack();
    }

    public void HasPoweredOn()
    {
        _enemyAi.HasPoweredOn = true;
    }

}
