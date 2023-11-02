using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankSpawner : NetworkBehaviour, IPointerClickHandler, ISpawnerBuilding
{
    [SerializeField] private SoUnit unitToSpawn;
    [SerializeField] private Transform unitSpawnPoint;
    [SerializeField] private SOBulding building;
    [HideInInspector] public int ownerId;
    private List<SoUnit> unitsQueue = new ();
    private float spawnTimer;

    [Command]
    private void SpawnUnitCommand()
    {
        Debug.Log("SpawnUnitCommand " + unitToSpawn.prefab);
        GameObject unitInstance = Instantiate(unitToSpawn.prefab, unitSpawnPoint.position, unitSpawnPoint.rotation);       
        var unit = unitInstance.GetComponent<Unit>();
        unit.ownerId = connectionToClient.connectionId;

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    public void AddUnitToQueue(SoUnit unit)
    {
        Debug.Log("AddUnitToQueue " + unit);
        unitsQueue.Add(unit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Debug.Log(isOwned);
        if (!isOwned) return;
        BuildingManager.Instance.CreateUnitTab(building, this);
    }

    private void StartQueue()
    {
        if (unitsQueue.Count > 0)
        {
            spawnTimer = unitsQueue[0].spawnTime;
        }
    }

    private void SpawnUnit()
    {
        if (spawnTimer > 0) return;

        if (unitsQueue.Count > 0)
        {
            Debug.Log("SpawnUnit " + unitsQueue[0]);
            unitToSpawn = unitsQueue[0];
            SpawnUnitCommand();
            unitsQueue.RemoveAt(0);
            StartQueue();
        }
    }

    private void Start() {
        ownerId = connectionToClient.connectionId;
    }

    private void Update() {
        if (!isOwned) return;

        spawnTimer -= Time.deltaTime;
        StartQueue();
        SpawnUnit();

    }
}
