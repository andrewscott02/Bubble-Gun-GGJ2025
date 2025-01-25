using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Transform projectileSpawnTransform;

    [SerializeField]
    private Object projectileObject;

    public void Fire()
    {
        GameObject spawnedProjectile = GameObject.Instantiate(projectileObject, projectileSpawnTransform) as GameObject;

        spawnedProjectile.transform.parent = null;
        Bubble bubbleMovement = spawnedProjectile.GetComponent<Bubble>();

        bubbleMovement.ApplyProjectileMovement(Camera.main.transform.forward);
    }
}