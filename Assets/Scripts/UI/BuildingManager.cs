using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private SOBulding[] buildings;
    [SerializeField] private GameObject buildingTabPrefab;
    private static BuildingManager instance;
    public static BuildingManager Instance { get { return instance; } }
    private SOBulding selectedBuilding;

    public void ClearBuldingTabs() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public SOBulding GetSelectedBuilding() {
        return selectedBuilding;
    }

    public void SetSelectedBuilding(SOBulding building) {
        selectedBuilding = building;
    }

    public void CreateBuildingTabs(SOBulding.BuildingType buildingType) {
        ClearBuldingTabs();
        foreach (var bulding in buildings)
        {
            if (bulding.buildingType == buildingType)
            {
                CreateBuildingTab(bulding);
            }
        }
    }

    public void CreateBuildingTab(SOBulding bulding) {
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

    public void CreateUnitTab(SOBulding sOBulding, ISpawnerBuilding spawnerBuilding) {
        ClearBuldingTabs();

        foreach(var soUnit in sOBulding.unitsToSpawn) {
            GameObject buildingTab = Instantiate(buildingTabPrefab, transform);
            buildingTab.name = soUnit.name;

            var button = buildingTab.GetComponentInChildren<UnityEngine.UI.Image>();

            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                spawnerBuilding.AddUnitToQueue(soUnit);
            });

            // Get last image in children
            UnityEngine.UI.Image[] images = buildingTab.GetComponentsInChildren<UnityEngine.UI.Image>();
            var image = images[images.Length - 1];

            if (image is not null) {
                image.sprite = soUnit.sprite;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
