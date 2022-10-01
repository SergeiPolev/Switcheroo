using UnityEngine;

using static GameData;

public class Bubble : MonoBehaviour, IHittable
{
    [SerializeField] private Health health;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private float shootForce = 1000f;
    [SerializeField] private float shootDelay = 1.4f;
    [SerializeField] private float shootDamage = 5f;

    private float timer;

    private void Update()
    {
        if (timer < Time.time)
        {
            Shoot();
        }
    }
    public void GetHit(float damage)
    {
        health.GetDamage(damage);
    }
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        _gameSystem.PlayShot(shotClip, .4f);

        bullet.OnTriggerEnterComponent.OnEnter += Hit;

        bullet.RB.AddForce(transform.forward * shootForce);

        timer = Time.time + shootDelay;
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