using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab;

    private void Start()
    {
        // OnClientConnectedCallback += HandleClientConnected;
    }

}
