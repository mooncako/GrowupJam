using System;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;
    [SerializeField, BoxGroup("References")] private Button _button;

    private Vector3 _defaultLocalPos;

    private Tween _alphaTween;
    private Tween _shakeTween;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (_button == null) _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    void OnDisable()
    {

        _button.onClick.RemoveListener(OnButtonClicked);

        _alphaTween.Stop();
        _shakeTween.Stop();
    }

    private void OnButtonClicked()
    {
        Application.Quit(0);
    }

    void Awake()
    {
        _defaultLocalPos = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        _shakeTween.Stop();
        _shakeTween = Tween.ShakeLocalPosition(transform, Vector3.one * 10, .5f, cycles: -1);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _shakeTween.Stop();
    }

    
}
