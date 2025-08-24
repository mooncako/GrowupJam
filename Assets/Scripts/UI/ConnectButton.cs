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

        _button.onClick.AddListener(OnButtonClicked);
    }

    void OnDisable()
    {
        if (_input != null)
        {
            _input.onValueChanged.RemoveListener(OnValueChanged);
        }

        _button.onClick.RemoveListener(OnButtonClicked);

        _alphaTween.Stop();
        _shakeTween.Stop();
    }

    private void OnButtonClicked()
    {
        if (_enabled)
        {
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_input.text, 7777);
            EventHub.Instance.OnConnect.Invoke(_input.text);
        }
    }

    private void OnValueChanged(string input)
    {
        if (input.Length < 6)
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
