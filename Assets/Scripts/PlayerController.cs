using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //* Movement properties
    [SerializeField] float movementSpeed = 5f;
    InputControls controls;
    InputAction moveAction;
    Rigidbody2D body;

    //* Attacking properties
    InputAction attackAction;
    bool attackQueued = false;
    [SerializeField] bool attackAutomaticaly = false;
    [SerializeField] GameObject playerBulletPrefab;

    void Awake()
    {
        controls = new InputControls();
        moveAction = controls.Player.Move;
        attackAction = controls.Player.Attack;
        body = GetComponent<Rigidbody2D>();
        // body.gravityScale = 0f; // can also be disabled in the editor
    }

    void Update()
    {
        attackQueued = attackQueued || attackAutomaticaly || attackAction.WasPressedThisFrame();
    }

    void FixedUpdate()
    {
        body.linearVelocity = Vector2.Normalize(moveAction.ReadValue<Vector2>()) * movementSpeed;
        if (attackQueued) ShootBullet();
        attackQueued = false;
    }

    void ShootBullet()
    {
        GameObject obj = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
        PlayerBullet script = obj.GetComponent<PlayerBullet>();
        if (script == null) return;

        // get mouse position on screen, but it may differ in the world
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        // get mouse position in the world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane)
        );

        // get mouse position relative to current object
        script.dir = Vector2.Normalize(mouseWorldPos - transform.position);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnDispose()
    {
        controls.Dispose();
    }
}
