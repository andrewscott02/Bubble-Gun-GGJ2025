using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    private PlayerController player;

    private NavMeshAgent agent;

    [SerializeField]
    private LayerMask environmentLayers, playerLayers, hazardLayers;

    [SerializeField]
    private GameObject deathFX;
    [SerializeField]
    private float hitForcePlayer = 250;
    [SerializeField]
    private float hitForceSelf = 600;

    [SerializeField]
    private float landForce = 1;
    [SerializeField]
    private float standDelay = 1;

    public float bubbleSizeMultiplier;

    private Rigidbody rb;

    public bool Encased { get; private set; } = false;

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
        else if (Utils.IsLayerInMask(playerLayers, layer) && !Encased)
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.Damage(1);
                ApplyKnockbackToSelf(other);
                ApplyKnockbackToPlayer(other);
            }
        }
        else if (Utils.IsLayerInMask(hazardLayers, layer) && !Encased)
        {
            Die();
        }
    }

    private void ApplyKnockbackToPlayer(Collider other)
    {
        Vector3 playerDir = other.transform.position - transform.position;
        playerDir.Normalize();
        playerDir.y = 0.5f;
        other.GetComponent<Rigidbody>().AddForce(playerDir * hitForcePlayer);
    }

    private void ApplyKnockbackToSelf(Collider other)
    {
        Vector3 enemyDir = transform.position - other.transform.position;
        enemyDir.Normalize();
        enemyDir.y = 0.5f;
        rb.AddForce(enemyDir * hitForceSelf);
    }

    private IEnumerator IDelayStand(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!Encased)
            StandUp();
    }

    private void StandUp()
    {
        rb.isKinematic = true;
        Vector3 rot = transform.rotation.eulerAngles;

        rot.x = rot.z = 0;

        transform.rotation = Quaternion.Euler(rot);

        rb.AddForce(Vector3.up * landForce);
        agent.enabled = true;
    }

    private void Die()
    {
        Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}