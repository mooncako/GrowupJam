using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class SunlightOrb : NetworkBehaviour
{
    [SerializeField, BoxGroup("Settings")] private LayerMask _playerMask;
    [SerializeField, BoxGroup("References")] private GameObject _orb;
    [SerializeField, BoxGroup("References")] private VisualEffect _burst;

    void OnTriggerEnter(Collider other)
    {
        if ((_playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<PlayerController>().OnReceiveSunlight();
            _orb.SetActive(false);
            _burst.Play();

            Destroy(gameObject, .5f);
        }
    }
}
