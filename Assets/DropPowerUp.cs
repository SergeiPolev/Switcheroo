using UnityEngine;

using static GameData;

public class DropPowerUp : MonoBehaviour
{
    [SerializeField] private float dropChance;

    private void Awake()
    {
        GetComponent<Health>().Died += Drop;
    }

    private void Drop()
    {
        if (_gameSystem.Drops.Count > 0)
        {
            if (Random.Range(0, 100) < dropChance)
            {
                var drop = _gameSystem.Drops[Random.Range(0, _gameSystem.Drops.Count)];

                var enemyPos = transform.position;
                enemyPos.y = _gameSystem.player.transform.position.y;

                Instantiate(drop, enemyPos, Quaternion.identity);
            }
        }
    }
}