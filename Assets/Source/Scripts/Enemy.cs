using UnityEngine;
using UnityEngine.AI;

using static GameData;

public class Enemy : MonoBehaviour, IHittable
{
    [SerializeField] private float speed = 4f;
    [SerializeField] internal float damage = 4f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float hitDistance = .5f;

    private bool isDead;
    private OnTriggerEnterComponent triggerEnterComponent;
    private Health health;
    private NavMeshAgent navMesh;
    private Collider currentCollider;
    private CharacterCanvas canvas;
    private int refreshNavMeshFrames = 3;

    private float attackTimer;

    internal virtual void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();
        health = GetComponent<Health>();
        navMesh = GetComponent<NavMeshAgent>();
        currentCollider = GetComponent<Collider>();
        canvas = GetComponentInChildren<CharacterCanvas>();

        navMesh.speed = speed;

        health.Died += Die;

        health.SetMaxPoints(health.MaxPoints * (1 + _gameSystem.PassedLevels / 10), true);
    }
    public virtual void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!_gameSystem.Playing)
        {
            return;
        }

        var playerPos = _gameSystem.player.transform.position;
        var distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        if (distanceToPlayer < hitDistance)
        {
            if (Time.time > attackTimer)
            {
                Attack();
            }
        }

        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(playerPos);

            if (Vector3.Distance(transform.position, playerPos) > _gameSystem.DespawnDistance)
            {
                DespawnEnemy();
            }
        }
    }

    internal virtual void Attack()
    {
        attackTimer = Time.time + attackDelay;

        _gameSystem.player.TakeDamage(damage);
    }

    internal void DespawnEnemy()
    {
        _enemySpawn.EnemyDied();

        isDead = true;

        Destroy(gameObject);
    }
    public void GetHit(float damage)
    {
        if (isDead)
        {
            return;
        }

        health.GetDamage(damage);
        canvas.UpdateFill(health.CurrentPoints / health.MaxPoints);
    }
    internal virtual void Die()
    {
        _enemySpawn.EnemyDied();

        isDead = true;

        navMesh.isStopped = true;
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        navMesh.enabled = false;

        canvas.HealthBar.gameObject.SetActive(false);

        currentCollider.enabled = false;

        Destroy(gameObject, 3f);
    }
}