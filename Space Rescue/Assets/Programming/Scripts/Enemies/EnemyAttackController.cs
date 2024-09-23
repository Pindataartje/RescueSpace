using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAi;

    [SerializeField] ShockWaveController _shockWaveController;

    [SerializeField] EnemyAttackCollider _stompAttackCollider;

    [SerializeField] Collider _stompCollider;

    private void Start()
    {
        _stompAttackCollider.InitialzeAttack(enemyAi.damage);
    }

    public void StartStomp()
    {
        _stompAttackCollider.DoAttack();

        _shockWaveController.Shockwave();
    }

    public void StopStomp()
    {
        _stompAttackCollider.StopAttack();
    }
}
