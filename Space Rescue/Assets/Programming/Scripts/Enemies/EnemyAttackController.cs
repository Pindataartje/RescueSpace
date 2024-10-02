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
        _enemyAi.GetComponent<TurtleBot>().LedRenderer.material = _enemyAi.GetComponent<TurtleBot>().OnLed;
    }

    public void Hide()
    {
        _enemyAi.GetComponent<TurtleBot>().IsHiding = true;
        _enemyAi.GetComponent<TurtleBot>().LedRenderer.material = _enemyAi.GetComponent<TurtleBot>().OffLed;
    }

    public void UnHide()
    {
        _enemyAi.GetComponent<TurtleBot>().IsHiding = false;
        _enemyAi.GetComponent<TurtleBot>().LedRenderer.material = _enemyAi.GetComponent<TurtleBot>().OnLed;
    }

    public void DoRandomAttack()
    {
        _animator.SetTrigger("Attack"); // stomp
    }

    public void SmashStart()
    {
        _1attackCollider.DoAttack();
    }

    public void SmashStop()
    {
        _1attackCollider.StopAttack();
        _enemyAi.IsAttacking = false;
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
        _enemyAi.IsAttacking = false;
    }

    public void HasPoweredOn()
    {
        _enemyAi.HasPoweredOn = true;
    }

}
