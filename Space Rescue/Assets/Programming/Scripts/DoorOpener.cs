using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] Animator _doorAnimator;

    [SerializeField] List<Entity> _entitiesInRange;

    [SerializeField] AudioSource audioSource;

    [SerializeField] bool _isOpen;

    [SerializeField] bool player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Entity>(out Entity entity))
        {
            if (!_entitiesInRange.Contains(entity) && !_isOpen)
            {
                _isOpen = true;
                _entitiesInRange.Add(entity);
                _doorAnimator.SetBool("Open", true);
                audioSource.Play();
            }
        }
        if (other.CompareTag("Player") && !_isOpen)
        {
            _isOpen = true;
            _doorAnimator.SetBool("Open", true);
            audioSource.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = true;
        }

        if (other.TryGetComponent<Entity>(out Entity entity))
        {
            if (!_entitiesInRange.Contains(entity))
            {
                _entitiesInRange.Add(entity);
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

        if (other.CompareTag("Player"))
        {
            player = false;
        }

        if (_entitiesInRange.Count <= 0 && _isOpen && !player)
        {
            _isOpen = false;
            _doorAnimator.SetBool("Open", false);
            audioSource.Play();
        }
    }
}
