using UnityEngine;

public class Bubble : MonoBehaviour, IHittable
{
    [SerializeField] private Health health;
    public void GetHit(float damage)
    {
        health.GetDamage(damage);
    }
}