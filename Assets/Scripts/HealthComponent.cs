using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] int maxHealth = 2;
    int health;
    public event Action Damaged;
    public event Action Died;

    void Awake()
    {
        maxHealth = maxHealth > 0 ? maxHealth : 1;
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (health <= 0 || dmg <= 0) return;
        health -= dmg;
        if (health > 0) Damaged?.Invoke();
        else Died?.Invoke();
    }
}
