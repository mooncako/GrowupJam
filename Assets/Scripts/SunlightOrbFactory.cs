using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class SunlightOrbFactory : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private CapsuleCollider _collider;
    [SerializeField, BoxGroup("References")] private GameObject _sunlightOrbPrefab;
    [SerializeField] private Bounds _bounds;

    void OnValidate()
    {
        if (_collider == null)
        {
            _collider = GetComponent<CapsuleCollider>();
            _collider.isTrigger = true;
        }
    }

    void Awake()
    {
        _bounds = _collider.bounds;
    }



    public void GenerateOrb(int number, ulong serverId)
    {
        if (number <= 0) return;

        for (int i = 0; i < number; i++)
        {
            NetworkObject orb = Instantiate(_sunlightOrbPrefab, GetRandomVectorInBounds(_bounds.min, _bounds.max), Quaternion.identity).GetComponent<NetworkObject>();
            orb.SpawnWithOwnership(serverId);
        }
    }

    private Vector3 GetRandomVectorInBounds(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

}
