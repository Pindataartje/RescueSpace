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

    public void DoRandomAttack()
    {
        _animator.SetTrigger("Attack"); // stomp
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
}
