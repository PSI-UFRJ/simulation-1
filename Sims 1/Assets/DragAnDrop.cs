using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAnDrop : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta  / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("apertou");
    }


}
