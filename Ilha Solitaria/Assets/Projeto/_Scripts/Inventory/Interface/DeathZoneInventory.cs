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

            draggableItem.slotParent.TirarDoSlot();

            InventoryManager.instance.DropItem(Buscador.buscar.player.origin,draggableItem.item,draggableItem.qtd,new Vector2(2f,5f),dropped.GetComponent<ItemInventory>().toolLife);

            //Destruir item do inventario
            draggableItem.InQuantityEx();
        }
    }
}
