using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private const string PlayerIDPrefix = "Player ";
    private static Dictionary<string, PlayersManager> players = new ();
    [HideInInspector] public static int playersInGame = 0;

    public static void RegisterPlayer(ulong netID, PlayersManager player) {
        string playerID = PlayerIDPrefix + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer(string playerID) {
        players.Remove(playerID);
    }

    public static PlayersManager GetPlayer(string playerID) {
        return players[playerID];
    }

    // public override void OnStartClient() {
    //     base.OnStartClient();
    //     playersInGame++;
    // }

    // public override void OnStopClient() {
    //     base.OnStopClient();
    //     playersInGame--;
    // }
}
