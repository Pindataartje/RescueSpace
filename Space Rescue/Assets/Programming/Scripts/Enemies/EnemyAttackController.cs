using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] EnemyAI _enemyAi;

    [SerializeField] ShockWaveController _shockWaveController;

    [SerializeField] EnemyAttackCollider[] _attacks;

    [SerializeField] string[] _attackNames;

    [SerializeField] Animator _animator;


    private void Start()
    {
        _enemyAi = GetComponentInParent<EnemyAI>();

        for (int i = 0; i < _attacks.Length; i++)
        {
            if (_attacks[i] != null)
            {
                _attacks[i].InitialzeAttack(_enemyAi.damage);
            }
        }
    }

    public void PowerOn()
    {
        _enemyAi.HasPoweredOn = true;
        _enemyAi.GetComponent<TurtleBot>().LedRenderer.material = _enemyAi.GetComponent<TurtleBot>().OnLed;
    }

    public void Die()
    {
        _enemyAi.AfterDeath();
    }

    public void DoRandomAttack()
    {
        _animator.SetTrigger(_attackNames[Random.Range(0, _attackNames.Length)]);
    }

    public void StopAttackAnimation()
    {
        _enemyAi.IsAttacking = false;
    }

    #region Block

    public void SmashStart()
    {
        _attacks[1].DoAttack();
    }

    public void SmashStop()
    {
        _attacks[1].StopAttack();
    }

    #endregion


    #region Turtle

    public void StartNeck()
    {
        _attacks[0].DoAttack();
    }

    public void StopNeck()
    {
        _attacks[0].StopAttack();
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

    public void HasPoweredOn()
    {
        _enemyAi.HasPoweredOn = true;
    }

    #endregion

    public void CrushStart()
    {
        _attacks[0].DoAttack();
    }

    public void CrushStop()
    {
        _attacks[0].StopAttack();
    }

    #region Wolf

    public void StartStomp()
    {
        _attacks[0].DoAttack();

        _shockWaveController.Shockwave();
    }

    public void StopStomp()
    {
        _attacks[0].StopAttack();
    }

    public void LungeStart()
    {
        _attacks[1].DoAttack();
    }

    public void LungeStop()
    {
        _attacks[1].StopAttack();
    }

    #endregion


}
