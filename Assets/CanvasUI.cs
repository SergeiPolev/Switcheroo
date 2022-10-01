using UnityEngine;
using UnityEngine.UI;

using static GameData;

public class CanvasUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image timeBar;

    private void Awake()
    {
        _canvasUI = this;
    }

    public void UpdateTimeBar(float current, float max)
    {
        timeBar.fillAmount = current / max;
    }
    public void UpdateHealthBar(float current, float max)
    {
        healthBar.fillAmount = current / max;
    }
}