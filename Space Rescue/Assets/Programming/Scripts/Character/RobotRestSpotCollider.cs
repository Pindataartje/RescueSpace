using System.Collections.Generic;
using UnityEngine;

public class RobotRestSpotCollider : MonoBehaviour
{
    [SerializeField] RobotManager robotManager;
    [SerializeField] PlayerController player;

    [SerializeField] RaycastHit hit1;
    [SerializeField] RaycastHit hit2;

    [SerializeField] float _minSquadRange;
    [SerializeField] float _squadRange;

    [SerializeField] float _extraRobotSize;

    [SerializeField] LayerMask _terrainLayer;
    [SerializeField] LayerMask _robotLayer;

    [SerializeField] int _currentSquad;
    [SerializeField] int _currentRobot;

    [SerializeField] Vector3 _squadRangePos;
    public Vector3 SquadRangePos
    { get { return _squadRangePos; } }


    [SerializeField] float _squadXoffset;

    private void Start()
    {
        _squadXoffset = _minSquadRange;

        robotManager = FindObjectOfType<RobotManager>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        HandlePosition();

        HandleSquad();
    }

    void HandlePosition()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit1, 0.9f, _terrainLayer))
        {
            Debug.Log("hit ground");

            if (Physics.Raycast(transform.position, -transform.forward, out hit2, _squadRange, _terrainLayer))
            {
                Debug.Log("hit wall");

                Mathf.Abs(_squadRange);
            }
            _squadRangePos = new Vector3(0, hit1.point.y, _squadXoffset);
        }
    }

    void HandleSquad()
    {
        Collider[] colliders = Physics.OverlapSphere(_squadRangePos, _squadRange, _robotLayer);

        List<RobotAI> robotsInRange = new List<RobotAI>();
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out RobotAI robotAI) && robotAI._currentState == RobotAI.State.FOLLOW)
            {
                RobotAI newrobot = collider.GetComponent<RobotAI>();
                robotsInRange.Add(newrobot);

                if (!robotManager.SquadContains(newrobot))
                {
                    robotManager.AddRobotToSquad(newrobot);
                    HandleSquadRange();
                }
            }
        }

        List<RobotAI> robotsToRemove = new List<RobotAI>();
        foreach (RobotAI robot in robotManager.unsortedSquad)
        {
            if (!robotsInRange.Contains(robot))
            {
                robotsToRemove.Add(robot);
            }
        }

        foreach (RobotAI robotToRemove in robotsToRemove)
        {
            robotManager.RemoveRobotFromSquad(robotToRemove);
            HandleSquadRange();
        }

    }


    public void HandleSquadRange()
    {
        _squadXoffset = _minSquadRange;

        for (int i = 0; i < robotManager.NumberOfRobotsInSquad; i++)
        {
            _squadXoffset += _extraRobotSize;
            float robotofset = Mathf.Abs(_extraRobotSize);
            _squadRange += robotofset;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(_squadRangePos, _squadRange);
    }
}
