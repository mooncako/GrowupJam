using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private Score[] _scores = new Score[4];

    private int _currentIndex = 0;


    void OnEnable()
    {
        EventHub.Instance.OnPlayerJoined.AddListener(OnPlayerJoined);
    }

    void OnDisable()
    {
        EventHub.Instance.OnPlayerJoined.RemoveListener(OnPlayerJoined);
    }

    private void OnPlayerJoined(PlayerController player)
    {

        _scores[_currentIndex].gameObject.SetActive(true);
        _scores[_currentIndex].AssignOwner(player);
        _currentIndex++;
    }
}
