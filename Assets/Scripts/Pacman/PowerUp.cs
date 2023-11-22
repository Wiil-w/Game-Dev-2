
public class PowerUp : Collectable
{
    public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.PowerUpEaten(this);
    }

}