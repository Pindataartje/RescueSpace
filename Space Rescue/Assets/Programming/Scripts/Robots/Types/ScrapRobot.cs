using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ScrapRobot : RobotAI
{
    [SerializeField] float _spawnTime;

    [SerializeField] GameObject[] _robotPrefabs;

    [SerializeField] Transform _spawnPosition;

    [SerializeField] Transform _grabBone;

    [SerializeField] Transform _grabPosition;
    public Transform GrabPosition
    { get { return _grabBone; } }

    [SerializeField] Transform _scrapToCollect;

    [SerializeField] float _stopDistance;

    public bool hasPoweredOn;

    public override void Start()
    {
        base.Start();

        Target = Player.GetComponent<PlayerController>().SquadRangePos;
    }

    public override void Update()
    {
        base.Update();

        CheckForEntityInRange(8);
    }

    public override void CheckForEntityInRange(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, DetectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[0].GetComponentInParent<Entity>().entityType == EntityType.SCRAP)
            {
                _scrapToCollect = colliders[0].transform;

                ChangeState(State.GATHER);
            }
        }
    }

    public override void ChangeState(State newState)
    {
        switch (_currentState)
        {
            case State.IDLE:
                StopIdle();
                break;
            case State.FOLLOW:
                StopFollow();
                break;
            case State.GATHER:
                StopGather();
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
            case State.GATHER:
                StartGather();
                break;
            default:
                Debug.Log($"{newState} is not supported on this robot");
                break;
        }
    }

    public override void CheckState()
    {
        switch (_currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.FOLLOW:
                Follow();
                break;
            case State.GATHER:
                Gather();
                break;
        }
    }

    public override void StartIdle()
    {
        _currentState = State.IDLE;

        BodyAnimator.SetBool("Walking", false);
    }

    public override void StartFollow()
    {
        _currentState = State.FOLLOW;

        BodyAnimator.SetTrigger("Power");
        // WeaponAnimator.SetTrigger("Power");

        Target = Player.GetComponent<PlayerController>().SquadRangePos;

        Agent.enabled = true;
    }

    public override void Follow()
    {
        if (hasPoweredOn)
        {
            base.Follow();
        }
    }

    public override void Recal()
    {
        if (_currentState == State.IDLE)
        {
            ChangeState(State.FOLLOW);
        }
        else if (_currentState == State.FOLLOW)
        {
            ChangeState(State.IDLE);
        }
    }

    public void CollectScrap(int scrapWorth, Transform scrap)
    {
        scrap.GetComponent<NavMeshAgent>().enabled = false;

        scrap.transform.localScale = Vector3.one * 0.4f;

        scrap.SetParent(_grabBone);

        WeaponAnimator.SetTrigger("Oppakken");

        StartCoroutine(SpawnNewRobot(scrapWorth, scrap));
    }

    IEnumerator SpawnNewRobot(int robotsToSpawn, Transform scrap)
    {
        WeaponAnimator.SetBool("Maken", true);
        for (int i = 0; i < robotsToSpawn; i++)
        {


            yield return new WaitForSeconds(_spawnTime);

            Destroy(scrap.gameObject);

            GameObject newRobot = Instantiate(_robotPrefabs[Random.Range(0, _robotPrefabs.Length)]);

            newRobot.transform.position = _spawnPosition.position;
        }
        WeaponAnimator.SetBool("Maken", false);
    }
}
