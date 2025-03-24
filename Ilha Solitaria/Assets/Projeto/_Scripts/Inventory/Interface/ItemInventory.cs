using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public SlotController slotParent;

    public Image icon;
    public TextMeshProUGUI qtd_txt;

    public int qtd;

    public void AddItem(int? qtd_){
        qtd += qtd_??1;
        qtd_txt.text = qtd.ToString("00");
    }

    public void DecrementItem(int? qtd_){
        qtd -= qtd_??1;
        qtd_txt.text = qtd.ToString("00");

        InQuantity();
    }

    public void Style(Item item,int qtd){
        this.item = item;
        this.qtd = qtd;

        icon.sprite = item.icone;
        
        qtd_txt.text = this.qtd.ToString("00");
    }

    public void InQuantity(){
        if(qtd <= 0){
            InQuantityEx();
        }
    }

    public void InQuantityEx(){
        slotParent.full = false;
        slotParent.itemInv = null;

        InventoryManager.instance.itensInventory.Remove(this);

        Destroy(gameObject);
    }
}
