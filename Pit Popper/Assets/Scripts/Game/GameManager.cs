using System;
using UnityEngine;

/// <summary>
/// The main class (singleton) for the game that manges the flow and state of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Get
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    /// <summary>
    /// Reference to the BallSpawner component responsible for spawning balls.
    /// </summary>
    public BallSpawner ballSpawner;

    /// <summary>
    /// Reference to the Game Over UI GameObject.
    /// </summary>
    public GameObject gameOverUI;

    private int totalBallsToSpawn = 10;
    private int currentSpawnedBalls = 0;
    private int maxActiveBalls = 6;
    private int activeBalls = 0;
    private float spawnInterval = 1f;
    private float spawnTimer = 0f;
    private bool gameActive = false;

    public int TotalBallsToSpawn => totalBallsToSpawn;
    public int CurrentSpawnedBalls => currentSpawnedBalls;

    /// <summary>
    /// Event triggered whenever a new ball is spawned.
    /// </summary>
    public static event Action OnBallSpawned;

    /// <summary>
    /// Event triggered when the game has ended.
    /// </summary>
    public static event Action OnGameEnded;

    private void ChangeBallSpawned()
    {
        currentSpawnedBalls++;
        OnBallSpawned?.Invoke();
    }

    void Update()
    {
        if (!gameActive) return;

        if (activeBalls < maxActiveBalls && currentSpawnedBalls < totalBallsToSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                ballSpawner.SpawnBall(currentSpawnedBalls + 1);
                ChangeBallSpawned();
                activeBalls++;
                spawnTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Event that runs when a ball is destroyed.
    /// </summary>
    public void OnBallDestroyed()
    {
        activeBalls--;

        if (currentSpawnedBalls >= totalBallsToSpawn && activeBalls <= 0)
        {
            gameActive = false;
            OnGameEnded?.Invoke();
            gameOverUI.SetActive(true);
        }
    }

    /// <summary>
    /// Starts the game by resetting counters and enabling game state.
    /// </summary>
    public void StartGame()
    {
        currentSpawnedBalls = 0;
        activeBalls = 0;
        spawnTimer = 0f;
        gameActive = true;
    }
}
