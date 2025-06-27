using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraft : MonoBehaviour{

    [Header("Style")]
    [SerializeField] private Image icone;
    [SerializeField] private TextMeshProUGUI qtd_txt;

    [HideInInspector] public Item item;
    public int qtd;

    private ICrafting crafting;
    private CraftingManager craft;
 
    public void Style(Item item,int qtd,ICrafting craft,CraftingManager crafting){
        this.item = item;
        icone.sprite = item.icone;
        qtd_txt.text = this.qtd.ToString("00");

        this.crafting = craft;
        this.craft = crafting;
        
        Add(qtd);
    }

    void LateUpdate()
    {
        Remove();
    }

    public void Add(int qtd){
        this.qtd += qtd;
        qtd_txt.text = this.qtd.ToString("00");

        foreach(var i in crafting.Receitas()){
            if(i.mat == item.mat){
                if(this.qtd >= i.qtd){
                    i.Complete();
                    break;
                }
            }
        }
    }

    public void DecrementItem(int qtd){
        this.qtd -= qtd;
        qtd_txt.text = this.qtd.ToString("00");


        foreach(var i in crafting.Receitas()){
            if(i.mat == item.mat){
                if(this.qtd < i.qtd){
                    i.NotComplete();
                    break;
                }
            }
        }
    }

    public void Remove(){
        if(qtd <= 0){
            craft.itensCraft.Remove(this);
            Destroy(gameObject);
        }
    }

    public void ReturnInventory(){
        if(InventoryManager.instance.GenerateItem(item,qtd)){
            DecrementItem(qtd);
            Remove();
        }
    }

}