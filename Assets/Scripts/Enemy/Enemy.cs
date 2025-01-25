using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private PlayerController player;

    private NavMeshAgent agent;

    [SerializeField]
    private LayerMask environmentLayers;

    [SerializeField]
    private float landForce = 1;
    [SerializeField]
    private float standDelay = 1;

    public float bubbleSizeMultiplier;

    public bool Encased {  get; private set; } = false;

    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!Encased && agent.enabled)
            agent.SetDestination(player.transform.position);
    }

    public void Encase()
    {
        Encased = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        agent.enabled = false;
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
            StartCoroutine(IDelayStand(standDelay));
        }
    }

    private IEnumerator IDelayStand(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!Encased)
            StandUp();
    }

    private void StandUp()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        rot.x = rot.z = 0;

        transform.rotation = Quaternion.Euler(rot);

        rb.AddForce(Vector3.up * landForce);
        agent.enabled = true;
    }
}