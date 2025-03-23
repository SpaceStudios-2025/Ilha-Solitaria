using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragabble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        image.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        parentAfterDrag.GetComponent<SlotController>().itemInv = GetComponent<ItemInventory>();

        image.enabled = true;
        image.raycastTarget = true;
    }
}
