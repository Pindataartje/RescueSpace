using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] Collider _attackCollider;

    [SerializeField] float _damage = 0;

    public void InitialzeAttack(float damage)
    {
        _attackCollider = GetComponent<Collider>();
        _attackCollider.enabled = false;

        _damage = damage;
    }

    public void DoAttack()
    {
        _attackCollider.enabled = true;
    }

    public void StopAttack()
    {
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            other.GetComponent<RobotAI>().TakeDamage(_damage);
        }
        if (other.CompareTag("Player"))
        {
            // add player stuff
        }
    }
}
