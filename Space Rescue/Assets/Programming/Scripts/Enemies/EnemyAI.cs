using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Entity
{
    [SerializeField] EnemySO _enemyInfo;

    [SerializeField] Animator _animator;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] float _distanceFromTarget;

    [SerializeField] Transform _target;

    [SerializeField] int _patrolPointCount;

    [SerializeField] float _patrolRadius;

    [SerializeField] NavMeshHit _navmeshHit;

    [SerializeField] int _currentPatrol;

    [SerializeField] List<Transform> _patrolPoints = new();

    public bool isAlive;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        StartCoroutine(Damage());
    }

    public override void Start()
    {
        health = _enemyInfo.health;

        if (health > 0)
        {
            isAlive = true;
        }

        GeneratePatrol(_patrolPointCount);

        _target = _patrolPoints[0];
    }

    public override void Update()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);

            Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

            _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);
        }

        if (_agent.stoppingDistance >= _distanceFromTarget && !_agent.isStopped)
        {
            _currentPatrol++;
            if (_currentPatrol >= _patrolPointCount)
            {
                _currentPatrol = 0;
            }
            _target = _patrolPoints[_currentPatrol];
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.2f);
    }

    public virtual void GeneratePatrol(int patrolPoints)
    {
        float angleStep = 360f / patrolPoints;
        float randomOffsetRadius = 2f;

        GameObject holdObj = new GameObject("Hold_Points");

        for (int i = 0; i < patrolPoints; i++)
        {
            bool validPositionFound = false;

            while (!validPositionFound)
            {
                float angle = i * angleStep;

                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _patrolRadius;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad) * _patrolRadius;
                Vector3 basePosition = new Vector3(x, 0, z) + transform.position;

                Vector3 randomOffset = Random.insideUnitSphere * randomOffsetRadius;
                randomOffset.y = 0;
                Vector3 randomPosition = basePosition + randomOffset;

                if (NavMesh.SamplePosition(randomPosition, out _navmeshHit, _patrolRadius, NavMesh.AllAreas))
                {
                    validPositionFound = true;

                    GameObject patrolObj = new GameObject($"Hold_Pos ({i})");

                    patrolObj.transform.position = _navmeshHit.position;
                    patrolObj.transform.parent = transform;

                    patrolObj.transform.SetParent(holdObj.transform);

                    _patrolPoints.Add(patrolObj.transform);
                }
            }
        }
    }

    [SerializeField] float _gizmoSize;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _patrolPoints.Count; i++)
        {
            Gizmos.color = Color.yellow;

            foreach (Transform position in _patrolPoints)
            {
                if (position != null)
                {
                    Gizmos.DrawSphere(position.position, _gizmoSize);
                }
            }
        }
    }
}
