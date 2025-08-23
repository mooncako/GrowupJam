using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private GameObject _playerPrefab;
    [SerializeField, BoxGroup("References")] private SpawnPos[] _spawnPositions = new SpawnPos[4];
    void Awake()
    {
        NetworkManager.Singleton.NetworkConfig.AutoSpawnPlayerPrefabClientSide = false;
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += StartSession;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= StartSession;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
    }

    private void StartSession(ulong clientId)
    {
        
    }
}
