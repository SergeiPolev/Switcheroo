using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static GameData;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private float gameTimer = 10f;
    [SerializeField] private float despawnEnemy = 20f;
    [SerializeField] private float switcherooChance = 20f;
    [SerializeField] private float timeToBoss = 100f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioSource soundsSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip switchSound;
    [SerializeField] private AudioClip switcherooSound;
    [SerializeField] private AudioMixerGroup masterGroup;

    [SerializeField] private Drop[] allDrops;

    public static GameSystem gameSystem;

    public Player player;
    public Boss boss;
    public List<Drop> availableDrops;

    private float timer;
    private float bossTimer;

    public float DespawnDistance => despawnEnemy;
    public LayerMask GroundLayer => groundLayer;
    public LayerMask EnemyLayer => enemyLayer;
    public List<Drop> Drops => availableDrops;
    public bool Playing => playing;
    private bool playing = false;
    private bool bossAppeared = false;

    public event Action OnSwitch;
    public event Action OnSwitcheroo;

    public event Action OnLose;
    public event Action OnWin;
    public event Action OnBoss;
    public event Action OnSecondaryPickUp;

    public int PassedLevels => PlayerPrefs.GetInt(GAME_WON_KEY, 0);
    public float Volume => PlayerPrefs.GetFloat(VOLUME, 0);

    public string GAME_WON_KEY = "GameWon";
    public string VOLUME = "Volume";

    private void Awake()
    {
        _gameSystem = this;

        timer = Time.time + gameTimer;

        player.Health.Died += Lose;
        boss.GetHealth().Died += Win;

        availableDrops = new List<Drop>();

        foreach (var drop in allDrops)
        {
            if (PassedLevels >= drop.MinLevel)
            {
                availableDrops.Add(drop);
            }
        }

        masterGroup.audioMixer.SetFloat("MasterVolume", Volume);
    }
    public void Mute(bool enable)
    {
        var value = enable ? 0 : -80f;
        masterGroup.audioMixer.SetFloat("MasterVolume", value);
        PlayerPrefs.SetFloat(VOLUME, value);

        Debug.Log(PlayerPrefs.GetFloat(VOLUME));
    }
    private void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }
    public void SecondaryPickUp()
    {
        OnSecondaryPickUp?.Invoke();
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
        OnBoss?.Invoke();
    }

    public void Switcheroo()
    {
        OnSwitcheroo?.Invoke();

        PlayShot(switcherooSound, 1f);
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
                Switcheroo();
            }
            else
            {
                OnSwitch?.Invoke();
                PlayShot(switchSound, 1f);
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