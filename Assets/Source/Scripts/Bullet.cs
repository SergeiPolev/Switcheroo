using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private OnTriggerEnterComponent onTriggerEnterComponent;

    public Rigidbody RB => rb;
    public OnTriggerEnterComponent OnTriggerEnterComponent => onTriggerEnterComponent;
}