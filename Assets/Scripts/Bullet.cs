using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int dmg = 1;
    [SerializeField] public float speed = 10f;
    public Vector2 dir = Vector2.right; // will be set on creation by player

    // Awake will run too quickly, thus causing
    // Caller to miss the chance to set dir to required value
    void Start()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        dir = Vector2.Normalize(dir);
        body.linearVelocity = dir * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        HealthComponent health = obj.GetComponent<HealthComponent>();
        if (health) health.TakeDamage(dmg);
        Destroy(gameObject); // despawn self
    }
}
