using UnityEngine;

public class Encasable : MonoBehaviour
{
    protected Rigidbody rb;

    public bool Encased { get; protected set; } = false;

    public float bubbleSizeMultiplier;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    internal virtual void Encase()
    {
        Encased = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    internal virtual void StopEncase()
    {
        Encased = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}