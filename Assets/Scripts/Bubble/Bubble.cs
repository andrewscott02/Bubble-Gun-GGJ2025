using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private GameObject bubbleVisual;
    private SphereCollider col;

    [SerializeField]
    private LayerMask environmentLayers, encasableLayers, hazardLayers;

    [SerializeField]
    private Object bubblePopFX;

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

    [SerializeField]
    private float basePopThreshold = 0.5f;
    [SerializeField]
    private float encasePopThreshold = 0.5f;
    private float collisionDamage = 0;

    private Rigidbody rb;

    private bool encasing;
    private List<GameObject> encasedObjects = new();

    private Coroutine destroyCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

        destroyCoroutine = StartCoroutine(IDelayDestroy(lifetime));
    }

    private IEnumerator IDelayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyBubble();
    }

    private void DestroyBubble()
    {
        if (encasing)
        {
            foreach (GameObject encasedObject in encasedObjects)
            {
                encasedObject.transform.parent = null;

                if (encasedObject.TryGetComponent<Encasable>(out Encasable encasable))
                {
                    encasable.StopEncase();
                }
                else throw new System.Exception("Object does not have an encasable script");
            }
        }

        Instantiate(bubblePopFX, transform.position, Quaternion.identity);
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
        int layer = collision.collider.gameObject.layer;
        if (Utils.IsLayerInMask(environmentLayers, layer))
        {
            float popThreshold = encasing ? encasePopThreshold : basePopThreshold;

            if (collision.impulse.magnitude+ collisionDamage > popThreshold)
                DestroyBubble();
            else
                collisionDamage += collision.impulse.magnitude * 2;

        }
        else if (Utils.IsLayerInMask(encasableLayers, layer))
        {
            EncaseObject(collision.collider.gameObject);
        }
        else if (Utils.IsLayerInMask(hazardLayers, layer))
        {
            DestroyBubble();
        }
    }

    private void EncaseObject(GameObject encasableObject)
    {
        Encasable encasable = encasableObject.GetComponent<Encasable>();
        
        if (!encasable.Encased && !encasedObjects.Contains(encasableObject))
        {
            encasable.Encase();
            ResizeBubble(encasable);
            SetEncasedObjectTransform(encasableObject);
            SetEncaseForces();

            encasing = true;
            encasedObjects.Add(encasable.gameObject);

            StopCoroutine(destroyCoroutine);
            StartCoroutine(IDelayDestroy(encaseDuration));
        }
    }

    private void ResizeBubble(Encasable encasable)
    {
        bubbleVisual.transform.localScale = new(
                        encasable.bubbleSizeMultiplier,
                        encasable.bubbleSizeMultiplier,
                        encasable.bubbleSizeMultiplier
                        );

        col.radius = encasable.bubbleSizeMultiplier / 2;
    }

    private void SetEncasedObjectTransform(GameObject encasedObject)
    {
        encasedObject.transform.SetParent(transform, true);

        Vector3 desiredPos = Vector3.zero;
        desiredPos.y = -encasedObject.GetComponent<Collider>().bounds.size.y/2;

        encasedObject.transform.localPosition = desiredPos;
        encasedObject.transform.rotation = Quaternion.identity;
    }

    private void SetEncaseForces()
    {
        rb.linearVelocity *= encaseVelocityMultiplier;
        rb.angularVelocity *= encaseVelocityMultiplier;

        rb.AddForce(Vector3.up * encaseUpwardsSpeed);
    }
}