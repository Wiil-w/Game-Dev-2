using UnityEngine;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] bool rotateSprite;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Anim { get; private set; }
    public Movement Movement { get; private set; }
    public EnemyHome Home { get; private set; }
    public EnemyScatter Scatter { get; private set; }
    public EnemyChase Chase { get; private set; }
    public EnemyFrightened Frightened { get; private set; }
    // public new Light2D light { get; private set; }
    public EnemyBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        Movement = GetComponent<Movement>();
        Home = GetComponent<EnemyHome>();
        Scatter = GetComponent<EnemyScatter>();
        Chase = GetComponent<EnemyChase>();
        Frightened = GetComponent<EnemyFrightened>();
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (rotateSprite)
            RotateSprite();
    }

    private void RotateSprite()
    {
        // Rotate player to face the movement direction
        float angle = Mathf.Atan2(Movement.Direction.y, Movement.Direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        Movement.ResetState();

        Home.Enable();
        Frightened.Disable();
        Chase.Disable();
        Scatter.Disable();
 
        if (initialBehavior != null && initialBehavior != Home) {
            initialBehavior.Enable();
            Home.Disable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
            if (Frightened.enabled)
                GameManager.Instance.EnemyEaten(this);
            else
                GameManager.Instance.PlayerEaten();
    }

}