using UnityEngine;

public class EnemyChase : EnemyBehavior
{

    private void OnEnable()
    {
        // Enemy.light.enabled = true;
        Enemy.Anim.SetBool("isChasing", true);
    }

    private void OnDisable()
    {
        // Enemy.light.enabled = false;
        Enemy.Anim.SetBool("isChasing", false);
        if (!Enemy.Home.enabled)
            Enemy.Scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the enemy is frightened
        if (node != null && enabled && !Enemy.Frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Find the available direction that moves closet to the player
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is less than the current
                // min distance then this direction becomes the new closest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (Enemy.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            Enemy.Movement.SetDirection(direction);
        }
    }

}