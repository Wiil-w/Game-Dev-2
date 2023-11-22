using System.Collections;
using UnityEngine;

public class EnemyHome : EnemyBehavior
{
    public Transform inside;
    public Transform outside;
    [SerializeField] LayerMask obstacleLayer;

    private void OnEnable()
    {
        // Enemy.light.enabled = false;
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        // Check for active self to prevent error when object is destroyed
        if (gameObject.activeInHierarchy)
        {
            // if animation clip name is dead, set trigger isAlive
            if (Enemy.Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "eyeDeath")
                Enemy.Anim.SetTrigger("isAlive");
            StartCoroutine(ExitTransition());
            if (!Enemy.Chase.enabled)
                Enemy.Scatter.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction everytime the enemy hits a wall to create the
        // effect of the enemy bouncing around the home
        if (enabled && (obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
            Enemy.Movement.SetDirection(-Enemy.Movement.Direction);
    }

    private IEnumerator ExitTransition()
    {
        // Turn off movement while we manually animate the position
        Enemy.Movement.SetDirection(Vector2.up, true);
        Enemy.Movement.SetKinematic(true);
        Enemy.Movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        // Animate to the starting point
        while (elapsed < duration)
        {
            Enemy.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animate exiting the enemy home
        while (elapsed < duration)
        {
            Enemy.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pick a random direction left or right and re-enable movement
        Enemy.Movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        Enemy.Movement.SetKinematic(false);
        Enemy.Movement.enabled = true;
    }

}