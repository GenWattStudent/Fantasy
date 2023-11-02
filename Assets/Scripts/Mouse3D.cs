using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask terrainLayer;

    public Vector3 GetMousePositionIn3DWorld()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, terrainLayer)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
}
