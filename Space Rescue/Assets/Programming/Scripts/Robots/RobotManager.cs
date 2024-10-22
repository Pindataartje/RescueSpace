using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    List<RobotAI> _allRobots = new();

    List<RobotAI> _robotsOnTheField = new();

    List<List<RobotAI>> _robotsInSquad = new();
    public List<List<RobotAI>> RobotsInSquad
    { get { return _robotsInSquad; } }

    List<List<RobotAI>> _throwableRobots = new();

    public List<RobotAI> unsortedSquad;

    [SerializeField] int _numberOfRobotsInSquad;
    public int NumberOfRobotsInSquad
    { get { return _numberOfRobotsInSquad; } }

    [SerializeField] RobotAI _currentRobot;
    public RobotAI CurrentRobot
    { get { return _currentRobot; } }

    [SerializeField] int _currentSquad;
    public int CurrentSquad
    { get { return _currentSquad; } set { _currentSquad = value; } }


    private void Start()
    {
        _robotsInSquad.Add(new List<RobotAI>());
    }

    public void HandleCurrentSquad()
    {
        _currentSquad--;

        if (_currentSquad < 0)
        {
            _currentSquad = _robotsInSquad.Count - 1;

            // Debug.Log($"{_currentSquad} squad number");
        }
    }

    public void RemoveRobotFromSquad(RobotAI robotToRemove)
    {
        Debug.Log("AA");
        for (int i = 0; i < _robotsInSquad.Count; i++)
        {
            if (_robotsInSquad[i].Contains(robotToRemove))
            {
                unsortedSquad.Remove(robotToRemove);
                _robotsInSquad[i].Remove(robotToRemove);
                robotToRemove.LeaveSquad(); // Call LeaveSquad to perform any necessary cleanup on the robot
                _numberOfRobotsInSquad--;

                Debug.Log($"Removed robot from squad {i}, remaining count: {_robotsInSquad[i].Count}");

                if (_robotsInSquad[i].Count == 0)
                {
                    _robotsInSquad.RemoveAt(i);

                    if (_currentSquad == i)
                    {
                        HandleCurrentSquad();
                    }

                    Debug.Log($"Removed squad {i}");

                }

                return; // Exit after removing to avoid unnecessary iterations
            }
        }
    }

    public void AddRobotToSquad(RobotAI robotToAdd)
    {
        if (_robotsInSquad.Count == 0)
        {
            _robotsInSquad.Add(new List<RobotAI>());
        }

        for (int i = 0; i < _robotsInSquad.Count; i++)
        {
            if (_robotsInSquad[_robotsInSquad.Count - 1].Count == 0)
            {
                unsortedSquad.Add(robotToAdd);
                _robotsInSquad[_robotsInSquad.Count - 1].Add(robotToAdd);
                robotToAdd.EnterSquad();
                _numberOfRobotsInSquad++;
                Debug.Log("Added robot to existing empty squad");
                return;
            }
            else if (_robotsInSquad[i].Count > 0 && robotToAdd.type == _robotsInSquad[i][0].type)
            {
                unsortedSquad.Add(robotToAdd);
                _robotsInSquad[i].Add(robotToAdd);
                robotToAdd.EnterSquad();
                _numberOfRobotsInSquad++;

                Debug.Log($"Added robot to: {i}, {_robotsInSquad[i].Count} squad that already has the same robot");
                return;
            }
            if (i == _robotsInSquad.Count - 1)
            {
                Debug.Log("Last try");
                if (robotToAdd.type != _robotsInSquad[_robotsInSquad.Count - 1][0].type)
                {
                    _robotsInSquad.Add(new List<RobotAI>());
                    unsortedSquad.Add(robotToAdd);
                    _robotsInSquad[_robotsInSquad.Count - 1].Add(robotToAdd);
                    robotToAdd.EnterSquad();
                    _numberOfRobotsInSquad++;

                    Debug.Log($"Added robot to new squad: {i + 1}, {_robotsInSquad[_robotsInSquad.Count - 1].Count}");

                    break;
                }
            }
        }
    }

    public bool SquadContains(RobotAI robotToCheck)
    {
        for (int i = 0; i < unsortedSquad.Count; i++)
        {
            if (unsortedSquad[i] == robotToCheck)
            {
                return true;
            }
        }

        return false;
    }
}
