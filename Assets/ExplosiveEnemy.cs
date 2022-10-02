using UnityEngine;

public class ExplosiveEnemy : Enemy
{
    [SerializeField] private float radius = 4f;
    internal override void Attack()
    {
        var colliders = Physics.OverlapSphere(transform.position, 4f);

        foreach (var item in colliders)
        {
            if (item.CompareTag("Player") || item.CompareTag("Enemy"))
            {
                var health = item.GetComponent<Health>();

                if (health != null)
                {
                    health.GetDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}