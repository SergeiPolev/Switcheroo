using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static GameData;

public class CanvasUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image timeBar;
    [SerializeField] private TextMeshProUGUI switcherooText;

    [Header("Colors")]
    [SerializeField] private Color switchColor;
    [SerializeField] private Color switcherooColor;

    private void Awake()
    {
        _canvasUI = this;

        _gameSystem.OnSwitcheroo += SwitcherooAnimation;
        _gameSystem.OnSwitch += SwitchAnimation;
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