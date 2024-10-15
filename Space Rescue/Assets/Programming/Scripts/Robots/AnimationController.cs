using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] ScrapRobot _scrapRobot;

    [SerializeField] RobotAI _robot;

    public void PowerOn()
    {
        _scrapRobot.hasPoweredOn = true;
        _scrapRobot.Activate();
    }

    public void PowerOff()
    {
        _scrapRobot.hasPoweredOn = false;
    }

    public void Activate()
    {
        _robot.Activate();
    }

    public void Explosion()
    {
        _robot.GetComponent<KaboomBot>().DeathAttack();
    }
}
