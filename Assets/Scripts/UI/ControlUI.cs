using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, BoxGroup("References")] private CanvasGroup _tutorialCanvas;

    private Tween _alphaTween;

    void OnDisable()
    {
        _alphaTween.Stop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _alphaTween.Stop();
        _alphaTween = Tween.Alpha(_tutorialCanvas, 1, .5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _alphaTween.Stop();
        _alphaTween = Tween.Alpha(_tutorialCanvas, 0, .5f);
    }
}
