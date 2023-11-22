using UnityEngine;

public class EnemyScatter : EnemyBehavior
{
    private void OnDisable()
    {
        if (!Enemy.Home.enabled)
            Enemy.Chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the enemy is frightened
        if (node != null && enabled && !Enemy.Frightened.enabled)
        {
            // Pick a random available direction
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -Enemy.Movement.Direction)
                // Wrap the index back around if overflowed
                index = (index + 1) % node.availableDirections.Count;

            Enemy.Movement.SetDirection(node.availableDirections[index]);
        }
    }

}