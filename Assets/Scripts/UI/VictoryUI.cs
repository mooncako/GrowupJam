using System;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;
    [SerializeField, BoxGroup("References")] private CanvasGroup _canvasGroup;
    [SerializeField, BoxGroup("References")] private Button _button;

    private Tween _alphaTween;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        if (_button == null) _button = GetComponentInChildren<Button>();
    }

    void OnEnable()
    {
        EventHub.Instance.OnPlayerVictory.AddListener(OnVictory);
    }

    void OnDisable()
    {
        EventHub.Instance.OnPlayerVictory.RemoveListener(OnVictory);
        _button.onClick.RemoveAllListeners();
        _alphaTween.Stop();
    }

    private void OnVictory(PlayerController player)
    {
        _button.onClick.AddListener(Disconnect);
        _alphaTween.Stop();
        _alphaTween = Tween.Alpha(_canvasGroup, 1, .5f);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _tmp.text = $"Player{player.OwnerClientId} Wins";
    }

    private void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }
}
