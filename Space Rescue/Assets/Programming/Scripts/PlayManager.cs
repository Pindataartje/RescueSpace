using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public bool killedAllEnemies;

    [SerializeField] List<EnemyAI> enemies;

    [SerializeField] List<RobotAI> robots;

    [SerializeField] ScrapRobot scrapRobot;

    [SerializeField] NavigationSystem _navigationSystem;

    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;

    private void Awake()
    {
        enemies = FindObjectsOfType<EnemyAI>().ToList();

        robots = FindObjectsOfType<RobotAI>().ToList();

        scrapRobot = FindObjectOfType<ScrapRobot>();
    }

    public void OnEnemyDeath(EnemyAI enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

        if (enemies.Count == 0)
        {
            killedAllEnemies = true;

            if (_navigationSystem != null)
            {
                _navigationSystem.canWin = true;
            }
        }
    }

    public void OnRobotSpawned(RobotAI robot)
    {
        if (robots.Contains(robot))
        {
            return;
        }
        else
        {
            robots.Add(robot);
        }
    }

    public void OnRobotDeath(RobotAI robot)
    {
        if (robots.Contains(robot))
        {
            robots.Remove(robot);
        }

        if (robots.Count == 0)
        {
            RobotFailSafe();
        }
    }

    public void OnPlayerDeath()
    {
        Time.timeScale = 0;

        _loseScreen.SetActive(true);
    }

    public void OnWin()
    {
        _navigationSystem.canWin = false;

        _winScreen.SetActive(true);
    }

    public void RobotFailSafe()
    {
        scrapRobot.RobotFailSafe();
    }
}
