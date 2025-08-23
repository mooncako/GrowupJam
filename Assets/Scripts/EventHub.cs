using UnityEngine;
using UnityEngine.Events;

public class EventHub : MonoBehaviour
{
    public static EventHub Instance;

    [HideInInspector] public UnityEvent OnGameStart;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
