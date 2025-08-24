using TMPro;
using UnityEngine;

public class RoomCode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        EventHub.Instance.OnJoinCodePublished.AddListener(code => _tmp.text = code);
    }

    void OnDisable()
    {
        EventHub.Instance.OnJoinCodePublished.RemoveListener(code => _tmp.text = code);
    }
}
