using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    [SerializeField] protected GameObject enemyBulletPrefab;
    [SerializeField] protected float movementSpeed = 1f;
    [SerializeField] protected float reloadDuration = 1f;
    protected GameObject player;
    protected HealthComponent playerHealth;
    protected float reloadRemaining;
    protected Rigidbody2D body;
    protected HealthComponent health;


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
        // Stop thinking if player despawns or dies
        if (player == null || playerHealth == null || playerHealth.IsDead())
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        reloadRemaining = Mathf.Clamp(reloadRemaining - Time.fixedDeltaTime, 0f, reloadDuration);
        Think();
    }

    // Think acts as an overridable method
    // For Updating enemy behaviour
    // By default will follow the player and shoot whenever he can
    virtual protected void Think()
    {
        Move();
        Attack();
    }

    // Move by default moves towards the player
    // Can be overriden for new behaviour
    virtual protected void Move()
    {
        Vector2 dir = Vector2.Normalize(player.transform.position - transform.position);
        body.linearVelocity = dir * movementSpeed;
    }

    // Attack the player
    // By default shoots the bullet, but can be overriden
    // The reload is handled in the FixedUpdate
    virtual protected void Attack()
    {
        if (reloadRemaining <= 0f)
        {
            reloadRemaining = reloadDuration;

            Vector2 dir = Vector2.Normalize(player.transform.position - transform.position);
            GameObject obj = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
            Bullet script = obj.GetComponent<Bullet>();
            if (script == null) return;
            script.dir = dir;
        }
    }

    virtual protected void OnDamaged()
    {
        Debug.Log(string.Format("HP: {0} / {1} = {2}%", health.GetHealth(), health.GetMaxHealth(), health.GetHealth() * 100 / health.GetMaxHealth()));
    }

    virtual protected void OnDeath()
    {
        health.Damaged -= OnDamaged;
        health.Died -= OnDeath;
        Destroy(gameObject);
    }
}
