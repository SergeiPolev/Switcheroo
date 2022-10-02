using DG.Tweening;
using UnityEngine;

using static GameData;

public enum DropType
{
    Health,
    Rate,
    CircleShot,
    Switcheroo,
    SecondarySkill
}

public class Drop : MonoBehaviour
{
    [SerializeField] private DropType dropType;
    [SerializeField] private int minLevel = 0;

    public DropType DropType => dropType;
    public int MinLevel => minLevel;

    private void Awake()
    {
        if (_gameSystem.PassedLevels < minLevel)
        {
            Destroy(gameObject);
            return;
        }

        transform.DORotate(Vector3.up * 180f, 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(transform.position.y + 1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}