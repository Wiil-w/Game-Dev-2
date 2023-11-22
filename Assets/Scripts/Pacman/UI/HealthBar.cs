using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    [SerializeField] GameObject heartPrefab;
    List<Heart> hearts = new();

    private void OnEnable()
    {
        GameManager.OnHealthChange += DrawHearts;
    }

    private void OnDisable()
    {
        GameManager.OnHealthChange -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        for (int i = 0; i < GameManager.Instance.MaxLives; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            Heart heart = newHeart.GetComponent<Heart>();
            hearts.Add(heart);

            if (i < GameManager.Instance.CurrentLives) 
                heart.SetHeartState(HeartState.Full);
            else 
                heart.SetHeartState(HeartState.Empty);
        }
    }

    public void ClearHearts()
    {
        foreach (Transform heart in transform)
            Destroy(heart.gameObject);

        hearts = new List<Heart>();
    }
}
