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
        Dragabble draggableItem = dropped.GetComponent<Dragabble>();
        if(draggableItem.GetComponent<ItemInventory>()){
            ItemInventory itemInventory = draggableItem.GetComponent<ItemInventory>();

            if(!full){
                if(hand && !itemInventory.item.tools){
                    return;
                }

                if(hand && itemInventory.item.tools){
                    FindFirstObjectByType<Character_Controller>().ToolInHand((int)itemInventory.item.tool);
                    FindFirstObjectByType<Character_Controller>().BackTool(itemInventory.item.icone);
                }
                
                draggableItem.parentAfterDrag.GetComponent<SlotController>().full = false;

                if(draggableItem.parentAfterDrag.GetComponent<SlotController>().hand){
                    FindFirstObjectByType<Character_Controller>().BackTool(null);
                }

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
