using System.Collections.Generic;
using UnityEngine;

using static GameData;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private int maxEnemiesOnField;
    [SerializeField] private float delayBetweenSpawn = .4f;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Wave firstWave;
    [SerializeField] private LayerMask groundLayer;

    private int currentAmount = 0;

    private float currentWaveTimer = 0;
    private float timer;

    private Wave currentWave;
    private Wave lastWave;

    private Camera currentCamera;

    private bool isStopped;
    private int maxOnField;

    private const float timeToSwitch = 10f;

    private void Awake()
    {
        _enemySpawn = this;

        currentCamera = Camera.main;

        SetWave(firstWave);

        _gameSystem.OnSwitch += SwitchWave;

        maxOnField = maxEnemiesOnField;
    }
    private void OnDestroy()
    {
        _gameSystem.OnSwitch -= SwitchWave;
    }
    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        currentWaveTimer += Time.deltaTime;

        if (currentWaveTimer >= timeToSwitch)
        {
            currentWaveTimer = 0;

            SetWave(waves[Random.Range(0, waves.Length)]);
        }

        if (currentAmount >= maxOnField)
        {
            return;
        }

        if (timer <= Time.time)
        {
            int randomIndex = Random.Range(0, currentWave.Enemies.Length);
            SpawnEnemy(currentWave.Enemies[randomIndex]);

            timer = Time.time + delayBetweenSpawn;
        }
    }

    private void SwitchWave()
    {
        List<Wave> newWaves = new List<Wave>();

        foreach (var wave in waves)
        {
            if (wave == lastWave)
            {
                continue;
            }

            newWaves.Add(wave);
        }

        lastWave = currentWave;

        var newWave = newWaves.Count > 0 ? newWaves[UnityEngine.Random.Range(0, newWaves.Count)] : firstWave;
        lastWave = newWave;
    }

    public void SetWave(Wave wave)
    {
        lastWave = currentWave == null ? wave : currentWave;
        currentWave = wave;

        Debug.Log(wave.name);
    }
    public void EnemyDied()
    {
        currentAmount--;
    }
    private void SpawnEnemy(GameObject enemy)
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
            var enemyGO = Instantiate(enemy, hit.point, Quaternion.identity);

            currentAmount++;
        }
    }
}