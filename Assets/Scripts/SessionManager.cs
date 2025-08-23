using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private GameObject _playerPrefab;
    [field: SerializeField, BoxGroup("References")] private Transform[] _spawnPositions = new Transform[4];
    [SerializeField, BoxGroup("Debug"), ReadOnly] private List<Transform> _availableSpawnPoints = new List<Transform>();
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += StartSession;
        _availableSpawnPoints = _spawnPositions.ToList();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= StartSession;
    }


    // Update is called once per frame
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
}
