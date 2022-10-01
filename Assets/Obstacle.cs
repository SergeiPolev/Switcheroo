using System;
using UnityEngine;

using static GameData;

public class Obstacle : MonoBehaviour
{
    public Action OnDespawn;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _gameSystem.player.transform.position) > _gameSystem.DespawnDistance)
        {
            Despawn();

            Destroy(gameObject);
        }
    }
    private void Despawn()
    {
        OnDespawn?.Invoke();
    }
}