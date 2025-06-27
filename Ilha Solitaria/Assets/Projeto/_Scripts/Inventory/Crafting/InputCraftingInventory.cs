using UnityEngine;
using UnityEngine.EventSystems;

public class InputCraftingInventory : MonoBehaviour, IDropHandler{
    
    public void OnDrop(PointerEventData eventData){
        GameObject dropped = eventData.pointerDrag;
        Dragabble draggableItem = dropped.GetComponent<Dragabble>();

        ItemInventory item = draggableItem.GetComponent<ItemInventory>();
        if(item.item.type == TypeItem.craft){
            FindFirstObjectByType<CraftingInventory>().GenerateItem(ref item,item.qtd);
        }
    }
}