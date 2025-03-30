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

        transform.localScale = new Vector3(1.2f,1.2f);

        GetComponent<ItemInventory>().life.gameObject.SetActive(false);
        GetComponent<ItemInventory>().qtd_txt.gameObject.SetActive(false);
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

        GetComponent<ItemInventory>().qtd_txt.gameObject.SetActive(true);

        transform.localScale = new Vector3(1,1);

        if(GetComponent<ItemInventory>().item.type == TypeItem.tool)
            GetComponent<ItemInventory>().life.gameObject.SetActive(true);
    }
}
