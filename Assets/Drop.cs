using DG.Tweening;
using UnityEngine;

public enum DropType
{
    Health,
    Rate,
    CircleShot,
    Switcheroo
}

public class Drop : MonoBehaviour
{
    [SerializeField] private DropType dropType;

    public DropType DropType => dropType;

    private void Awake()
    {
        transform.DORotate(Vector3.up * 180f, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(transform.position.y + 1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}