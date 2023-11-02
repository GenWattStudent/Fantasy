using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private List<Unit> selectedUnits = new List<Unit>();
    private static UnitSelection instance;
    public static UnitSelection Instance { get => instance; }
    public List<Unit> SelectedUnits { get => selectedUnits; }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UnitSelection instance!");
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void SelectAllUnits(List<Unit> units) {
        foreach (Unit unit in units) {
            unit.Select();
            unit.GetComponent<UnitMovement>().enabled = true;
            unit.OnDied += RemoveUnitFromSelected;
        }
    }

    public void RemoveUnitFromSelected(Unit unit) {
        selectedUnits.Remove(unit);
    }

    public void DeselectAllUnits() {
        foreach (Unit unit in selectedUnits) {
            unit.Deselect();
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.OnDied -= RemoveUnitFromSelected;
        }
        selectedUnits.Clear();
    }

    public void Select(Unit unit) {
        selectedUnits.Add(unit);
        unit.Select();
        unit.GetComponent<UnitMovement>().enabled = true;
        unit.OnDied += RemoveUnitFromSelected;
    }

    public void DragSelect(Unit unit) {
        if (selectedUnits.Contains(unit)) return;
        Select(unit);
    }
}
