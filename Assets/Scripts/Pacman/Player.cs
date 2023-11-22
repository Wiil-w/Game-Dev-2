using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer spriteRenderer;
    private Movement movement;
    private Animator anim;
    private new Collider2D collider;
    [SerializeField] bool rotateSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            movement.SetDirection(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            movement.SetDirection(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            movement.SetDirection(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            movement.SetDirection(Vector2.right);


        if (rotateSprite)
            RotateSprite();
        else {
            // flip sprite
            if (movement.Direction == Vector2.right) transform.localScale = new Vector3(1, 1, 1);
            else if (movement.Direction == Vector2.left) transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void RotateSprite()
    {
        // Rotate player to face the movement direction
        float angle = Mathf.Atan2(movement.Direction.y, movement.Direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        movement.ResetState();

        // if animation clip name is dead, set trigger isAlive
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "brainDeath")
            anim.SetTrigger("isAlive");
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        collider.enabled = false;
        movement.enabled = false;
        anim.SetTrigger("isDead");
    }

    public void Dead()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        GameManager.Instance.PlayerIsDead();
    }

    public void PowerUp(float duration)
    {
        anim.SetBool("poweredUp", true);
        Invoke(nameof(PowerDown), duration);
    }

    private void PowerDown()
    {
        anim.SetBool("poweredUp", false);
    }
}