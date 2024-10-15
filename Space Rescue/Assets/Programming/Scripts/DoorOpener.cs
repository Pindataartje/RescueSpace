using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] Animator _doorAnimator;

    [SerializeField] List<Entity> _entitiesInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Entity>(out Entity entity))
        {
            if (!_entitiesInRange.Contains(entity))
            {
                _entitiesInRange.Add(entity);
                _doorAnimator.SetBool("Open", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Entity>(out Entity entity))
        {
            if (_entitiesInRange.Contains(entity))
            {
                _entitiesInRange.Remove(entity);
            }
        }

        if (_entitiesInRange.Count <= 0)
        {
            _doorAnimator.SetBool("Open", false);
        }
    }
}
