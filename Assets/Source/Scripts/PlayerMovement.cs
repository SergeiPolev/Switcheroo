using UnityEngine;
using UnityEngine.AI;

using static GameData;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private NavMeshAgent meshAgent;

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (!_gameSystem.Playing)
        {
            return;
        }

        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        meshAgent.Move(moveVector.normalized * Time.deltaTime * speed);
    }
}