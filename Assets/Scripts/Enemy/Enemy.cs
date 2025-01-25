using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private LayerMask environmentLayers;

    [SerializeField]
    private float landForce = 1;

    public float bubbleSizeMultiplier;

    public bool Encased {  get; private set; } = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Encase()
    {
        Encased = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    internal void StopEncase()
    {
        Encased = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.collider);
    }

    private void CheckCollision(Collider other)
    {
        int layer = other.gameObject.layer;
        if (Utils.IsLayerInMask(environmentLayers, layer) && !Encased)
        {
            Vector3 rot = transform.rotation.eulerAngles;

            rot.x = rot.z = 0;

            transform.rotation = Quaternion.Euler(rot);

            rb.AddForce(Vector3.up * landForce);
        }
    }
}