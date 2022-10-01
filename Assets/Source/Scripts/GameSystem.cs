using System;
using UnityEngine;

using static GameData;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private float gameTimer = 10f;
    [SerializeField] private float despawnEnemy = 20f;
    [SerializeField] private float switcherooChance = 20f;
    [SerializeField] private LayerMask groundLayer;

    public static GameSystem gameSystem;
    
    public Player player;

    private float timer;

    public float DespawnDistance => despawnEnemy;
    public LayerMask GroundLayer => groundLayer;

    public event Action OnSwitch;
    public event Action OnSwitcheroo;

    private void Awake()
    {
        if (gameSystem == null)
        {
            DontDestroyOnLoad(this);
            gameSystem = this;
        }
        else
        {
            Destroy(this);
        }

        timer = Time.time + gameTimer;
    }

    private void Start()
    {
        _gameSystem = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (timer <= Time.time)
        {
            if (UnityEngine.Random.Range(0, 100) < switcherooChance)
            {
                OnSwitcheroo?.Invoke();
            }
            else
            {
                OnSwitch?.Invoke();
            }

            timer = Time.time + gameTimer;
        }

        _canvasUI.UpdateHealthBar(player.GetHealth(), player.GetMaxHealth());
        _canvasUI.UpdateTimeBar(timer - Time.time, gameTimer);
    }
}