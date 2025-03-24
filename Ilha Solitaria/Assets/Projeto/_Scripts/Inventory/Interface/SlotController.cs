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

        if(!full){
            if(hand && !draggableItem.GetComponent<ItemInventory>().item.tools){
                return;
            }

            if(hand && draggableItem.GetComponent<ItemInventory>().item.tools){
                FindFirstObjectByType<Character_Controller>().ToolInHand((int)draggableItem.GetComponent<ItemInventory>().item.tool);
            }
            
            draggableItem.parentAfterDrag.GetComponent<SlotController>().full = false;

            draggableItem.parentAfterDrag = transform;
            draggableItem.GetComponent<ItemInventory>().slotParent = this;
            full = true;
        }else{
            //Preencher
            if(itemInv.item.group){
                if(itemInv.item == draggableItem.GetComponent<ItemInventory>().item){
                    if(itemInv.qtd < itemInv.item.maxGroup){
                        int qtd_ = 0;
                        for (int i = 0; i < draggableItem.GetComponent<ItemInventory>().qtd; i++){
                            if(itemInv.qtd < itemInv.item.maxGroup){
                                qtd_++;
                                itemInv.AddItem(1);
                            }else{
                                break;
                            }
                        }

                        draggableItem.GetComponent<ItemInventory>().DecrementItem(qtd_);
                        draggableItem.GetComponent<ItemInventory>().InQuantity();
                    }
                }
            }
        }
    }
}
