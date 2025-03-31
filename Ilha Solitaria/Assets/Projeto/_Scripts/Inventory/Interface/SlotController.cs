using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler
{
    [HideInInspector] public bool full;
    [HideInInspector] public ItemInventory itemInv;

    public bool hand;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if(dropped.GetComponent<Dragabble>()){
        Dragabble draggableItem = dropped.GetComponent<Dragabble>();
        if(draggableItem.GetComponent<ItemInventory>()){
            ItemInventory itemInventory = draggableItem.GetComponent<ItemInventory>();

            if(!full){
                if(hand && itemInventory.item.type != TypeItem.tool && itemInventory.item.type != TypeItem.plant) return;

                if(hand && itemInventory.item.type == TypeItem.tool){
                    Buscador.buscar.player.ToolInHand((int)itemInventory.item.tool);
                    Buscador.buscar.player.BackTool(itemInventory.item.icone);
                }else if(hand && itemInventory.item.type == TypeItem.plant){
                    FindFirstObjectByType<PlantsManager>().Style(itemInventory);
                }
                
                draggableItem.parentAfterDrag.GetComponent<SlotController>().TirarDoSlot();

                draggableItem.parentAfterDrag = transform;
                itemInventory.slotParent = this;
                full = true;
            }else{
                //Preencher
                if(itemInv.item.group){
                    if(itemInv.item == itemInventory.item){
                        if(itemInv.qtd < itemInv.item.maxGroup){
                            int qtd_ = 0;
                            for (int i = 0; i < itemInventory.qtd; i++){
                                if(itemInv.qtd < itemInv.item.maxGroup){
                                    qtd_++;
                                    itemInv.AddItem(1);
                                }else{
                                    break;
                                }
                            }

                            itemInventory.DecrementItem(qtd_);
                            itemInventory.InQuantity();
                        }
                    }
                }
            }
        }
        }
    }

    public void TirarDoSlot(){
        full = false;

        if(hand && itemInv != null && itemInv.item.type == TypeItem.tool){
            Buscador.buscar.player.BackTool(null);
        }else if(hand && itemInv != null && itemInv.item.type == TypeItem.plant){
            FindFirstObjectByType<PlantsManager>().Desable();
        }

        itemInv = null;
    }
}
