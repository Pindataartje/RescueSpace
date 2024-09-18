using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapRobot : RobotAI
{
    [SerializeField] float _spawnTime;

    [SerializeField] GameObject[] _robotPrefabs;

    [SerializeField] Transform _spawnPosition;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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

    public override void CheckState(State state)
    {
        switch (state)
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

    public void CollectScrap(int scrapWorth)
    {
        // ChangeState(State.GATHER);

        StartCoroutine(SpawnNewRobot(scrapWorth));
    }

    IEnumerator SpawnNewRobot(int robotsToSpawn)
    {
        for (int i = 0; i < robotsToSpawn; i++)
        {
            yield return new WaitForSeconds(_spawnTime);
            GameObject newRobot = Instantiate(_robotPrefabs[Random.Range(0, _robotPrefabs.Length - 1)]);

            newRobot.transform.position = _spawnPosition.position;
        }
    }
}
