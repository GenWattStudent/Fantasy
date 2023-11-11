using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : NetworkBehaviour
{
    public NetworkList<PlayerData> players;
    [SerializeField] private List<Material> playerTextures;
    [SerializeField] private GameObject unitSpawnerPrefab;

    public static PlayersManager Instance { get; private set; }
    
    private const string remoteLayerName = "RemotePlayer";
    private const string localLayer = "LocalPlayer";
    public static Material playerTexture;

    public override void OnNetworkSpawn() {
        if (!IsServer) return;
        NetworkManager.OnClientConnectedCallback += HandleClientConnect;
        NetworkManager.OnClientDisconnectCallback += HandleClientDisconnect;

        // foreach (var client in NetworkManager.Singleton.ConnectedClientsList) {
        //     HandleClientConnect(client.ClientId);
        // }
    }

    public override void OnNetworkDespawn() {
        if (!IsServer) return;
        NetworkManager.OnClientConnectedCallback -= HandleClientConnect;
        NetworkManager.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    private void HandleClientConnect(ulong clientId) {
        players.Add(new PlayerData(clientId, (byte) (NetworkManager.Singleton.ConnectedClientsList.Count - 1)));
        SpawnPlayerServerRpc(clientId);
        Debug.Log($"Players: {players.Count}");
    }

    private void HandleClientDisconnect(ulong clientId) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].ClientId == clientId) {
                players.RemoveAt(i);
                return;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong clientId) {
        if (!IsServer) return;
        Debug.Log("SpawnPlayer " + clientId);
        Vector3 randomSpawnPointVector =  new Vector3(Random.Range(2, 9), 0, Random.Range(2, 9));

        var unitSpawnerInstance = Instantiate(unitSpawnerPrefab, randomSpawnPointVector, Quaternion.identity);
        var playerTextureIndex = -1;

        for (int i = 0; i < players.Count; i++) {
            if (players[i].ClientId == clientId) {
                playerTextureIndex = players[i].TextureId;
                break;
            }
        }

        unitSpawnerInstance.GetComponentInChildren<MeshRenderer>().material = playerTextures[playerTextureIndex];
        unitSpawnerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private void Awake() {
        players = new NetworkList<PlayerData>();
    }

    public Material GetPlayerMaterial(ulong clientId) {
        var playerTextureIndex = -1;

        for (int i = 0; i < players.Count; i++) {
            if (players[i].ClientId == clientId) {
                playerTextureIndex = players[i].TextureId;
                break;
            }
        }

        return playerTextures[playerTextureIndex];
    }

    void Start()
    {
        Instance = this;
        // if (!IsOwner) {
        //     AssignRemoteLayer();
        // } else {
        //     AssignLocalLayer();
        //     var texture2D = playerTextures[NetworkManager.Singleton.ConnectedClients.Count - 1];

        //     if (texture2D != null) {
        //         playerTexture = texture2D;
        //     }
        // }

        Debug.Log("Start " + IsOwner + " " + GameManager.playersInGame);
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void AssignLocalLayer() {
        gameObject.layer = LayerMask.NameToLayer(localLayer);
    }
}
