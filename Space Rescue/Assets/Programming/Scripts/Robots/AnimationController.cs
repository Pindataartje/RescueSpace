using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] ScrapRobot _scrapRobot;

    [SerializeField] RobotAI _robot;

    public void PowerOn()
    {
        Debug.Log("Animation");
        _scrapRobot.hasPoweredOn = true;
    }

    public void PowerOff()
    {
        _scrapRobot.hasPoweredOn = false;
    }

    public void Started()
    {
        // canwalk for base robot
    }

    public void Explosion()
    {
        _robot.GetComponent<KaboomBot>().DeathAttack();
    }
}
