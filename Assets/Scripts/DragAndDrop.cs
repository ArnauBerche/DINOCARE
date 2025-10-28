using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform[] snapTargets;
    public float snapDistance = 40f;
    public bool inPlace = false;
    public bool requireAllTargetsVisited = false;

    public bool useLag = true;
    [Range(0f, 1f)]
    public float lagFactor = 0.15f;

    public bool lockX = false;
    public bool lockY = false;
    public bool useFixedX = false;
    public float fixedX = 0f;
    public bool useFixedY = false;
    public float fixedY = 0f;
    public bool useBounds = false;
    public Vector2 minAnchoredPosition = new Vector2(-1000f, -1000f);
    public Vector2 maxAnchoredPosition = new Vector2(1000f, 1000f);

    Canvas _canvas;
    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;
    Transform _originalParent;  
    Vector2 _originalAnchoredPosition;

    Vector2 _dragOffset;
    Vector2 _targetAnchoredPosition;
    bool _isDragging;

    bool[] _visitedTargets;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        EnsureVisitedArray();
    }

    void OnValidate()
    {
        EnsureVisitedArray();
    }

    void EnsureVisitedArray()
    {
        int len = snapTargets != null ? snapTargets.Length : 0;
        if (_visitedTargets == null || _visitedTargets.Length != len)
        {
            _visitedTargets = new bool[len];
        }
    }

    public void ResetVisited()
    {
        if (_visitedTargets == null) EnsureVisitedArray();
        for (int i = 0; i < _visitedTargets.Length; i++) _visitedTargets[i] = false;
        inPlace = false;
    }

    void Update()
    {
        if (!_isDragging) return;
        if (_rectTransform == null) return;
        if (useLag)
        {
            float t = Mathf.Clamp01(lagFactor * Time.deltaTime * 60f);
            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _targetAnchoredPosition, t);
        }
        else
        {
            _rectTransform.anchoredPosition = _targetAnchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inPlace) return;
        _originalParent = transform.parent;
        _originalAnchoredPosition = _rectTransform.anchoredPosition;
        if (_canvas != null) transform.SetParent(_canvas.transform, true);
        _canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        Camera cam = (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;
        Vector2 localPoint;
        RectTransform canvasRect = _canvas != null ? _canvas.transform as RectTransform : null;
        if (canvasRect != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, cam, out localPoint))
        {
            _dragOffset = _rectTransform.anchoredPosition - localPoint;
            _targetAnchoredPosition = _rectTransform.anchoredPosition;
        }
        else
        {
            _dragOffset = Vector2.zero;
            _targetAnchoredPosition = _rectTransform.anchoredPosition;
        }

        _isDragging = true;
    }

    void ApplyConstraints(ref Vector2 candidate)
    {
        if (lockX) candidate.x = _originalAnchoredPosition.x;
        if (lockY) candidate.y = _originalAnchoredPosition.y;
        if (useFixedX) candidate.x = fixedX;
        if (useFixedY) candidate.y = fixedY;
        if (useBounds)
        {
            candidate.x = Mathf.Clamp(candidate.x, minAnchoredPosition.x, maxAnchoredPosition.x);
            candidate.y = Mathf.Clamp(candidate.y, minAnchoredPosition.y, maxAnchoredPosition.y);
        }
    }

    int FindNearestTargetIndex(Vector2 thisScreenPos)
    {
        if (snapTargets == null || snapTargets.Length == 0) return -1;
        Camera cam = (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;
        int bestIndex = -1;
        float bestDist = float.MaxValue;
        for (int i = 0; i < snapTargets.Length; i++)
        {
            var target = snapTargets[i];
            if (target == null) continue;
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(cam, target.position);
            float d = Vector2.Distance(thisScreenPos, targetScreenPos);
            if (d <= snapDistance && d < bestDist)
            {
                bestDist = d;
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inPlace) return;
        if (_rectTransform == null || _canvas == null) return;
        Vector2 localPoint;
        Camera cam = (_canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, cam, out localPoint))
        {
            Vector2 candidate = localPoint + _dragOffset;
            ApplyConstraints(ref candidate);
            _targetAnchoredPosition = candidate;
            if (!useLag)
            {
                _rectTransform.anchoredPosition = _targetAnchoredPosition;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _isDragging = false;
        if (inPlace) return;

        Camera cam = (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;
        Vector2 thisScreenPos = RectTransformUtility.WorldToScreenPoint(cam, _rectTransform.position);
        int targetIndex = FindNearestTargetIndex(thisScreenPos);

        if (targetIndex >= 0 && snapTargets != null && snapTargets.Length > targetIndex && snapTargets[targetIndex] != null)
        {
            var target = snapTargets[targetIndex];
            transform.SetParent(target.parent, false);
            _rectTransform.anchoredPosition = target.anchoredPosition;
            if (_visitedTargets == null) EnsureVisitedArray();
            if (targetIndex >= 0 && targetIndex < _visitedTargets.Length) _visitedTargets[targetIndex] = true;
            if (!requireAllTargetsVisited)
            {
                inPlace = true;
                return;
            }
            else
            {
                bool allVisited = true;
                for (int i = 0; i < _visitedTargets.Length; i++)
                {
                    if (!_visitedTargets[i])
                    {
                        allVisited = false;
                        break;
                    }
                }
                if (allVisited)
                {
                    inPlace = true;
                    return;
                }
                else
                {
                    inPlace = false;
                    return;
                }
            }
        }

        if (_originalParent != null) transform.SetParent(_originalParent, false);
        _rectTransform.anchoredPosition = _originalAnchoredPosition;
        inPlace = false;
    }
}
