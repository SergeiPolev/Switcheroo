using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvas : MonoBehaviour
{
    public Image HealthBar;
    public CanvasGroup HealthBarRoot;

    private Camera currentCamera;

    private void Awake()
    {
        currentCamera = Camera.main;

        HealthBarRoot.DOFade(0, 0);
    }

    private void LateUpdate()
    {
        transform.forward = currentCamera.transform.forward;
    }

    public void UpdateFill(float value)
    {
        HealthBar.fillAmount = value;

        HealthBarRoot.DOFade(1, .1f);
        HealthBarRoot.DOFade(0, .5f).SetDelay(2f);
    }
}