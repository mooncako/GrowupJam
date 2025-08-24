using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private GameObject _playerPrefab;
    [field: SerializeField, BoxGroup("References")] private Transform[] _spawnPositions = new Transform[4];
    [SerializeField, BoxGroup("References")] private SunlightOrbFactory _factory;
    [SerializeField, BoxGroup("Setting")] private int _startingOrbNumber = 10;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private List<Transform> _availableSpawnPoints = new List<Transform>();
    [SerializeField, BoxGroup("Debug"), ReadOnly] private List<PlayerController> _currentPlayerList = new List<PlayerController>();
 
    private bool _orbSpawned = false;
    private ulong _serverId = 0;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += StartSession;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
        _availableSpawnPoints = _spawnPositions.ToList();
        if (NetworkManager.Singleton.GetComponent<ConnectionHandler>().IsServerHosted)
        {
            NetworkManager.Singleton.GetComponent<RelayClient>().StartClient(NetworkManager.Singleton.GetComponent<ConnectionHandler>().Code);
        }
        else
        {
            NetworkManager.Singleton.GetComponent<RelayHost>().StartHost();
        }
    }

    private void OnEnable()
    {
        EventHub.Instance.OnPlayerVictory.AddListener(EndSession);
        EventHub.Instance.OnPlayerReady.AddListener(CheckReadyCondition);
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= StartSession;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnect;
        }

        EventHub.Instance.OnPlayerVictory.RemoveListener(EndSession);
        EventHub.Instance.OnPlayerReady.RemoveListener(CheckReadyCondition);
    }

    private void CheckReadyCondition()
    {
        StartCountdown();
    }

    private void OnDisconnect(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId) return;
        SceneManager.LoadScene("MainMenu");
    }


    // Test 
    void Update()
    {
        // if (Input.GetKey(KeyCode.Alpha0))
        // {
        //     NetworkManager.Singleton.StartHost();
        // }
        // if (Input.GetKey(KeyCode.Alpha1))
        // {
        //     NetworkManager.Singleton.StartClient();
        // }
        // if (Input.GetKey(KeyCode.Alpha2))
        // {
        //     EventHub.Instance.OnGameStart.Invoke();
        // }
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
        _currentPlayerList.Add(player.GetComponent<PlayerController>());
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

    private void StartCountdown()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            MatchStateServer.Instance.StartMatch();
        }

        EventHub.Instance.OnGameStart.Invoke();
            
        
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
