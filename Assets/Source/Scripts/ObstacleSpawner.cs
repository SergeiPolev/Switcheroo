using UnityEngine;

using static GameData;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private int maxObstacleOnScreen = 3;
    [SerializeField] private float spawnCooldown = 3f;
    [SerializeField] private Obstacle[] obstaclePrefabs;
    [SerializeField] private Vector3 minSize;
    [SerializeField] private Vector3 maxSize;
    [SerializeField] private float maxXRotation = 45;

    private Camera currentCamera;

    private int currentAmount;

    private float timer;

    private void Awake()
    {
        currentCamera = Camera.main;
    }

    private void Update()
    {
        if (maxObstacleOnScreen > currentAmount)
        {
            if (Time.time <= timer)
            {
                return;
            }

            SpawnObstacle();

            timer = Time.time + spawnCooldown;
        }
    }
    private void DespawnObstacle()
    {
        currentAmount--;
    }
    private void SpawnObstacle()
    {
        float xPos;
        float yPos;
        float sign = Mathf.Sign(Random.Range(-1, 2));

        if (Random.Range(0, 100) > 50)
        {
            xPos = 0.5f + 1f * sign;
            yPos = Random.Range(0, 100) / 100f;
        }
        else
        {
            xPos = Random.Range(0, 100) / 100f;
            yPos = 0.5f + 1f * sign;
        }

        Vector3 point = currentCamera.ViewportToWorldPoint(new Vector3(xPos, yPos, 0));
        Ray ray = new Ray(point, currentCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _gameSystem.GroundLayer))
        {
            Obstacle obstacleComp = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)].gameObject, hit.point, Quaternion.identity).GetComponent<Obstacle>();

            Vector3 randomScale = new Vector3(
                Random.Range(minSize.x, maxSize.x),
                Random.Range(minSize.y, maxSize.y),
                Random.Range(minSize.z, maxSize.z));

            obstacleComp.gameObject.transform.localScale = randomScale;

            Vector3 newRotation = new Vector3(
                Random.Range(-maxXRotation, maxXRotation),
                Random.Range(0, 359f),
                Random.Range(-maxXRotation, maxXRotation));

            obstacleComp.transform.rotation = Quaternion.Euler(newRotation);

            obstacleComp.OnDespawn += DespawnObstacle;

            currentAmount++;
        }
    }
}