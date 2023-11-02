using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient connection)
    {
        base.OnServerAddPlayer(connection);
        GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, connection.identity.transform.position, connection.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerInstance, connection);
    }
}
