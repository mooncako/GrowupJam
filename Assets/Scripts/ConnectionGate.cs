using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionGate : MonoBehaviour
{
    void Awake()
    {
        GetComponent<NetworkManager>().NetworkConfig.ConnectionApproval = true;
        GetComponent<NetworkManager>().ConnectionApprovalCallback += OnApprove;
    }

    private readonly HashSet<ulong> _whiteList = new();

    private void OnApprove(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        res.Approved = false;
        res.CreatePlayerObject = false;

        var stateOk = MatchStateServer.Instance == null || MatchStateServer.Instance.State.Value != MatchState.InProgress;

        if (stateOk || _whiteList.Contains(req.ClientNetworkId))
        {
            res.Approved = true;
            res.CreatePlayerObject = true;
            return;
        }

        res.Reason = "MatchAlreadyStarted";
    }
}
