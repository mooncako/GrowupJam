using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;
    [SerializeField, BoxGroup("References")] public PlayerController Owner;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Owner != null)
        {
            _tmp.text = Mathf.RoundToInt(Owner.Energy.Value).ToString();
        }
    }


    public void AssignOwner(PlayerController owner)
    {
        Owner = owner;
        if (owner.IsOwner)
        {
            _tmp.color = Color.green;
            _tmp.text = Mathf.RoundToInt(Owner.Energy.Value).ToString();
        }
        
    }
}
