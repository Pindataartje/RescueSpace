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

    public void CollectScrap(int scrapWorth)
    {

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
