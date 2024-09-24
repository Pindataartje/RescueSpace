using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAi;

    [SerializeField] ShockWaveController _shockWaveController;

    [SerializeField] EnemyAttackCollider _1attackCollider;

    [SerializeField] Animator _animator;


    private void Start()
    {
        _1attackCollider.InitialzeAttack(enemyAi.damage);
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
