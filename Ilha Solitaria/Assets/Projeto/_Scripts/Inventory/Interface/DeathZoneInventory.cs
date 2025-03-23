using UnityEngine;
using UnityEngine.EventSystems;

public class DeathZoneInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Chamar o item dropado
        GameObject dropped = eventData.pointerDrag;

        if(dropped.GetComponent<ItemInventory>()){
            ItemInventory draggableItem = dropped.GetComponent<ItemInventory>();

            InventoryManager.instance.DropItem(FindFirstObjectByType<Character_Controller>().origin,draggableItem.item,draggableItem.qtd,new Vector2(5f,10f));

            //Destruir item do inventario
            draggableItem.InQuantityEx();
        }
    }
}
