using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}
