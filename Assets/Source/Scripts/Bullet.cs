using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private OnTriggerEnterComponent onTriggerEnterComponent;
    [SerializeField] private float lifeTime = 3f;


    private float timer;

    public Rigidbody RB => rb;
    public OnTriggerEnterComponent OnTriggerEnterComponent => onTriggerEnterComponent;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}