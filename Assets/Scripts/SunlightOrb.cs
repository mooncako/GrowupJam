using Sirenix.OdinInspector;
using UnityEngine;

public class SunlightOrb : MonoBehaviour
{
    [SerializeField, BoxGroup("Settings")] private LayerMask _playerMask;

    void OnTriggerEnter(Collider other)
    {
        if ((_playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<PlayerController>().OnReceiveSunlight();
            gameObject.SetActive(false);
        }
    }
}
