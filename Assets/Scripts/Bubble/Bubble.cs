using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using System.Collections;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private GameObject bubbleVisual;

    [SerializeField]
    private LayerMask environmentLayers, enemyLayers;

    [SerializeField]
    private float projectileSpeed = 1;
    [SerializeField]
    private float encaseUpwardsSpeed = 1;
    [SerializeField]
    private float encaseVelocityMultiplier = 0.5f;

    [SerializeField]
    private float lifetime = 10;
    [SerializeField]
    private float encaseDuration = 10;

    private Rigidbody rb;

    private bool encasing;
    private GameObject encasedObject;

    private Coroutine destroyCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        destroyCoroutine = StartCoroutine(IDelayDestroy(lifetime));
    }

    private IEnumerator IDelayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (encasing)
        {
            encasedObject.transform.parent = null;
            Enemy enemy = encasedObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.StopEncase();
            }
        }

        Destroy(this.gameObject);
    }

    internal void ApplyProjectileMovement(Vector3 forward)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.AddForce(forward*projectileSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider other)
    {
        int layer = other.gameObject.layer;
        if (Utils.IsLayerInMask(environmentLayers, layer) && !encasing)
        {
            Destroy(this.gameObject);
        }
        else if (Utils.IsLayerInMask(enemyLayers, layer))
        {
            EncaseEnemy(other.gameObject);
        }
    }

    private void EncaseEnemy(GameObject enemyObject)
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        
        if (enemy.Encased)
            Destroy(this.gameObject);
        else
        {
            enemy.Encase();

            bubbleVisual.transform.localScale = new(
                enemy.bubbleSizeMultiplier,
                enemy.bubbleSizeMultiplier,
                enemy.bubbleSizeMultiplier
                );

            enemy.transform.parent = transform;
            enemy.transform.localPosition = Vector3.zero;

            rb.linearVelocity *= encaseVelocityMultiplier;
            rb.angularVelocity *= encaseVelocityMultiplier;

            rb.AddForce(Vector3.up * encaseUpwardsSpeed);

            encasing = true;
            encasedObject = enemy.gameObject;

            StopCoroutine(destroyCoroutine);
            StartCoroutine(IDelayDestroy(encaseDuration));
        }
    }
}