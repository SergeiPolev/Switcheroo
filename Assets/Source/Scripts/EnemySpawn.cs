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
    private int kills = 0;

    private float currentWaveTimer = 0;
    private float timer;

    private Wave currentWave;
    private Wave lastWave;
    private List<Wave> availableWaves = new List<Wave>();

    private Camera currentCamera;

    private int maxOnField;

    private const float timeToSwitch = 5f;

    private void Awake()
    {
        _enemySpawn = this;

        currentCamera = FindObjectOfType<RenderCamera>().GetComponent<Camera>();

        SetWave(firstWave);

        _gameSystem.OnSwitch += SwitchWave;

        int level = PlayerPrefs.GetInt(_gameSystem.GAME_WON_KEY, 0);

        foreach (Wave wave in waves)
        {
            if (wave.MinLevel <= level)
            {
                availableWaves.Add(wave);
            }
        }

        maxOnField = maxEnemiesOnField;
    }
    private void OnDestroy()
    {
        _gameSystem.OnSwitch -= SwitchWave;
    }
    private void Update()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        currentWaveTimer += Time.deltaTime;

        if (currentWaveTimer >= timeToSwitch)
        {
            currentWaveTimer = 0;

            SetWave(availableWaves[Random.Range(0, availableWaves.Count)]);
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

        foreach (var wave in availableWaves)
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
        kills++;
        _canvasUI.UpdateKills(kills);
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
            Instantiate(enemy, hit.point, Quaternion.identity);

            currentAmount++;
        }
    }
}