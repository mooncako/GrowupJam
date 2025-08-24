using System;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EventHub : MonoBehaviour
{
    public static EventHub Instance;

    [HideInInspector] public UnityEvent<PlayerController> OnPlayerJoined;
    [HideInInspector] public UnityEvent OnGameStart;
    [HideInInspector] public UnityEvent<PlayerController> OnPlayerVictory;
    [HideInInspector] public UnityEvent OnHost;
    [HideInInspector] public UnityEvent<string> OnJoinCodePublished;
    [HideInInspector] public UnityEvent<string> OnConnect;
    [HideInInspector] public UnityEvent OnPlayerReady;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Button]
    private void TestVictory(PlayerController player)
    {
        OnPlayerVictory.Invoke(player);
    }
}
