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

    [Header("Death")]
    [SerializeField] private CanvasGroup deathPanel;
    [SerializeField] private Button restartButton;

    [Header("Start")]
    [SerializeField] private CanvasGroup startPanel;
    [SerializeField] private Button startButton;

    [Header("Victory")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private Button startAgainButton;


    private Tween punchScaleKillTween;

    private void Awake()
    {
        _canvasUI = this;

        _gameSystem.OnSwitcheroo += SwitcherooAnimation;
        _gameSystem.OnSwitch += SwitchAnimation;

        _gameSystem.OnLose += OnLose;
        _gameSystem.OnWin += OnWin;

        restartButton.onClick.AddListener(Restart);
        startAgainButton.onClick.AddListener(Restart);

        switcherooSign.transform.DORotate(rotateVector, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        switcherooSign.DOFade(0, 0);

        startButton.onClick.AddListener(StartGame);

        UpdateLevel();
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

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private void StartGame()
    {
        _gameSystem.StartGame();

        startPanel.gameObject.SetActive(false);

        diceRoot.DOFade(1, .5f);
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
        switcherooText.transform.DOScale(1.3f, 1f);

        yield return new WaitForSeconds(1f);

        switcherooText.transform.DOScale(0f, 0.3f);
        switcherooText.DOFade(0, .3f);
    }
}