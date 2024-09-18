using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;

    public Type type;

    public enum Type
    {
        NONE,
        FIRE,
        WATER,
    }

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
