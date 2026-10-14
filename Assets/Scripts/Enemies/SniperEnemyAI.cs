using UnityEngine;

// Inherits everything that BasicEnemyAI has
// But can override certain methods
// We will modify `Think()` method
public class SniperEnemyAI : BasicEnemyAI
{
    enum STATE
    {
        Moving,
        Shooting,
    };

    STATE currentState = STATE.Moving;

    // When enemy transitions from Moving to Shooting state
    [SerializeField] float shootingDistance = 6f;

    // When enemy transitions from Shooting to Moving state
    [SerializeField] float movingDistance = 7f;

    override protected void Think()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        switch (currentState)
        {
            case STATE.Moving:
                HandleMovingState(dist);
                break;
            case STATE.Shooting:
                HandleShootingState(dist);
                break;
        }
    }

    void HandleMovingState(float dist)
    {
        Move(); // use default enemy movement method
        if (dist <= shootingDistance)
        {
            currentState = STATE.Shooting;
        }
    }

    void HandleShootingState(float dist)
    {
        body.linearVelocity = Vector2.zero;
        Attack(); // use default enemy shooting method
        if (dist >= movingDistance)
        {
            currentState = STATE.Moving;
        }
    }
}