using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave Settings")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] enemies;

    public GameObject[] Enemies => enemies;
}