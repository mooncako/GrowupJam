using Sirenix.OdinInspector;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField, BoxGroup("Settings")] private LayerMask _playerMask;

    void OnTriggerEnter(Collider other)
    {
        if ((_playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            EventHub.Instance.OnPlayerVictory.Invoke(other.GetComponent<PlayerController>());
        }
    }
}
