using UnityEngine;

/// <summary>
/// Spawns balls at random positions with random rotations.
/// </summary>
public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;

    public void SpawnBall(int ballID)
    {
        float xPosition = Random.Range(-3.8f, 4.6f);
        float zPosition = Random.Range(-1.7f, 1.7f);
        Vector3 position = new Vector3(xPosition, 1.22f, zPosition);

        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject ball = Instantiate(ballPrefab, position, rotation);
        ball.name = "Ball_" + ballID;
    }
}
