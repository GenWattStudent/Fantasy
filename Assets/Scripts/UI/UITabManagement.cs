using System.Collections.Generic;
using UnityEngine;

public class UITabManagement : MonoBehaviour
{
    private List<GameObject> tabs = new List<GameObject>();
    [SerializeField] private GameObject tabButtonPrefab;
    public string CurrentTab { get; private set; }

    private void CreateTabs()
    {
        // Make tabs from enum SoBulding
        var unitTypes = System.Enum.GetValues(typeof(SOBulding.BuildingType));

        foreach (var unitType in unitTypes)
        {
            // Create tab 
            GameObject tab = Instantiate(tabButtonPrefab, transform);
            tab.name = unitType.ToString();
            // Get text component from tab
            TMPro.TextMeshProUGUI tabText = tab.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            // Set text to tab
            tabText.text = unitType.ToString();
            // add event listener to tab
            tab.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                // Set current tab
                Debug.Log("Clicked on " + tab.name);
                CurrentTab = tab.name;
                // Set all tabs to not active
 
                BuildingManager.Instance.CreateBuildingTabs((SOBulding.BuildingType)System.Enum.Parse(typeof(SOBulding.BuildingType), tab.name));
            });
            // Add tab to list
            tabs.Add(tab);
        }
    }

    void Start()
    {
        CreateTabs();
    }
}
