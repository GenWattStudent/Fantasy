using UnityEngine;

public class UIBuildingManager : MonoBehaviour
{
    [SerializeField] private SOBuilding[] buildings;
    [SerializeField] private GameObject buildingTabPrefab;
    private SOBuilding selectedBuilding;
    public static UIBuildingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ClearTabs() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public SOBuilding GetSelectedBuilding() {
        return selectedBuilding;
    }

    public void SetSelectedBuilding(SOBuilding building) {
        selectedBuilding = building;
    }

    public void CreateBuildingTabs(SOBuilding.BuildingType buildingType) {
        ClearTabs();
        UIUnitManager.Instance.IsUnitUIOpen = false;
        foreach (var bulding in buildings)
        {
            if (bulding.buildingType == buildingType)
            {
                CreateBuildingTab(bulding);
            }
        }
    }

    public void CreateBuildingTab(SOBuilding bulding) {
        GameObject buildingTab = Instantiate(buildingTabPrefab, transform);
        buildingTab.name = bulding.buildingName;

        var button = buildingTab.GetComponentInChildren<UnityEngine.UI.Image>();

        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            Debug.Log("Clicked on " + bulding.buildingName);
            selectedBuilding = bulding;
        });

        // Get last image in children
        UnityEngine.UI.Image[] images = buildingTab.GetComponentsInChildren<UnityEngine.UI.Image>();
        var image = images[images.Length - 1];

        if (image is not null) {
            image.sprite = bulding.buildingSprite;
        }
    }
}
