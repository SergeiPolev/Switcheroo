using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using static GameData;

public class Boss : MonoBehaviour, IHittable
{
    [SerializeField] private List<Health> bubbles;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform bubblesRoot;
    [SerializeField] private Health bossHealth;

    private int refreshNavMeshFrames = 4;

    private void Awake()
    {
        foreach (var bubble in bubbles)
        {
            bubble.Died += DeleteBubble;
        }

        bubblesRoot.DORotate(Vector3.up * 180, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    public virtual void Update()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_gameSystem.player.transform.position);
        }
    }
    private void DeleteBubble()
    {
        var allBubbles = new List<Health>(bubbles);

        foreach (var bubble in bubbles)
        {
            if (bubble.CurrentPoints <= 0)
            {
                allBubbles.Remove(bubble);
            }
        }

        bubbles = allBubbles;

        if (bubbles.Count == 0)
        {
            RemoveShield();
        }
    }

    private void RemoveShield()
    {
        shield.SetActive(false);
    }

    public void GetHit(float damage)
    {
        bossHealth.GetDamage(damage);
    }
}