using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUnitManager : MonoBehaviour
{
    [SerializeField] private GameObject unitSlotTabPrefab;
    private List<GameObject> unitSlotTabs = new ();
    private List<SoUnit> unitsAttachedToTab = new ();
    public static UIUnitManager Instance { get; private set; }
    private SOBuilding selectedBuilding;
    private ISpawnerBuilding spawnerBuilding;
    public bool IsUnitUIOpen { get; set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate() {
        if (IsUnitUIOpen && spawnerBuilding is not null) {
            foreach (var unitTab in unitSlotTabs) {
                var nameText = unitTab.GetComponentsInChildren<TextMeshProUGUI>()[0];
                var currentTime = spawnerBuilding.GetSpawnTimer();
                var currentSpawningUnit = spawnerBuilding.GetCurrentSpawningUnit();
                var unitQueueCount = spawnerBuilding.GetUnitQueueCountByName(unitTab.name);
                var soUnit = unitsAttachedToTab.Find(x => x.unitName == unitTab.name);
    
                if (currentSpawningUnit is not null && currentSpawningUnit.unitName == unitTab.name) {
                    Debug.Log("currentSpawningUnit2 " + currentSpawningUnit.unitName);
                    var spawnPanel = unitTab.GetComponentInChildren<SpawnPanel>(true);
                    spawnPanel.SetSpawnData(soUnit, unitQueueCount, currentTime);
                }

                if (unitQueueCount <= 0) {
                    var spawnPanel = unitTab.GetComponentInChildren<SpawnPanel>(true);
                    if (!spawnPanel) return;
                    spawnPanel.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetUnitData(GameObject unitTab, SoUnit soUnit) {
        var unitNameText = unitTab.GetComponentsInChildren<TextMeshProUGUI>()[0];
        var costText = unitTab.GetComponentsInChildren<TextMeshProUGUI>()[1];
        var button = unitTab.GetComponentInChildren<UnityEngine.UI.Image>();

        unitNameText.text = soUnit.unitName;
        costText.text = soUnit.cost.ToString();

        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            spawnerBuilding.AddUnitToQueue(soUnit);
        });

        UnityEngine.UI.Image[] images = unitTab.GetComponentsInChildren<UnityEngine.UI.Image>();
        var image = images[1];

        if (image is not null) {
            image.sprite = soUnit.sprite;
        }
    }

    private void ClearTabs() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateUnitTabs(SOBuilding soBuilding, ISpawnerBuilding spawnerBuilding) {
        ClearTabs();
        unitSlotTabs.Clear();
        unitsAttachedToTab.Clear();
        this.selectedBuilding = soBuilding;
        this.spawnerBuilding = spawnerBuilding;

        var currentTime = spawnerBuilding.GetSpawnTimer();
        var currentSpawningUnit = spawnerBuilding.GetCurrentSpawningUnit();

        foreach(var soUnit in soBuilding.unitsToSpawn) {
            GameObject unitTab = Instantiate(unitSlotTabPrefab, transform);
            unitTab.name = soUnit.unitName;
            var unitQueueCount = spawnerBuilding.GetUnitQueueCountByName(soUnit.unitName);
            Debug.Log("Current: " + currentSpawningUnit + "Count: " + unitQueueCount + " CurrentSpawn:" + currentSpawningUnit + soUnit.unitName);
            if (currentSpawningUnit is not null && currentSpawningUnit.unitName == soUnit.unitName) {
                var spawnPanel = unitTab.GetComponentInChildren<SpawnPanel>(true);
                spawnPanel.SetSpawnData(soUnit, unitQueueCount, currentTime);
            } else if (unitQueueCount > 0) {
                var spawnPanel = unitTab.GetComponentInChildren<SpawnPanel>(true);
                Debug.Log("spawnPanel " + spawnPanel);
                spawnPanel.SetSpawnData(soUnit, unitQueueCount, currentTime);
            }

            SetUnitData(unitTab, soUnit);
            unitSlotTabs.Add(unitTab);
            unitsAttachedToTab.Add(soUnit);
        }

        IsUnitUIOpen = true;
    }
}
