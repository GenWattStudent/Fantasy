using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitManager : NetworkBehaviour
{
    [SerializeField] private List<Behaviour> componentsToDisable;
    [SerializeField] private List<GameObject> prefabsToColorChange;

    void Start()
    {
        if (!IsOwner) {
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
            prefab.GetComponentInChildren<MeshRenderer>().material = PlayersManager.playerTexture;
        }
    }

    public void AssignMaterialColorForOwnedUnits() {
        foreach(var prefab in prefabsToColorChange) {
            // change color of prefab to green (evey child of the prefab)
            prefab.GetComponentInChildren<MeshRenderer>().material = PlayersManager.playerTexture;
    
        }
    }
}
