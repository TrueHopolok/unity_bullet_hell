using UnityEngine;

public class DummyController : MonoBehaviour
{
    HealthComponent health;

    void Awake()
    {
        health = GetComponent<HealthComponent>();
        health.Damaged += OnDamaged;
        health.Died += OnDeath;
    }

    void OnDamaged()
    {
        Debug.Log(string.Format("HP: {0} / {1} = {2}%", health.GetHealth(), health.GetMaxHealth(), health.GetHealth() * 100 / health.GetMaxHealth()));
    }

    void OnDeath()
    {
        health.Damaged -= OnDamaged;
        health.Died -= OnDeath;
        OnDamaged();
        Destroy(gameObject);
    }
}
