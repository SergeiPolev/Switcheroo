using System;
using System.Collections.Generic;
using UnityEngine;

using static GameData;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootDelay = .5f;
    [SerializeField] private float shootSecondaryDelay = 10f;
    [SerializeField] private float shootSecondaryRadius = 5f;
    [SerializeField] private float shootSecondaryDamage = 5f;
    [SerializeField] private float shootForce = 30f;
    [SerializeField] private float shootDamage = 5f;
    [SerializeField] private Transform[] shootSides;
    [SerializeField] private Transform firstShootSide;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gun;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Bullet spreadBulletPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip shotClip;

    private Camera currentCamera;

    private Transform currentShootPoint = null;
    private Transform lastShootPoint = null;

    private float timer;
    private float secondaryTimer;
    private float modifierTimer;

    private bool IsReverseAim = false;

    private float fireRateModifier = 1f;

    private void Awake()
    {
        currentCamera = Camera.main;

        currentShootPoint = lastShootPoint = firstShootSide;

        InitPoints();

        _gameSystem.OnSwitch += SwitchShootPoint;
        _gameSystem.OnSwitcheroo += Switcheroo;
    }
    private void InitPoints()
    {
        foreach (var point in shootSides)
        {
            var lookPoint = (point.position - transform.position) * 100f;
            lookPoint.y = point.position.y;

            point.LookAt(lookPoint);
        }
    }
    private void OnDestroy()
    {
        if (_gameSystem != null)
        {
            _gameSystem.OnSwitch -= SwitchShootPoint;
            // _gameSystem.OnSwitcheroo -= Switcheroo;
        }
    }

    private void Update()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        Aim();

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= timer)
            {
                Shoot();
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time >= secondaryTimer)
            {
                SecondarySkill();

                secondaryTimer = Time.time + shootSecondaryDelay;
            }
        }

        if (fireRateModifier != 1f && modifierTimer <= Time.time)
        {
            ResetModifier();
        }

        _canvasUI.UpdateSkillBar((shootSecondaryDelay - (secondaryTimer - Time.time)) / shootSecondaryDelay);
    }

    private void SecondarySkill()
    {
        var enemies = Physics.OverlapSphere(transform.position, shootSecondaryRadius, _gameSystem.EnemyLayer);

        foreach (var enemy in enemies)
        {
            var health = enemy.GetComponent<Health>();

            if (health != null)
            {
                health.GetDamage(shootSecondaryDamage);
            }
        }
    }
    private void ResetModifier()
    {
        fireRateModifier = 1f;
    }
    public void SetModifier(float modifier, float duration)
    {
        modifierTimer = Time.time + duration;

        fireRateModifier = modifier;
    }
    private void SwitchShootPoint()
    {
        List<Transform> newPoints = new List<Transform>();

        foreach (var point in shootSides)
        {
            if (point == lastShootPoint)
            {
                continue;
            }

            newPoints.Add(point);
        }

        lastShootPoint = currentShootPoint;

        var newPoint = newPoints.Count > 0 ? newPoints[UnityEngine.Random.Range(0, newPoints.Count)] : firstShootSide;
        currentShootPoint = newPoint;
        gun.forward = newPoint.forward;
    }
    private void Switcheroo()
    {
        IsReverseAim = !IsReverseAim;

        if (IsReverseAim)
        {
            currentShootPoint = firstShootSide;
            gun.forward = currentShootPoint.forward;
        }
    }
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        _gameSystem.PlayShot(shotClip);

        bullet.OnTriggerEnterComponent.OnEnter += Hit;

        bullet.RB.AddForce(currentShootPoint.forward * shootForce);

        timer = Time.time + shootDelay * fireRateModifier;
    }

    public void ShootFromEachSide()
    {
        _gameSystem.PlayShot(shotClip);

        foreach (var side in shootSides)
        {
            var bullet = Instantiate(spreadBulletPrefab, side.transform.position, Quaternion.identity);

            bullet.OnTriggerEnterComponent.OnEnter += HitSpread;

            bullet.RB.AddForce(side.forward * shootForce);
        }
    }
    private void Aim()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            var lookPoint = hit.point;

            lookPoint.y = transform.position.y;

            if (IsReverseAim)
            {
                Vector3 forwardToPoint = transform.position - lookPoint;

                var degree = Vector3.Angle(Vector3.forward, forwardToPoint);
                var reverseDegree = degree * Math.Sign(Vector3.Dot(forwardToPoint, Vector3.left));

                var rotation = transform.rotation;
                rotation.eulerAngles = new Vector3(0, reverseDegree, 0);

                transform.rotation = rotation;
            }
            else
            {
                transform.LookAt(lookPoint);
            }
        }
    }

    private void Hit(Transform other, Transform @object)
    {
        if (other.CompareTag("Enemy"))
        {
            var hittable = other.GetComponent<IHittable>();

            hittable.GetHit(shootDamage);
        }

        @object.GetComponent<Bullet>().OnTriggerEnterComponent.OnEnter -= Hit;
        Destroy(@object.gameObject);
    }
    private void HitSpread(Transform other, Transform @object)
    {
        if (other.CompareTag("Enemy"))
        {
            var hittable = other.GetComponent<IHittable>();

            hittable.GetHit(shootDamage * 3f);
        }
    }
}