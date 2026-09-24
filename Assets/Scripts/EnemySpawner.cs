using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float respawnDuration = 3f;
    float respawnRemaining;
    [SerializeField] GameObject[] enemyPrefabs;
    GameObject[] spawnpoints;
    GameObject player;
    HealthComponent playerHealth;

    void Start()
    {
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthComponent>();
        respawnRemaining = respawnDuration;
    }

    void FixedUpdate()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead()) return;

        respawnRemaining -= Time.fixedDeltaTime;
        if (respawnRemaining <= 0f)
        {
            respawnRemaining = respawnDuration;

            if (spawnpoints.Length <= 0) return;
            GameObject selectedSpawn = spawnpoints[Random.Range(0, spawnpoints.Length)];
            if (selectedSpawn == null) return;

            if (enemyPrefabs.Length <= 0) return;
            GameObject selectedEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            if (selectedEnemy == null) return;

            Instantiate(selectedEnemy, selectedSpawn.transform.position, selectedSpawn.transform.rotation);
        }
    }
}
