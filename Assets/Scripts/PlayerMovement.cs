using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    InputControls controls;
    InputAction moveAction;
    Rigidbody2D body;

    void Awake()
    {
        controls = new InputControls();
        moveAction = controls.Player.Move;
        body = GetComponent<Rigidbody2D>();
        // body.gravityScale = 0f; // can also be disabled in the editor
    }

    void Start()
    {
        GetComponent<HealthComponent>().Died += OnDeath;
    }

    void FixedUpdate()
    {
        body.linearVelocity = Vector2.Normalize(moveAction.ReadValue<Vector2>()) * movementSpeed;
    }

    void OnDeath()
    {
        GetComponent<HealthComponent>().Died -= OnDeath;
        body.bodyType = RigidbodyType2D.Static;
        enabled = false;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}
