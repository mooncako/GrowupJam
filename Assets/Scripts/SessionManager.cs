using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private GameObject _playerPrefab;
    [field: SerializeField, BoxGroup("References")] private Transform[] _spawnPositions = new Transform[4];
    [SerializeField, BoxGroup("References")] private SunlightOrbFactory _factory;
    [SerializeField, BoxGroup("Setting")] private int _startingOrbNumber = 10;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private List<Transform> _availableSpawnPoints = new List<Transform>();

    private bool _orbSpawned = false;
    private ulong _serverId = 0;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += StartSession;
        _availableSpawnPoints = _spawnPositions.ToList();
    }

    private void OnEnable()
    {
        EventHub.Instance.OnPlayerVictory.AddListener(EndSession);
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= StartSession;
        EventHub.Instance.OnPlayerVictory.RemoveListener(EndSession);
    }


    // Test 
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            NetworkManager.Singleton.StartHost();
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            NetworkManager.Singleton.StartClient();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            EventHub.Instance.OnGameStart.Invoke();
        }
    }

    private void StartSession(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (!_orbSpawned)
        {
            if (_factory != null)
            {
                _serverId = clientId;
                _factory.GenerateOrb(_startingOrbNumber, clientId);
                _orbSpawned = true;
            }
        }

        NetworkObject player = Instantiate(_playerPrefab, GetAvailableSpawnPoint().position, GetAvailableSpawnPoint().rotation).GetComponent<NetworkObject>();

        player.SpawnAsPlayerObject(clientId);
    }

    private Transform GetAvailableSpawnPoint()
    {
        int index = Random.Range(0, _availableSpawnPoints.Count);
        Transform spawnPoint = _availableSpawnPoints[index];
        _availableSpawnPoints.Remove(spawnPoint);
        return spawnPoint;

    }

    private void EndSession(PlayerController player)
    {
        // NetworkManager.Singleton.Shutdown();
    }

    [Button]
    private void StartGenerateOrb()
    {
        StartCoroutine(GenerateOrbCO());
    }

    private IEnumerator GenerateOrbCO()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            _factory.GenerateOrb(1, _serverId);
        }
        
    }
}
