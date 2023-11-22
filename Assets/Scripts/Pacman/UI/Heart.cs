using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    [SerializeField] private Sprite fullHeart, emptyHeart;
    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartState(HeartState state)
    {
        switch (state)
        {
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

public enum HeartState
{
    Full,
    Empty
}
