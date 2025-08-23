using System;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private CinemachineCamera _cam;
    [SerializeField, BoxGroup("References")] private CinemachineInputAxisController _axisController;

    void OnValidate()
    {
        if (_cam == null) _cam = GetComponent<CinemachineCamera>();
        if (_axisController == null) _axisController = GetComponent<CinemachineInputAxisController>();
    }

    void OnEnable()
    {
        EventHub.Instance.OnPlayerJoined.AddListener(OnPlayerJoined);
    }

    void OnDisable()
    {
        EventHub.Instance.OnPlayerJoined.RemoveListener(OnPlayerJoined);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _axisController.enabled = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _axisController.enabled = false;
        }
        

    }

    private void OnPlayerJoined(PlayerController player)
    {
        if (player.IsOwner)
        {
            _cam.Target.TrackingTarget = player.transform;
            _axisController.PlayerIndex = player.GetInputId();
            _axisController.enabled = false;
        }
    }
}
