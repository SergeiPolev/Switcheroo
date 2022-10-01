using UnityEngine;
using UnityEngine.AI;

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
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        meshAgent.Move(moveVector.normalized * Time.deltaTime * speed);
    }
}