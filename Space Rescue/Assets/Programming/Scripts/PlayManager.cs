using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public bool killedAllEnemies;

    [SerializeField] int _killCount;
    [SerializeField] int _enemyCount;
    [SerializeField] TMP_Text _killCountTxt;

    [SerializeField] List<EnemyAI> _enemies;

    [SerializeField] List<RobotAI> _robots;

    [SerializeField] ScrapRobot _scrapRobot;

    [SerializeField] NavigationSystem _navigationSystem;

    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;



    private void Awake()
    {
        _enemies = FindObjectsOfType<EnemyAI>().ToList();
        _enemyCount = _enemies.Count;

        _robots = FindObjectsOfType<RobotAI>().ToList();

        _scrapRobot = FindObjectOfType<ScrapRobot>();

        _navigationSystem = FindObjectOfType<NavigationSystem>();

        if (_killCountTxt != null)
        {
            _killCountTxt.text = $"Enemies left: {_enemyCount - _killCount}/{_enemyCount}";
        }
    }

    public void OnEnemyDeath(EnemyAI enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }

        if (_enemies.Count == 0)
        {
            killedAllEnemies = true;

            if (_navigationSystem != null)
            {
                _navigationSystem.canWin = true;
            }
        }

        _killCount++;

        if (_killCountTxt != null)
        {
            _killCountTxt.text = $"Enemies left: {_enemyCount - _killCount}/{_enemyCount}";
        }
    }

    public void OnRobotSpawned(RobotAI robot)
    {
        if (_robots.Contains(robot))
        {
            return;
        }
        else
        {
            _robots.Add(robot);
        }
    }

    public void OnRobotDeath(RobotAI robot)
    {
        if (_robots.Contains(robot))
        {
            _robots.Remove(robot);
        }

        if (_robots.Count == 0)
        {
            RobotFailSafe();
        }

    }

    public void OnPlayerDeath()
    {
        Debug.Log("PlayerDeath");
        Time.timeScale = 0;

        _loseScreen.SetActive(true);

        Cursor.visible = true;
    }

    public void OnWin()
    {
        _navigationSystem.canWin = false;

        _winScreen.SetActive(true);

        Cursor.visible = true;
    }

    public void RobotFailSafe()
    {
        _scrapRobot.RobotFailSafe();
    }
}
