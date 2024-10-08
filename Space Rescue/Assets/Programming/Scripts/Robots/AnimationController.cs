using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] ScrapRobot _scrapRobot;

    public void PowerOn()
    {
        Debug.Log("Animation");
        _scrapRobot.hasPoweredOn = true;
    }

    public void PowerOff()
    {
        _scrapRobot.hasPoweredOn = false;
    }
}
