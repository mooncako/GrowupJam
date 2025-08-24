using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;
    [SerializeField, BoxGroup("References")] public PlayerController Owner;
    [SerializeField, BoxGroup("References")] public Image _image;


    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (_image == null) _image = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (Owner != null)
        {
            _tmp.text = Mathf.RoundToInt(Owner.Energy.Value).ToString();
            _image.fillAmount = Owner.Energy.Value / Owner.Stats.MaxEnergy;
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
