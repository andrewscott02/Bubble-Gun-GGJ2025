using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField]
    private LayerMask environmentLayers, hazardLayers;

    [SerializeField]
    private float explodeThreshold = 0.5f;

    [SerializeField]
    private Object explodeFX;
    [SerializeField]
    private float explodeRadius;

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (Utils.IsLayerInMask(environmentLayers, layer))
        {
            if (collision.impulse.magnitude > explodeThreshold)
                Explode();

        }
        else if (Utils.IsLayerInMask(hazardLayers, layer))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explodeFX, transform.position, Quaternion.identity);

        foreach (Collider collider in Physics.OverlapSphere(transform.position, explodeRadius))
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Die();
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);   
    }
}