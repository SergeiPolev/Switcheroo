using System;
using UnityEngine;

using static GameData;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private float gameTimer = 10f;
    [SerializeField] private float despawnEnemy = 20f;
    [SerializeField] private float switcherooChance = 20f;
    [SerializeField] private float timeToBoss = 100f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource soundsSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip switchSound;
    [SerializeField] private AudioClip switcherooSound;

    public static GameSystem gameSystem;

    public Player player;
    public Boss boss;

    private float timer;
    private float bossTimer;

    public float DespawnDistance => despawnEnemy;
    public LayerMask GroundLayer => groundLayer;
    public bool Playing => playing;
    private bool playing = false;
    private bool bossAppeared = false;

    public event Action OnSwitch;
    public event Action OnSwitcheroo;

    public event Action OnLose;
    public event Action OnWin;

    public string GAME_WON_KEY = "GameWon";

    private void Awake()
    {
        _gameSystem = this;

        timer = Time.time + gameTimer;

        player.Health.Died += Lose;
        boss.GetHealth().Died += Win;
    }

    private void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StartGame()
    {
        playing = true;

        timer = Time.time + gameTimer;

        PlayMusic(gameMusic);
    }
    private void Win()
    {
        OnWin?.Invoke();

        playing = false;

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.DespawnEnemy();
        }

        PlayMusic(winMusic);
    }
    private void Lose()
    {
        OnLose?.Invoke();

        playing = false;
        player.gameObject.SetActive(false);

        PlayMusic(menuMusic);
    }
    private void BossAppear()
    {
        boss.gameObject.SetActive(true);
        boss.transform.SetParent(null);
        bossAppeared = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (!playing)
        {
            return;
        }

        if (timer <= Time.time)
        {
            if (UnityEngine.Random.Range(0, 100) < switcherooChance)
            {
                OnSwitcheroo?.Invoke();

                PlayShot(switcherooSound, .7f);
            }
            else
            {
                OnSwitch?.Invoke();
                PlayShot(switchSound, .7f);
            }

            timer = Time.time + gameTimer;
        }

        bossTimer += Time.deltaTime;

        if (bossTimer >= timeToBoss && !bossAppeared)
        {
            BossAppear();
        }

        _canvasUI.UpdateHealthBar(player.GetHealth(), player.GetMaxHealth());
        _canvasUI.UpdateTimeBar(timer - Time.time, gameTimer);
        _canvasUI.UpdateDiceBar(bossAppeared ? boss.GetHealth().CurrentPoints / boss.GetHealth().MaxPoints : bossTimer / timeToBoss);
    }

    public void PlayShot(AudioClip clip, float volume = 0.5f)
    {
        soundsSource.PlayOneShot(clip, volume);
    }
}