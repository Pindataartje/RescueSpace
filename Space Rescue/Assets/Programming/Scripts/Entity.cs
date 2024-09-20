using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Type")]
    public Type type;

    [Header("Info")]

    public int health;
    public float speed;
    public int damage;

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
