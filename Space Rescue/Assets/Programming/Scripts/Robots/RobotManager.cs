using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    List<RobotAI> _allRobots = new();

    List<RobotAI> _robotsOnTheField = new();

    List<List<RobotAI>> _robotsInSquad = new();

    public RobotAI _robotToAdd;

    private void Start()
    {
        _robotsInSquad.Add(new List<RobotAI>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddRobot(_robotToAdd);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            for (int i = 0; i < _robotsInSquad.Count; i++)
            {
                for (int j = 0; j < _robotsInSquad[i].Count; j++)
                {
                    Debug.Log($"{_robotsInSquad[i][j].name} + {i} + {j}");
                }
            }
        }
    }
    public void AddRobot(RobotAI newRobot)
    {
        for (int i = 0; i < _robotsInSquad.Count; i++)
        {
            if (_robotsInSquad[i].Count > 0 && newRobot.type == _robotsInSquad[i][0].type)
            {
                _robotsInSquad[i].Add(newRobot);

                Debug.Log($"Added robot to: {i}, {_robotsInSquad[i].Count}");
                break;
            }
            else if (_robotsInSquad[_robotsInSquad.Count - 1].Count > 0 && newRobot.type != _robotsInSquad[_robotsInSquad.Count - 1][0].type)
            {
                _robotsInSquad.Add(new List<RobotAI>());
                _robotsInSquad[_robotsInSquad.Count - 1].Add(newRobot);

                Debug.Log($"Added robot to new List: {i + 1}, {_robotsInSquad[_robotsInSquad.Count - 1].Count}");

                break;
            }
            else if (_robotsInSquad[_robotsInSquad.Count - 1].Count == 0)
            {
                _robotsInSquad[_robotsInSquad.Count - 1].Add(newRobot);

                Debug.Log("Added robot to existing empty list");
            }
            // else
            // {
            //     // Debug.Log($"Error {i}");
            // }
        }
    }
}
