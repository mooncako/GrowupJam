using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, BoxGroup("References")] private CanvasGroup _tutorialCanvas;
    [SerializeField, BoxGroup("References")] private Image _image;

    private Tween _alphaTween;
    private Tween _buttonAlphaTween;

    void OnValidate()
    {
        if (_image == null) _image = GetComponent<Image>();
    }

    void OnDisable()
    {
        _alphaTween.Stop();
        _buttonAlphaTween.Stop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _alphaTween.Stop();
        _buttonAlphaTween.Stop();
        _alphaTween = Tween.Alpha(_tutorialCanvas, 1, .5f);
        _buttonAlphaTween = Tween.Alpha(_image, 1, .5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _alphaTween.Stop();
        _buttonAlphaTween.Stop();
        _alphaTween = Tween.Alpha(_tutorialCanvas, 0, .5f);
        _buttonAlphaTween = Tween.Alpha(_image, .2f, .5f);
    }
}
