using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health health;
    public float GetMaxHealth()
    {
        return health.MaxPoints;
    }
    public float GetHealth()
    {
        return health.CurrentPoints;
    }
    public void TakeDamage(float damage)
    {
        health.GetDamage(damage);
    }
}