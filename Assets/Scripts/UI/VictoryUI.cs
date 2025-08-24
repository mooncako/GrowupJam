using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        EventHub.Instance.OnPlayerVictory.AddListener(OnVictory);
    }

    void OnDisable()
    {
        EventHub.Instance.OnPlayerVictory.RemoveListener(OnVictory);
    }

    private void OnVictory(PlayerController player)
    {
        _tmp.text = $"Player{player.GetInputId()} Wins";
    }
}
