using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    public int points = 10;

    protected virtual void Eat()
    {
        GameManager.Instance.CollectableEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
            Eat();

    }

}