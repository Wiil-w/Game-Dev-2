using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class EnemyBehavior : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    public float duration;

    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }

}