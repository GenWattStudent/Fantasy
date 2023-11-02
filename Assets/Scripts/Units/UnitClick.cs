using UnityEngine;
using UnityEngine.InputSystem;

public class UnitClick : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && UnitSelection.Instance is not null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                UnitSelection.Instance.DeselectAllUnits();
                return;
            }

            Unit unit = hit.collider.GetComponent<Unit>();
            UnitSelection.Instance.DeselectAllUnits();
            UnitSelection.Instance.Select(unit);
        } 
    }
}
