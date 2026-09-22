using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject playerBulletPrefab;
    [SerializeField] bool attackAutomaticaly = false;
    InputControls controls;
    InputAction attackAction;
    bool attackQueued = false;

    void Awake()
    {
        controls = new InputControls();
        attackAction = controls.Player.Attack;
    }

    void Start()
    {
        GetComponent<HealthComponent>().Died += OnDeath;
    }

    void Update()
    {
        attackQueued = attackQueued || attackAutomaticaly || attackAction.WasPressedThisFrame();
    }

    void FixedUpdate()
    {
        if (!attackQueued) return;
        attackQueued = false;

        GameObject obj = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
        Bullet script = obj.GetComponent<Bullet>();
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

    void OnDeath()
    {
        GetComponent<HealthComponent>().Died -= OnDeath;
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

    void OnDispose()
    {
        controls.Dispose();
    }
}