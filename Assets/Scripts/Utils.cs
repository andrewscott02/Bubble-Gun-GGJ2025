using UnityEngine;

public static class Utils
{
    public static bool IsLayerInMask(LayerMask layerMask, int layer)
        => (layerMask & (1 << layer)) != 0;

    public static Vector3 LerpVector3(Vector3 a, Vector3 b, float t)
        => new
        (
            Mathf.Lerp(a.x, b.x, t),
            Mathf.Lerp(a.y, b.y, t),
            Mathf.Lerp(a.z, b.z, t)
        );
}