using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class WashHands : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public LevelManager levelManager;

    public RectTransform centerRect;
    public int requiredSpins = 3;
    public Slider progressSlider;
    public bool allowReverseDecrease = false;
    public UnityEvent onComplete;

    Canvas _canvas;
    Camera _canvasCamera;
    float _prevAngle;
    float _accumulatedDegrees;
    bool _isDragging;
    bool _isComplete;

    void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _canvasCamera = _canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceCamera ? _canvas.worldCamera : null;
        UpdateProgressUI();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isComplete) return;
        if (centerRect == null) return;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(centerRect, eventData.position, _canvasCamera, out localPoint))
        {
            _prevAngle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
            _isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging || _isComplete || centerRect == null) return;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(centerRect, eventData.position, _canvasCamera, out localPoint)) return;
        float currentAngle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
        float delta = Mathf.DeltaAngle(_prevAngle, currentAngle);
        if (delta < 0f)
        {
            _accumulatedDegrees += -delta;
        }
        else if (allowReverseDecrease)
        {
            _accumulatedDegrees -= delta;
            if (_accumulatedDegrees < 0f) _accumulatedDegrees = 0f;
        }
        _prevAngle = currentAngle;
        float maxDegrees = requiredSpins * 360f;
        if (_accumulatedDegrees >= maxDegrees)
        {
            _accumulatedDegrees = maxDegrees;
            Complete();
        }
        UpdateProgressUI();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }

    void UpdateProgressUI()
    {
        float percent = Mathf.Clamp01(_accumulatedDegrees / (requiredSpins * 360f));
        if (progressSlider != null) progressSlider.value = percent;
    }

    void Complete()
    {
        if (_isComplete) return;
        _isComplete = true;
        levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.CurrentScene++;
        onComplete?.Invoke();
    }

    public void ResetProgress()
    {
        _accumulatedDegrees = 0f;
        _isComplete = false;
        UpdateProgressUI();
    }

    public float GetProgressPercent() => Mathf.Clamp01(_accumulatedDegrees / (requiredSpins * 360f)) * 100f;
}
