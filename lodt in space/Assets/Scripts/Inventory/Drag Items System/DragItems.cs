using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItems : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _movingIcon;
    [SerializeField] private RectTransform _itemPosition;
    [SerializeField] private ScrollRect _scrollRect;
    private Vector2 _currentPosition;
    [SerializeField] private bool _draggingSlot;
    [SerializeField] private int _doubleClick;

    public void SetDragging(bool value)
    {
        _draggingSlot = value;
    }

    public bool GetDragging()
    {
        return _draggingSlot;
    }

    private void Start()
    {
        _movingIcon = GameObject.FindGameObjectWithTag("MovingCell").GetComponent<RectTransform>();
        _itemPosition = transform.GetChild(1).GetComponent<RectTransform>();
        _currentPosition = _itemPosition.anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("StartTimer");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_scrollRect != null)
            _scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_draggingSlot)
        {
            _movingIcon.anchoredPosition += eventData.delta;
        }
        else
        {
            if (_scrollRect != null)
                _scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_scrollRect != null)
            _scrollRect.OnEndDrag(eventData);

        _movingIcon.gameObject.SetActive(false);

        if (GetComponent<CellContainer>().ItemPrefab != null)
            _itemPosition.gameObject.SetActive(true);

        _itemPosition.anchoredPosition = _currentPosition;
        if (_draggingSlot)
        {
            _draggingSlot = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<CellContainer>().ItemPrefab != null)
        {
            _movingIcon.anchoredPosition = _itemPosition.position;
            _movingIcon.GetComponent<Image>().sprite = _itemPosition.GetComponent<Image>().sprite;
        }

        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("StartTimer");
    }

    private IEnumerator StartTimerDoubleClick()
    {
        yield return new WaitForSeconds(.2f);

    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (GetComponent<CellContainer>().ItemPrefab != null)
        {
            _itemPosition.gameObject.SetActive(false);
            _movingIcon.gameObject.SetActive(true);
        }
        _draggingSlot = true;
    }
}
