using System.Collections;
using UnityEngine;

public class EnemyFrightened : EnemyBehavior
{
    public SpriteRenderer extra;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float flashes = 3;
    [SerializeField] float flashDuration = 4f;

    private bool eaten;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        if (extra != null)
            extra.enabled = false;

        CancelInvoke(nameof(Flash));
        Invoke(nameof(Flash), duration - flashDuration);
    }

    public override void Disable()
    {
        base.Disable();

        if (extra != null)
            extra.enabled = true;
    }

    private void Eaten()
    {
        eaten = true;
        if (extra != null)
            extra.enabled = true;
        Enemy.Movement.SetCollider(false);
        Enemy.Anim.SetBool("isFrightened", false);
        Enemy.Anim.SetTrigger("isDead");
    }

    private void Dead()
    {
        // Enemy.light.enabled = false;
        Enemy.SetPosition(Enemy.Home.inside.position);
        Enemy.Movement.SetCollider(true);
        Enemy.Home.Enable(duration);
        Enemy.Chase.Disable();
        Enemy.Scatter.Disable();
        Disable();
    }

    private void Flash()
    {
        if (!eaten)
            StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        for (int i = 0; i < flashes; i++)
        {
            Enemy.SpriteRenderer.color = new Color(.5f, .5f, .5f, .5f);
            yield return new WaitForSeconds(flashDuration / (flashes * 2));
            Enemy.SpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration / (flashes * 2));
        }
    }

    private void OnEnable()
    {
        // Enemy.light.enabled = false;
        Enemy.Anim.SetBool("isFrightened", true);
        Enemy.Movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        // if (Enemy.Chase.enabled)
        //     Enemy.light.enabled = true;

        Enemy.Anim.SetBool("isFrightened", false);
        Enemy.Movement.speedMultiplier = 1f;
        eaten = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from the player
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (Enemy.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            Enemy.Movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && !eaten && (playerLayer.value & (1 << collision.gameObject.layer)) > 0)
            Eaten();
    }

}