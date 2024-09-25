using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : Entity
{
    [SerializeField] RobotSO _robotInfo;
    public RobotSO RobotInfo
    { get { return _robotInfo; } }

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Animator _bodyAnimator;
    public Animator BodyAnimator
    { get { return _bodyAnimator; } }
    [SerializeField] Animator _weaponAnimator;
    public Animator WeaponAnimator
    { get { return _weaponAnimator; } }

    [SerializeField] Transform _target;
    public Transform Target
    { get { return _target; } }

    [SerializeField] Transform _player;

    [SerializeField] Collider _collider;

    [SerializeField] Transform _scrapRobot;

    [SerializeField] bool _isAttacking;
    public bool IsAttacking
    { get { return _isAttacking; } }

    [SerializeField] float _distanceFromTarget;

    [SerializeField] float _checkRadius;
    [SerializeField] float _groundedCheckRadius;
    [SerializeField] LayerMask _throwLayer;

    [SerializeField] LayerMask _groundMask;

    [SerializeField] bool _isThrown;

    [SerializeField] GameObject _deathEffectPrefab;


    public override void Start()
    {
        if (_robotInfo != null)
        {
            _agent.speed = _robotInfo.speed;
        }

        _player = FindAnyObjectByType<PlayerController>().transform;

        _scrapRobot = FindAnyObjectByType<ScrapRobot>().transform;

        _bodyAnimator = GetComponentInChildren<Animator>();
    }

    public override void Update()
    {
        CheckState();
    }

    public override void Death()
    {
        GameObject effect = Instantiate(_deathEffectPrefab);

        effect.transform.position = transform.position;

        Destroy(effect, 1f);

        base.Death();
    }

    public virtual void InitializeRobotInfo()
    {
        if (_robotInfo != null)
        {
            maxHealth = _robotInfo.health;
            health = maxHealth;

            speed = _robotInfo.speed;
            if (_agent != null) { _agent.speed = speed; }

            damage = _robotInfo.damage;

            // _regenRate = _robotInfo.regenRate;
            // _regenAmount = _robotInfo.regenAmount;

            // _patrolRadius = _robotInfo.patrolRadius;
            // _patrolReturnDistance = _robotInfo.patrolReturnDistance;
            // _patrolPointCount = _robotInfo.patrolPoints;

            // _searchRadius = _robotInfo.searchRadius;
            // _timeToSearch = _robotInfo.timeToSearch;
            // _searchCount = _robotInfo.searchCount;
        }
    }

    public State _currentState;

    public enum State
    {
        IDLE,
        FOLLOW,
        ATTACK,
        THROWN,
        GATHER,
        ATTACHED,
    }

    public virtual void ChangeState(State newState)
    {
        switch (_currentState)
        {
            case State.IDLE:
                StopIdle();
                break;
            case State.FOLLOW:
                StopFollow();
                break;
            case State.ATTACK:
                StopAttack();
                break;
            case State.THROWN:
                StopThrown();
                break;
            case State.GATHER:
                StopGather();
                break;
            case State.ATTACHED:
                StopAttached();
                break;
            default:
                Debug.Log($"{newState} is not supported on this robot");
                break;
        }

        switch (newState)
        {
            case State.IDLE:
                StartIdle();
                break;
            case State.FOLLOW:
                StartFollow();
                break;
            case State.ATTACK:
                StartAttack();
                break;
            case State.THROWN:
                StartThrown();
                break;
            case State.GATHER:
                StartGather();
                break;
            case State.ATTACHED:
                StartAttached();
                break;
            default:
                Debug.Log($"{newState} is not supported on this robot");
                break;
        }
    }

    public virtual void CheckState()
    {
        switch (_currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.FOLLOW:
                Follow();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.THROWN:
                Thrown();
                break;
            case State.GATHER:
                Gather();
                break;
            case State.ATTACHED:
                Attached();
                break;
        }
    }

    #region Idle

    public virtual void StartIdle()
    {
        _currentState = State.IDLE;
    }

    public virtual void Idle()
    {

    }

    public virtual void StopIdle()
    {

    }

    #endregion

    #region Follow

    public virtual void StartFollow()
    {
        _currentState = State.FOLLOW;

        _target = _player.GetComponent<PlayerController>().restSpot;

        _agent.enabled = true;

        _agent.stoppingDistance = _robotInfo.followDistance;
    }

    public virtual void Follow()
    {
        _agent.SetDestination(_target.position);

        Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

        _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);

        if (_agent.stoppingDistance >= _distanceFromTarget && !_agent.isStopped)
        {
            _agent.isStopped = true;
            _bodyAnimator.SetBool("Walking", false);
        }
        else
        {
            _agent.isStopped = false;
            _bodyAnimator.SetBool("Walking", true);
        }
    }

    public virtual void StopFollow()
    {

    }

    #endregion

    #region Attack

    public virtual void StartAttack()
    {
        _currentState = State.ATTACK;

        _agent.stoppingDistance = _robotInfo.range;

        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    public virtual void Attack()
    {
        // if (_agent.isActiveAndEnabled)
        // {
        //     _agent.SetDestination(GameObject.FindGameObjectWithTag("Enemy").transform.position);
        // }


        if (_target != null)
        {
            _distanceFromTarget = Vector3.Distance(transform.position, _target.position);
            transform.LookAt(_target);
        }


        if (_agent.stoppingDistance > _distanceFromTarget && !_isAttacking)
        {
            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = true;
            }

            StartCoroutine(StartAttacking());
        }
        else
        {
            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = false;
            }
        }
    }

    public virtual void StopAttack()
    {
        StartCoroutine(StopAttacking());
    }

    public virtual IEnumerator StartAttacking()
    {
        _isAttacking = true;

        _weaponAnimator.SetBool("Ready", true);

        yield return new WaitForSeconds(_robotInfo.windUpTime);

        StartCoroutine(Attacking());
    }

    public virtual IEnumerator Attacking() // make sure it stops attacking before changing state
    {
        if (_target != null)
        {
            _target.GetComponent<Entity>().TakeDamage(_robotInfo.damage); // this still happens with this because it tries to attack the player ( probably )

            _bodyAnimator.SetTrigger("Attack");
            _weaponAnimator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(_robotInfo.fireRate);

        if (_isAttacking)
        {
            StartCoroutine(Attacking());
        }
    }

    public virtual IEnumerator StopAttacking()
    {
        _weaponAnimator.SetBool("Ready", false);

        yield return new WaitForSeconds(_robotInfo.windDownTime);
        _isAttacking = false;
    }

    #endregion

    #region Thrown

    public virtual void StartThrown()
    {
        _currentState = State.THROWN;

        _agent.enabled = false;
        _collider.isTrigger = true;

        _isThrown = true;
    }

    public virtual void Thrown()
    {
        if (_isThrown)
        {
            CheckForEntityInRange(_checkRadius);
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f, _groundMask))
        {
            Debug.Log("Hit Ground");
            ChangeState(State.IDLE);
            CheckForEntityInRange(_groundedCheckRadius);
        }
    }

    public virtual void StopThrown()
    {
        _agent.enabled = true;
        _collider.isTrigger = false;
    }

    public virtual void CheckForEntityInRange(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, _throwLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<EnemyAI>(out EnemyAI enemy))
            {
                if (colliders[i].GetComponent<EnemyAI>().isAlive)
                {
                    Debug.Log("ENEMY DETECTED");

                    _target = colliders[i].transform;

                    ChangeState(State.ATTACK);

                    break;
                }
                else
                {
                    _target = colliders[i].transform;

                    ChangeState(State.GATHER);

                    break;
                }
            }
            else if (colliders[i].TryGetComponent<Scrap>(out Scrap scrap))
            {
                _target = colliders[i].GetComponent<Scrap>().transform;

                ChangeState(State.GATHER);

                break;
            }

        }
    }

    #endregion

    #region Gather

    public virtual void StartGather()
    {
        _currentState = State.GATHER;

        GetNewGatherTargetPosition();

        _agent.stoppingDistance = 0.1f;
    }

    public virtual void Gather()
    {
        if (_agent.isActiveAndEnabled)
        {
            _agent.SetDestination(_target.position);
        }

        Vector3 targetWithOffset = new Vector3(_target.position.x, transform.position.y, _target.position.z);

        _distanceFromTarget = Vector3.Distance(transform.position, targetWithOffset);

        if (_agent.stoppingDistance >= _distanceFromTarget && _agent.isActiveAndEnabled && !_agent.isStopped)
        {
            Debug.Log("Stop");

            _target.GetComponentInParent<Scrap>().AddRobot(this);

            _agent.enabled = false;

            transform.SetParent(_target);
        }
    }

    public virtual void StopGather()
    {
        transform.SetParent(null);

        if (_target != null)
        {
            _target.GetComponentInParent<Scrap>().RemoveRobot(this);
        }

        _agent.enabled = true;
    }

    public virtual void GetNewGatherTargetPosition()
    {
        _target = _target.GetComponent<Scrap>().GetGatherPosition(this);

        if (_target == null)
        {
            ChangeState(State.IDLE);
        }
    }

    public void ChangeGatherTarget()
    {
        _agent.enabled = true;

        _target.GetComponentInParent<Scrap>().RemoveRobot(this);

        _target = _target.GetComponentInParent<Scrap>().GetGatherPosition(this);
    }

    #endregion

    #region Attached

    public virtual void StartAttached()
    {
        _currentState = State.ATTACHED;

        transform.SetParent(_target.transform, true);
    }

    public virtual void Attached()
    {

    }

    public virtual void StopAttached()
    {
        transform.SetParent(null);

        _collider.isTrigger = false;

        _agent.enabled = true; // maybe when hits the floor
    }

    #endregion

    public virtual void CollectScrapAtBase()
    {
        ChangeState(State.IDLE);
    }

    public void OnValidate()
    {
        InitializeRobotInfo();
    }
}
