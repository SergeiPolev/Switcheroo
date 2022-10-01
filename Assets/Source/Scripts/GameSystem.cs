using System;
using UnityEngine;

using static GameData;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private float gameTimer = 10f;
    [SerializeField] private float despawnEnemy = 20f;
    [SerializeField] private float switcherooChance = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource soundsSource;

    public static GameSystem gameSystem;
    
    public Player player;

    private float timer;

    public float DespawnDistance => despawnEnemy;
    public LayerMask GroundLayer => groundLayer;
    public bool Playing => player.Health.CurrentPoints > 0;
    private bool playing = true;

    public event Action OnSwitch;
    public event Action OnSwitcheroo;

    public event Action OnLose;
    public event Action OnWin;

    private void Awake()
    {
        _gameSystem = this;

        timer = Time.time + gameTimer;

        player.Health.Died += Lose;
        //player.Health.Died += Win;
    }

    private void Win()
    {
        OnWin?.Invoke();

        playing = false;
    }
    private void Lose()
    {
        OnLose?.Invoke();

        playing = false;
        player.gameObject.SetActive(false);
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

    public void PlayShot(AudioClip clip, float volume = 0.5f)
    {
        soundsSource.PlayOneShot(clip, volume);
    }
}