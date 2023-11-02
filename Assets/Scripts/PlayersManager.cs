using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayersManager : NetworkBehaviour
{
    [SerializeField] private List<Texture2D> playerTextures;
    private const string remoteLayerName = "RemotePlayer";
    private const string localLayer = "LocalPlayer";
    public static Texture2D playerTexture;

    public override void OnStartClient() {
        base.OnStartClient();
        int netId = GetComponent<NetworkIdentity>().connectionToClient.connectionId;
        Debug.Log("OnStartCliendft " + netId.ToString());
        GameManager.RegisterPlayer(netId, this);
    }

    void Start()
    {
        if (!isOwned) {
            AssignRemoteLayer();
        } else {
            AssignLocalLayer();
            Debug.Log("Start " + isOwned + " " + GameManager.playersInGame);
            var texture2D = playerTextures[GameManager.playersInGame - 1];

            if (texture2D != null) {
                playerTexture = texture2D;
            }
        }

        Debug.Log("Start " + isOwned + " " + GameManager.playersInGame);
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void AssignLocalLayer() {
        gameObject.layer = LayerMask.NameToLayer(localLayer);
    }

    private void OnDisable() {
        GameManager.UnRegisterPlayer(transform.name);
    }
}
