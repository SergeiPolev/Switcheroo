using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using static GameData;

public class Boss : MonoBehaviour, IHittable
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private Animator animator;
    [SerializeField] private float shootForce = 1000f;
    [SerializeField] private float shootDelay = 1.4f;
    [SerializeField] private float shootDamage = 10f;
    [SerializeField] private int shootAmount = 10;

    [SerializeField] private List<Health> bubbles;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform bubblesRoot;
    [SerializeField] private Transform bossModelRoot;
    [SerializeField] private Health bossHealth;

    private int refreshNavMeshFrames = 4;

    private bool canFire = false;
    
    private float fireTimer;

    private void Awake()
    {
        foreach (var bubble in bubbles)
        {
            bubble.Died += DeleteBubble;
        }

        bubblesRoot.DOLocalRotate(Vector3.up * 180, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        bossModelRoot.DORotate(Vector3.up * -180, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    public virtual void Update()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        if (canFire)
        {
            if (Time.time > fireTimer)
            {
                ShootAnimation();

                fireTimer = Time.time + shootDelay;
            }
        }

        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_gameSystem.player.transform.position);
        }
    }

    public void GetHit(float damage)
    {
        bossHealth.GetDamage(damage);
    }

    public Health GetHealth()
    {
        return bossHealth;
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

        canFire = true;
    }
    private void ShootAnimation()
    {
        animator.SetTrigger("Shoot");
    }
    private void Fire()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        var angleStep = 360 / shootAmount;

        for (int i = 0; i < shootAmount; i++)
        {
            Shoot(GivePositionInCircle(angleStep * i));
        }

        fireTimer = Time.time + shootDelay;
    }
    private Vector3 GivePositionInCircle(float degree)
    {
        var radians = degree * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);

        return new Vector3(x, 0, y);
    }
    private void Shoot(Vector3 fireVector)
    {
        var shootPos = transform.position;
        shootPos.y = _gameSystem.player.transform.position.y;

        var bullet = Instantiate(bulletPrefab, shootPos, Quaternion.identity);

        _gameSystem.PlayShot(shotClip, .4f);

        bullet.OnTriggerEnterComponent.OnEnter += Hit;

        bullet.RB.AddForce(fireVector * shootForce);

    }

    private void Hit(Transform other, Transform @object)
    {
        if (other.CompareTag("Player"))
        {
            var hittable = other.GetComponent<IHittable>();

            hittable.GetHit(shootDamage);
        }

        @object.GetComponent<Bullet>().OnTriggerEnterComponent.OnEnter -= Hit;
        Destroy(@object.gameObject);
    }
}