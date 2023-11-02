using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    [SerializeField] private List<Behaviour> componentsToDisable;
    [SerializeField] private List<GameObject> prefabsToColorChange;

    void Start()
    {
        if (!isOwned) {
            DisableComponents();
            AssignRemoteLayer();
            AssignMaterialColor();
        } else {
            AssignLocalLayer();
            AssignMaterialColorForOwnedUnits();
        }
    }

    private void DisableComponents() {
        foreach (var component in componentsToDisable) {
            component.enabled = false;
        }
    }

    private void AssignLocalLayer() {
        gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
    }

    private void AssignMaterialColor() {
        foreach(var prefab in prefabsToColorChange) {
            // change color of prefab to red (evey child of the prefab)
            var material = prefab.GetComponentInChildren<MeshRenderer>().material;
            // change material texture 
            Debug.Log("AssignMaterialColorForOwnedUnits " + PlayersManager.playerTexture);
            material.SetTexture("_playerTexture", PlayersManager.playerTexture);
        }
    }

    private void AssignMaterialColorForOwnedUnits() {
        foreach(var prefab in prefabsToColorChange) {
            // change color of prefab to green (evey child of the prefab)
            var material = prefab.GetComponentInChildren<MeshRenderer>().material;
            Debug.Log("AssignMaterialColorForOwnedUnits " + PlayersManager.playerTexture);
            material.SetTexture("_playerTexture", PlayersManager.playerTexture);
        }
    }
}
