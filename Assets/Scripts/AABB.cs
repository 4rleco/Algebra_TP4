using UnityEngine;
using UnityEngine.UIElements;

public class AABB : MonoBehaviour
{
    [SerializeField] Frustrum frustrum;

    private Vector3 localCenter;
    private Vector3 localExtents;

    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private bool isVisible = false;

    void Start()
    {
        // Calcular una sola vez en local space
        mesh = GetComponentInChildren<MeshFilter>().sharedMesh;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        Bounds localBounds = mesh.bounds;

        localCenter = localBounds.center;
        localExtents = localBounds.extents;
    }

    void OnDrawGizmos()
    {
        if (GetComponentInChildren<MeshFilter>() == null) return;

        if (mesh == null) return;

        if (localExtents == Vector3.zero)
        {
            Bounds localBounds = mesh.bounds;
            localCenter = localBounds.center;
            localExtents = localBounds.extents;
        }

        // Matriz de transformaci�n actual
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        // Centro en mundo
        Vector3 worldCenter = localToWorld.MultiplyPoint3x4(localCenter);

        // Extraemos ejes
        Vector3 right = new Vector3(localToWorld.m00, localToWorld.m01, localToWorld.m02);
        Vector3 up = new Vector3(localToWorld.m10, localToWorld.m11, localToWorld.m12);
        Vector3 forward = new Vector3(localToWorld.m20, localToWorld.m21, localToWorld.m22);

        // Valor absoluto
        right = new Vector3(Mathf.Abs(right.x), Mathf.Abs(right.y), Mathf.Abs(right.z));
        up = new Vector3(Mathf.Abs(up.x), Mathf.Abs(up.y), Mathf.Abs(up.z));
        forward = new Vector3(Mathf.Abs(forward.x), Mathf.Abs(forward.y), Mathf.Abs(forward.z));

        // Extents en mundo
        Vector3 worldExtents =
            right * localExtents.x +
            up * localExtents.y +
            forward * localExtents.z;

        // Bounds en mundo
        Bounds worldBounds = new Bounds(worldCenter, worldExtents * 2f);

        // Dibujar
        if (isVisible)        
            Gizmos.color = Color.green;         
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }

}
