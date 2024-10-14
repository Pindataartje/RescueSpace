using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(Mathf.Infinity);
        }

        if (other.CompareTag("Robot"))
        {
            other.GetComponent<RobotAI>().TakeDamage(Mathf.Infinity);
        }
    }
}
