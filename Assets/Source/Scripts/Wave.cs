using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave Settings")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int minLevel;

    public GameObject[] Enemies => enemies;
    public int MinLevel => minLevel;
}