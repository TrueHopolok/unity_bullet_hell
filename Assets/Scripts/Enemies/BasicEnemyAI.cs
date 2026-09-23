using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float reloadDuration = 1f;
    GameObject player;
    HealthComponent playerHealth;
    float reloadRemaining;
    Rigidbody2D body;
    HealthComponent health;


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthComponent>();
        health.Damaged += OnDamaged;
        health.Died += OnDeath;
        reloadRemaining = reloadDuration;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // we will fix this later, but for now its here
        if (player != null) playerHealth = player.GetComponent<HealthComponent>();
    }

    void FixedUpdate()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead())
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = Vector2.Normalize(player.transform.position - transform.position);
        body.linearVelocity = dir * movementSpeed;

        if (reloadRemaining <= 0f)
        {
            reloadRemaining = reloadDuration;

            GameObject obj = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
            Bullet script = obj.GetComponent<Bullet>();
            if (script == null) return;
            script.dir = dir;
        }
        else
        {
            reloadRemaining -= Time.fixedDeltaTime;
        }
    }

    void OnDamaged()
    {
        Debug.Log(string.Format("HP: {0} / {1} = {2}%", health.GetHealth(), health.GetMaxHealth(), health.GetHealth() * 100 / health.GetMaxHealth()));
    }

    void OnDeath()
    {
        health.Damaged -= OnDamaged;
        health.Died -= OnDeath;
        Destroy(gameObject);
    }
}
