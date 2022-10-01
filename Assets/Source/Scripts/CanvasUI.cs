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
    [SerializeField] private TextMeshProUGUI switcherooText;
    [SerializeField] private Image switcherooSign;
    [SerializeField] private Vector3 rotateVector;

    [Header("Colors")]
    [SerializeField] private Color switchColor;
    [SerializeField] private Color switcherooColor;

    [Header("Death")]
    [SerializeField] private CanvasGroup deathPanel;
    [SerializeField] private Button restartButton;


    private void Awake()
    {
        _canvasUI = this;

        _gameSystem.OnSwitcheroo += SwitcherooAnimation;
        _gameSystem.OnSwitch += SwitchAnimation;

        _gameSystem.OnLose += OnLose;

        restartButton.onClick.AddListener(Restart);

        switcherooSign.transform.DORotate(rotateVector, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        switcherooSign.DOFade(0, 0);
    }

    private void OnLose()
    {
        deathPanel.DOFade(1, 1f);
        deathPanel.interactable = true;
        deathPanel.blocksRaycasts = true;
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateTimeBar(float current, float max)
    {
        timeBar.fillAmount = current / max;
    }
    public void UpdateHealthBar(float current, float max)
    {
        healthBar.fillAmount = current / max;
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