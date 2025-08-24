using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionHandler : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug"), ReadOnly] public bool IsServerHosted = false;
    [SerializeField, BoxGroup("Debug"), ReadOnly] public string Code;

    void OnEnable()
    {
        EventHub.Instance.OnHost.AddListener(OnHost);
        EventHub.Instance.OnConnect.AddListener(OnConnect);
    }

    void OnDisable()
    {
        EventHub.Instance.OnHost.RemoveListener(OnHost);
        EventHub.Instance.OnConnect.RemoveListener(OnConnect);
    }

    private void OnHost()
    {
        IsServerHosted = false;
        SceneManager.LoadScene("Game");
    }

    private void OnConnect(string code)
    {
        IsServerHosted = true;
        Code = code;
        SceneManager.LoadScene("Game");
    }


}
