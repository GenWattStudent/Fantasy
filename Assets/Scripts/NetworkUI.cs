using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersInGameText;
    private NetworkVariable<int> playersInGame = new (0, NetworkVariableReadPermission.Everyone);

    private void OnEnable() {
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDisconnected;
        playersInGame.OnValueChanged += OnPlayersInGameChanged;
        hostButton.onClick.AddListener(Host);
        clientButton.onClick.AddListener(Client);
    }

    private void OnDisable() {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerDisconnected;
        playersInGame.OnValueChanged -= OnPlayersInGameChanged;
        hostButton.onClick.RemoveListener(Host);
        clientButton.onClick.RemoveListener(Client);
    }

    private void OnPlayersInGameChanged(int previousValue, int newValue) {
        playersInGameText.text = $"Players: {newValue}";
    }

    private void Host() {
        NetworkManager.Singleton.StartHost();
    }

    private void Client() {
        NetworkManager.Singleton.StartClient();
    }

    private void OnPlayerConnected(ulong clientId) {
        if (!IsServer) return;
        Debug.Log("OnPlayerConnected: " + clientId + " " + NetworkManager.Singleton.ConnectedClients.Count);
        playersInGame.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    private void OnPlayerDisconnected(ulong clientId) {
        if (!IsServer) return;
        Debug.Log("OnPlayerDisconnected: " + clientId + " " + NetworkManager.Singleton.ConnectedClients.Count);
        playersInGame.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
