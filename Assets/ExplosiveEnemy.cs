using UnityEngine;

public class ExplosiveEnemy : Enemy
{
    [SerializeField] private float radius = 4f;
    [SerializeField] private GameObject explosionVFX;

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

        var vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
    }
}