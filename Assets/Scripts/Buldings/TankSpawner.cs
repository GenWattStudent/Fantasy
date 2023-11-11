using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankSpawner : NetworkBehaviour, IPointerClickHandler, ISpawnerBuilding
{
    [SerializeField] private SoUnit unitToSpawn;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private SOBuilding building;
    private List<SoUnit> unitsQueue = new ();
    private float spawnTimer;
    private bool isUnitSpawning = false;
    private SoUnit currentSpawningUnit;

    [ServerRpc]
    private void SpawnUnitServerRpc()
    {
        Debug.Log("SpawnUnitCommand " + unitToSpawn.prefab);
        GameObject unitInstance = Instantiate(unitToSpawn.prefab, unitSpawnPoint.position, unitSpawnPoint.rotation);   
        // unitInstance.GetComponentInChildren<MeshRenderer>().material = PlayersManager.Instance.GetPlayerMaterial(OwnerClientId);
        unitInstance.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    public void AddUnitToQueue(SoUnit unit)
    {
        Debug.Log("AddUnitToQueue " + unit);
        unitsQueue.Add(unit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (!IsOwner) return;
        UIUnitManager.Instance.CreateUnitTabs(building, this);
    }

    private void StartQueue()
    {
        if (unitsQueue.Count > 0 && !isUnitSpawning)
        {
            spawnTimer = unitsQueue[0].spawnTime;
            currentSpawningUnit = unitsQueue[0];
            isUnitSpawning = true;
        }
    }

    private void SpawnUnit()
    {
        if (spawnTimer > 0) return;
    
        if (unitsQueue.Count > 0)
        {
            Debug.Log("SpawnUnit " + unitsQueue[0]);
            unitToSpawn = unitsQueue[0];
            SpawnUnitServerRpc();
            unitsQueue.RemoveAt(0);
            isUnitSpawning = false;
            currentSpawningUnit = null;
            StartQueue();
        }
    }

    private void Update() {
        if (!IsOwner) return;

        spawnTimer -= Time.deltaTime;
        StartQueue();
        SpawnUnit();
    }

    public float GetSpawnTimer()
    {
        if (spawnTimer < 0) return 0;
        return spawnTimer;
    }

    public SoUnit GetCurrentSpawningUnit()
    {
        return currentSpawningUnit;
    }

    public int GetUnitQueueCountByName(string unitName)
    {
        return unitsQueue.FindAll(unit => unit.name == unitName).Count;
    }
}
