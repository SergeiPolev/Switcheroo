using UnityEngine;

using static GameData;

public class Player : MonoBehaviour, IHittable
{
    [SerializeField] private Health health;
    [SerializeField] private ParticleSystem congrats;

    public Health Health => health;

    private void Awake()
    {
        _gameSystem.OnWin += ActivateCongrats;
    }
    private void ActivateCongrats()
    {
        congrats.gameObject.SetActive(true);
    }
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

    public void GetHit(float damage)
    {
        health.GetDamage(damage);
    }
}