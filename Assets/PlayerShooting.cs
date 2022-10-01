using System;
using System.Collections.Generic;
using UnityEngine;

using static GameData;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootDelay = .5f;
    [SerializeField] private float shootForce = 30f;
    [SerializeField] private float shootDamage = 5f;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private Transform firstShootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private LayerMask groundLayer;

    private Camera currentCamera;

    private Transform currentShootPoint = null;
    private Transform lastShootPoint = null;

    private float timer;

    private bool IsReverseAim = true;

    private void Awake()
    {
        currentCamera = Camera.main;

        currentShootPoint = lastShootPoint = firstShootPoint;

        InitPoints();

        _gameSystem.OnSwitch += SwitchShootPoint;
    }
    private void InitPoints()
    {
        foreach (var point in shootPoints)
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
        }
    }

    private void Update()
    {
        Aim();

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= timer)
            {
                Shoot();
            }
        }
    }
    private void SwitchShootPoint()
    {
        List<Transform> newPoints = new List<Transform>();

        foreach (var point in shootPoints)
        {
            if (point == lastShootPoint)
            {
                continue;
            }

            newPoints.Add(point);
        }

        lastShootPoint = currentShootPoint;

        var newPoint = newPoints.Count > 0 ? newPoints[UnityEngine.Random.Range(0, newPoints.Count)] : firstShootPoint;
        currentShootPoint = newPoint;

        IsReverseAim = !IsReverseAim;
    }
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, currentShootPoint.position, Quaternion.identity);

        bullet.OnTriggerEnterComponent.OnEnter += Hit;

        bullet.RB.AddForce(currentShootPoint.forward * shootForce);

        timer = Time.time + shootDelay;
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
            var enemy = other.GetComponent<Enemy>();

            enemy.GetHit(shootDamage);
        }

        @object.GetComponent<Bullet>().OnTriggerEnterComponent.OnEnter -= Hit;
        Destroy(@object.gameObject);
    }
}