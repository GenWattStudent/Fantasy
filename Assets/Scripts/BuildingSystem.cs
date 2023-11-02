using Mirror;
using UnityEngine;

public class BuildingSystem : NetworkBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask terrainLayer;
    public SOBulding SelectedBuilding { get; private set; }
    private PlaceableBuilding placeableBuilding;
    private GameObject previewPrefab;
    private static BuildingSystem instance;
    public static BuildingSystem Instance { get { return instance; } }
    private bool wasValid = false;

    private void Awake() {
        instance = this;
        camera = Camera.main;
    }

    public void SetSelectedBuilding(SOBulding building) {
        SelectedBuilding = building;
        if (previewPrefab) Destroy(previewPrefab);
        previewPrefab = Instantiate(SelectedBuilding.buildingValidPrefab);
        placeableBuilding = previewPrefab.GetComponent<PlaceableBuilding>();
    }

    private Vector3 GetMouseWorldPosition() {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, terrainLayer)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }

    private void BuildingPreview() {
        if (SelectedBuilding is null) return;

        var mousePosition =  GetMouseWorldPosition();
        bool isValid = IsValidPosition();

        if (isValid != wasValid) {
            Destroy(previewPrefab);
            if (isValid) {
                previewPrefab = Instantiate(SelectedBuilding.buildingValidPrefab);
            } else {
                previewPrefab = Instantiate(SelectedBuilding.buildingInvalidPrefab);
            }

            placeableBuilding = previewPrefab.GetComponent<PlaceableBuilding>();
            wasValid = isValid;
        }

        previewPrefab.transform.position = mousePosition;
    }

    [Command]
    private void CmdPlaceBuilding(int index, Vector3 position) {
        Debug.Log("CmdPlaceBuilding");
        var prefab = NetworkManager.singleton.spawnPrefabs[index];
        if (prefab is null) return;
        var newBuilding = Instantiate(prefab, position, Quaternion.identity);
        NetworkServer.Spawn(newBuilding, connectionToClient);
    }

    private void PlaceBuilding() {
        if (Input.GetMouseButtonDown(0) && IsValidPosition()) {
            Debug.Log("PlaceBuilding " + SelectedBuilding.buildingPrefab.name);
            CmdPlaceBuilding(0, previewPrefab.transform.position);
            CancelBuilding();
        }
    }

    private bool IsValidPosition() {
        return placeableBuilding != null && placeableBuilding.colliders.Count == 0;
    }

    public void CancelBuilding() {
        Destroy(previewPrefab);
        SelectedBuilding = null;
        BuildingManager.Instance.SetSelectedBuilding(null);
    }

    private void CheckSelectedBuilding() {
        var selectedBuilding = BuildingManager.Instance.GetSelectedBuilding();

        if (selectedBuilding != null && selectedBuilding != SelectedBuilding) {
            SetSelectedBuilding(selectedBuilding);
        }
    }

    [ClientCallback]
    private void Update() {
        // Debug.Log(transform.name + " Update " + isOwned + " " + SelectedBuilding);
        if (!isOwned && SelectedBuilding == null) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
            CancelBuilding();
        }

        // Debug.Log(transform.name + " Update");

        CheckSelectedBuilding();
        PlaceBuilding();
        BuildingPreview();
    }
}
