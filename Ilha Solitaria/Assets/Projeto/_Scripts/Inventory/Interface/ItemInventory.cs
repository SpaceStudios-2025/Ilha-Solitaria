using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public SlotController slotParent;
    [HideInInspector] public int toolLife;

    public Slider life;

    public Image icon;
    public TextMeshProUGUI qtd_txt;

    public int qtd;

    private int total;
    private Color default_;

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

        if(item.type == TypeItem.tool){
            toolLife = item.life;
            total = item.life;

            life.gameObject.SetActive(true);
            life.maxValue = toolLife;
            life.value = toolLife;

            default_ = life.fillRect.GetComponent<Image>().color;
        }
    }

    public void Style(Item item, int qtd, int life_){
        this.item = item;
        this.qtd = qtd;

        icon.sprite = item.icone;
        
        qtd_txt.text = this.qtd.ToString("00");

        if(item.type == TypeItem.tool){
            toolLife = life_;
            total = item.life;

            life.gameObject.SetActive(true);
            life.maxValue = item.life;
            life.value = toolLife;

            default_ = life.fillRect.GetComponent<Image>().color;
            CheckLife();
        }
    }

    public void InQuantity(){
        if(qtd <= 0){
            InQuantityEx();
        }
    }

    public void InQuantityEx(){
        slotParent.TirarDoSlot();
        InventoryManager.instance.itensInventory.Remove(this);

        Destroy(gameObject);
    }

    public void HitTool(){
        if(item.type == TypeItem.tool){
            toolLife--;
            life.value = toolLife;

            CheckLife();
        }
    }

    void CheckLife(){
        if(toolLife <= total / 2 && toolLife > total / 4) life.fillRect.GetComponent<Image>().color = Color.yellow; // se for menor que a metade fica amarelo
        else if(toolLife <= total / 4) life.fillRect.GetComponent<Image>().color = Color.red; // se for menor que 1/4 fica vermelho
        else life.fillRect.GetComponent<Image>().color = default_; // se não fica verde

        if(toolLife <= 0){
            Buscador.buscar.player.BackTool(null);
            //Quebrou
            InQuantityEx();
        }
    }
}
