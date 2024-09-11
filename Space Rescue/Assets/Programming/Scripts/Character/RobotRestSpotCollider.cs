using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RobotRestSpotCollider : MonoBehaviour
{
    [SerializeField] Transform model;

    [SerializeField] Transform player;

    [SerializeField] Vector3 offset;

    [SerializeField] Transform restSpot;

    [SerializeField] RaycastHit hit;
    [SerializeField] RaycastHit hit2;

    [SerializeField] LayerMask _terrainLayer;

    private void Update()
    {
        offset = new Vector3(player.position.x, player.position.y - 0.85f, player.position.z);

        if (Physics.Raycast(offset, -model.forward, out hit, 2, _terrainLayer))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = restSpot.position;
        }
    }
}
