using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static GameData;

public class CanvasUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image timeBar;
    [SerializeField] private Image diceBar;
    [SerializeField] private Image skillBar;
    [SerializeField] private Image skillBarCircle;
    [SerializeField] private CanvasGroup diceRoot;
    [SerializeField] private TextMeshProUGUI switcherooText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image switcherooSign;
    [SerializeField] private Vector3 rotateVector;

    [Header("Colors")]
    [SerializeField] private Color switchColor;
    [SerializeField] private Color switcherooColor;
    [SerializeField] private Color bossColor;

    [Header("Death")]
    [SerializeField] private CanvasGroup deathPanel;
    [SerializeField] private Button restartButton;

    [Header("Start")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private Button startButton;

    [Header("Volume Master")]
    [SerializeField] private Image volumeIcon;
    [SerializeField] private Button volumeButton;
    [SerializeField] private Sprite volumeOn;
    [SerializeField] private Sprite volumeOff;

    [Header("Victory")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private Button startAgainButton;

    private Color skillBarColor;

    private Tween punchScaleKillTween;
    private Tween secondaryTween;

    private void Awake()
    {
        _canvasUI = this;

        skillBarColor = skillBar.color;

        _gameSystem.OnSwitcheroo += SwitcherooAnimation;
        _gameSystem.OnSwitch += SwitchAnimation;

        _gameSystem.OnLose += OnLose;
        _gameSystem.OnWin += OnWin;
        _gameSystem.OnBoss += OnBoss;

        restartButton.onClick.AddListener(Restart);
        startAgainButton.onClick.AddListener(Restart);
        volumeButton.onClick.AddListener(DisableVolume);

        switcherooSign.transform.DORotate(rotateVector, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        switcherooSign.DOFade(0, 0);

        startButton.onClick.AddListener(StartGame);

        UpdateLevel();
    }
    private void Start()
    {
        UpdateIconVolume(_gameSystem.Volume == 0 ? true : false);
    }
    private void UpdateIconVolume(bool enable)
    {
        volumeIcon.sprite = enable ? volumeOn : volumeOff;
    }

    private void DisableVolume()
    {
        bool newValue = _gameSystem.Volume == 0 ? false : true;

        UpdateIconVolume(newValue);

        _gameSystem.Mute(newValue);
    }

    private void OnLose()
    {
        deathPanel.DOFade(1, 1f);
        deathPanel.interactable = true;
        deathPanel.blocksRaycasts = true;
    }
    private void OnWin()
    {
        victoryPanel.DOFade(1, 1f);
        victoryPanel.interactable = true;
        victoryPanel.blocksRaycasts = true;

        var gamesWon = PlayerPrefs.GetInt(_gameSystem.GAME_WON_KEY, 0);
        gamesWon++;

        PlayerPrefs.SetInt(_gameSystem.GAME_WON_KEY, gamesWon);
    }
    private void OnBoss()
    {
        diceBar.DOColor(bossColor, 1f);
        diceBar.transform.DOPunchScale(Vector3.one * .5f, .3f);
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private void StartGame()
    {
        _gameSystem.StartGame();

        startPanel.gameObject.SetActive(false);

        diceRoot.DOFade(1, .5f);

        volumeButton.gameObject.SetActive(false);
    }

    public void UpdateTimeBar(float current, float max)
    {
        var time = max - current;
        timeText.text = time.ToString("0.0");
        timeBar.fillAmount = time / max;
    }
    public void UpdateHealthBar(float current, float max)
    {
        healthBar.fillAmount = current / max;
    }
    public void UpdateDiceBar(float value)
    {
        diceBar.fillAmount = value;
    }
    public void UpdateSkillBar(float value)
    {
        skillBar.fillAmount = value;
    }
    public void UpdateKills(int value)
    {
        if (punchScaleKillTween != null)
        {
            punchScaleKillTween.Goto(0, true);
            punchScaleKillTween.Kill();
        }

        killsText.text = $"{value}";
        punchScaleKillTween = killsText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
    }
    private void UpdateLevel()
    {
        levelText.text = $"LVL {PlayerPrefs.GetInt(_gameSystem.GAME_WON_KEY, 0) + 1}";
    }
    public void SecondaryReady()
    {
        skillBar.DOColor(Color.green, .5f);
        secondaryTween = skillBar.transform.DOScale(Vector3.one * 1.1f, .5f).SetLoops(-1, LoopType.Yoyo);
        skillBarCircle.DOFade(1, .5f);
        skillBarCircle.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
    }
    public void SecondaryUsed()
    {
        secondaryTween.Kill();
        secondaryTween.Goto(0, true);
        secondaryTween = null;

        skillBarCircle.DOFade(0, .5f);
    }
    public void SwitchAnimation()
    {
        switcherooText.text = "Switch";
        switcherooText.color = switchColor;
        StartCoroutine(SwitcherooAnimationCoroutine());
    }
    public void SwitcherooAnimation()
    {
        switcherooText.text = "SWITCHEROO";
        switcherooText.color = switcherooColor;
        StartCoroutine(SwitcherooAnimationCoroutine());
        switcherooSign.DOFade(switcherooSign.color.a == 1 ? 0 : 1, .5f);
    }

    private IEnumerator SwitcherooAnimationCoroutine()
    {
        switcherooText.DOFade(1, .8f);
        switcherooText.transform.DOScale(2.3082f, 1f);

        yield return new WaitForSeconds(1f);

        switcherooText.transform.DOScale(0f, 0.3f);
        switcherooText.DOFade(0, .3f);
    }
}