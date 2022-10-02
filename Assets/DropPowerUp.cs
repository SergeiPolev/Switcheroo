using UnityEngine;

public class DropPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] drops;
    [SerializeField] private float dropChance;

    private void Awake()
    {
        GetComponent<Health>().Died += Drop;
    }

    private void Drop()
    {
        if (drops.Length > 0)
        {
            if (Random.Range(0, 100) < dropChance)
            {
                Instantiate(drops[Random.Range(0, drops.Length)], transform.position, Quaternion.identity);
            }
        }
    }
}