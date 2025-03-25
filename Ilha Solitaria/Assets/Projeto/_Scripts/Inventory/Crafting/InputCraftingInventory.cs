using UnityEngine;
using UnityEngine.EventSystems;

public class InputCraftingInventory : MonoBehaviour, IDropHandler{
    
    public void OnDrop(PointerEventData eventData){
        GameObject dropped = eventData.pointerDrag;
        Dragabble draggableItem = dropped.GetComponent<Dragabble>();

        ItemInventory item = draggableItem.GetComponent<ItemInventory>();
        FindFirstObjectByType<CraftingInventory>().GenerateItem(ref item);
    }
}