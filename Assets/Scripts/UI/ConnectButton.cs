using System;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, BoxGroup("References")] private TextMeshProUGUI _tmp;
    [SerializeField, BoxGroup("References")] private CanvasGroup _canvasGroup;
    [SerializeField, BoxGroup("References")] private TMP_InputField _input;
    [SerializeField, BoxGroup("References")] private Button _button;

    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _enabled = false;

    private Vector3 _defaultLocalPos;

    private Tween _alphaTween;
    private Tween _shakeTween;

    void OnValidate()
    {
        if (_tmp == null) _tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        if (_button == null) _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        if (_input != null)
        {
            _input.onValueChanged.AddListener(OnValueChanged);
        }
    }

    void OnDisable()
    {
        if (_input != null)
        {
            _input.onValueChanged.RemoveListener(OnValueChanged);
        }
        _alphaTween.Stop();
        _shakeTween.Stop();
    }

    private void OnValueChanged(string input)
    {
        if (!IPUtil.IsValidIPv4(input))
        {
            _enabled = false;
            _alphaTween.Stop();
            _alphaTween = Tween.Alpha(_canvasGroup, .2f, .5f);
        }
        else
        {
            _enabled = true;
            _alphaTween.Stop();
            _alphaTween = Tween.Alpha(_canvasGroup, 1, .5f);
        }
    }

    void Awake()
    {
        _defaultLocalPos = transform.localPosition;
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enabled)
        {
            _shakeTween.Stop();
            _shakeTween = Tween.ShakeLocalPosition(transform, Vector3.one * 10, .5f, cycles: -1);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_enabled)
        {
            _shakeTween.Stop();
            transform.localPosition = _defaultLocalPos;
        }
    }

    
}
