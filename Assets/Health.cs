using System;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Hit Flash")]
    [SerializeField] private Gradient gradient;

    [SerializeField] private float _maxPoints;

    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _deathEffect;

    public event Action Died;
    public event Action<float> Damaged;
    public event Action PointsChanged;

    private Tween flashTween;

    public GameObject LastAttacker { get; private set; }

    public float CurrentPoints
    {
        get => _currentPoints;
        private set
        {
            _currentPoints = value;
            PointsChanged?.Invoke();
        }
    }
    private float _currentPoints;

    public float MaxPoints
    {
        private set => _maxPoints = value;
        get
        {
            return _maxPoints;
        }
    }

    private void OnDrawGizmos()
    {
        if (_maxPoints <= 0)
            _maxPoints = 1;
    }

    private void Awake()
    {
        CurrentPoints = MaxPoints;
    }

    public void GetDamage(float damagePoints)
    {
        if (damagePoints <= 0)
        {
            throw new Exception("Negative damage points");
        }

        flashTween.Goto(0, true);
        flashTween.Kill();

        if (CurrentPoints <= 0)
            return;

        CurrentPoints -= damagePoints;

        if (CurrentPoints > 0)
        {
            Spawn(_hitEffect);

            Damaged?.Invoke(damagePoints);
        }
        else
        {
            Died?.Invoke();

            Death();
        }
    }

    private void Death()
    {
        Spawn(_deathEffect);
        Destroy(gameObject);
    }
    private void Spawn(GameObject prefab)
    {
        if (prefab == null)
            return;

        GameObject instance = Instantiate(prefab, transform.position,
            transform.rotation, null);

        Destroy(instance, 2f);
    }

    public void Heal(float healPoints)
    {
        if (healPoints <= 0)
            throw new Exception("Negative heal points");

        CurrentPoints = Math.Min(CurrentPoints + healPoints, _maxPoints);
    }

    public void SetMaxPoints(float newMaxPoints, bool needRefill = false)
    {
        if (newMaxPoints <= 0)
            throw new Exception("Negative max points");

        MaxPoints = newMaxPoints;

        if (needRefill)
            CurrentPoints = MaxPoints;
    }
}