using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action OnHealthChange;

    [SerializeField] private Player player;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Transform collectables;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI scoreText;


    private int enemyPointMultiplier = 1;
    private int score;
    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (CurrentLives == 0 && Input.GetKeyDown(KeyCode.Space))
            NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        SetLives(MaxLives);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;
        foreach (Transform collectable in collectables)
            collectable.gameObject.SetActive(true);

        ResetState();
    }

    private void ResetState()
    {
        foreach (Enemy enemy in enemies)
            enemy.ResetState();

        player.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        foreach (Enemy enemy in enemies)
            enemy.gameObject.SetActive(false);

        player.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        CurrentLives = lives;
        OnHealthChange?.Invoke();
    }

    public void EnemyEaten(Enemy enemy)
    {
        int points = enemy.points * enemyPointMultiplier;
        SetScore(score + points);

        enemyPointMultiplier++;
    }

    public void PlayerEaten()
    {
        SetLives(CurrentLives - 1);
        player.DeathSequence();
    }

    public void PlayerIsDead()
    {
        if (CurrentLives > 0) Invoke(nameof(ResetState), 2f);
        else GameOver();
    }

    public void CollectableEaten(Collectable collectable)
    {
        collectable.gameObject.SetActive(false);
        SetScore(score + collectable.points);

        if (!HasRemainingCollectables())
        {
            player.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerUpEaten(PowerUp powerUp)
    {
        foreach (Enemy enemy in enemies) {
            if (enemy.Home.enabled) continue;
            enemy.Frightened.Enable(powerUp.duration);
        }

        player.PowerUp(powerUp.duration);
        CollectableEaten(powerUp);
        CancelInvoke(nameof(ResetEnemyPointMultiplier));
        Invoke(nameof(ResetEnemyPointMultiplier), powerUp.duration);
    }

    private bool HasRemainingCollectables()
    {
        foreach (Transform collectable in collectables)
            if (collectable.gameObject.activeSelf)
                return true;

        return false;
    }

    private void ResetEnemyPointMultiplier()
    {
        enemyPointMultiplier = 1;
    }

}
