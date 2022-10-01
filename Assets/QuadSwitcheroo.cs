using DG.Tweening;
using UnityEngine;

using static GameData;

public class QuadSwitcheroo : MonoBehaviour
{
    [SerializeField] private Vector3 switcherooRotation;

    private bool isInverted = false;

    private void Awake()
    {
        _gameSystem.OnSwitcheroo += Switcheroo;
    }
    private void Switcheroo()
    {
        transform.DOLocalRotate(isInverted ? Vector3.zero : switcherooRotation, 1f).SetEase(Ease.Linear);

        isInverted = !isInverted;
    }
}