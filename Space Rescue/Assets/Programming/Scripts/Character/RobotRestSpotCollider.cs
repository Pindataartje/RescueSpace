using UnityEngine;

public class RobotRestSpotCollider : MonoBehaviour
{
    [SerializeField] Transform model;

    [SerializeField] Transform player;

    [SerializeField] Vector3 offset;

    [SerializeField] Transform restSpot;

    [SerializeField] RaycastHit hit;
    [SerializeField] RaycastHit hit2;

    [SerializeField] float _squadRange;

    [SerializeField] LayerMask _terrainLayer;
    [SerializeField] LayerMask _robotLayer;


    private void Update()
    {
        HandlePosition();

        HandleSquad();
    }

    void HandlePosition()
    {
        offset = new Vector3(player.transform.position.x, player.transform.position.y - 0.85f, player.transform.position.z);

        if (Physics.Raycast(offset, -model.forward, out hit, 2, _terrainLayer))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = restSpot.position;
        }
    }

    void HandleSquad()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _squadRange, _robotLayer);

        foreach (Collider collider in colliders)
        {
            if (!player.GetComponent<PlayerController>().Robots.Contains(collider.gameObject) && collider.GetComponent<RobotAI>()._currentState == RobotAI.State.FOLLOW)
            {
                player.GetComponent<PlayerController>().AddRobot(collider.gameObject);
            }
        }
    }
}
