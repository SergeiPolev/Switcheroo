using UnityEngine;

using static GameData;

public class ExplosiveEnemy : Enemy
{
    [SerializeField] private float radius = 4f;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private AudioClip explosionClip;

    internal override void Attack()
    {
        var colliders = Physics.OverlapSphere(transform.position, 4f);

        foreach (var item in colliders)
        {
            if (item.CompareTag("Player") || item.CompareTag("Enemy"))
            {
                var health = item.GetComponent<IHittable>();

                if (health != null)
                {
                    health.GetHit(damage);
                }
            }
        }

        Destroy(gameObject);

        _gameSystem.PlayShot(explosionClip, 1.5f);

        var vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
    }
    internal override void Die()
    {
        DespawnEnemy();
        Attack();
    }
}