using System;
using Unity.Netcode;
using UnityEngine;

public class MatchStateServer : NetworkBehaviour
{
    public static MatchStateServer Instance;
    public NetworkVariable<MatchState> State = new(
        MatchState.Lobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Awake()
    {
        Instance = this;
    }

    public void StartMatch()
    {
        State.Value = MatchState.InProgress;
    }


    public void EndMatch()
    {
        State.Value = MatchState.Ended;
    }
}
