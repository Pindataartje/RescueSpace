using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSystem : MonoBehaviour
{
    [SerializeField] PlayManager _playManager;

    public bool canWin;

    private void Awake()
    {
        _playManager = FindObjectOfType<PlayManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canWin)
        {
            if (other.CompareTag("Player"))
            {
                _playManager.OnWin();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canWin)
        {
            if (other.CompareTag("Player"))
            {
                _playManager.OnWin();
            }
        }
    }
}
