using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] Transform _rotationTransform;

    [SerializeField] Transform _holder;

    [SerializeField] EnemyAI enemyAI;

    public string[] detectableObjects;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void Update()
    {
        _holder.rotation = _rotationTransform.rotation;

        _holder.rotation = new Quaternion(0, _holder.rotation.y, 0, _holder.rotation.w);
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < detectableObjects.Length; i++)
        {
            if (other.CompareTag(detectableObjects[i]))
            {
                enemyAI.EnterDetection(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Robot") || other.CompareTag("Player"))
        {
            enemyAI.ExitDetection(other.transform);
        }
    }
}
