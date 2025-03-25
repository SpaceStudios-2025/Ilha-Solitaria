using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingInventory : MonoBehaviour, ICrafting
{
    [Header("Receitas")]
    [SerializeField] private Transform receitaParent;
    [SerializeField] private GameObject receitaPrefab;
    
    public List<ItemFabricavel> receitas = new List<ItemFabricavel>();
    [HideInInspector] public ItemFabricavel fabricavel_select;

    [Header("Item Fabricavel")]
    [SerializeField] private Image icone_fabricavel;
    [SerializeField] private TextMeshProUGUI nome_fabricavel_txt;


    [Header("ItensCraft")]
    [SerializeField] private Transform craft_Parent;
    [SerializeField] private GameObject craft_Prefab;

    private ItemCraft itens;

    [Header("Receita Item")]
    [SerializeField] private Transform receitaItemParent;
    [SerializeField] private GameObject receitaItemPrefab;

    private ReceitaItem receitaItem;



    void Start() => GenerateFabricaveis();

    public void GenerateFabricaveis(){
        //Remove todas as receitas abertas da ultima vez
        for (int i = 0; i < receitaParent.childCount; i++) Destroy(receitaParent.GetChild(i).gameObject);

        //Cria as receitas
        foreach(var receita in receitas){
            var rec = Instantiate(receitaPrefab,receitaParent);
            rec.GetComponent<FabricavelSlot>().Style(receita.item.icone,receita,this);
        }
    }

    public void SelecionarFabricavel(ItemFabricavel item){
        if(item != fabricavel_select){
            SelectFabri(item);
        }
    }

    void SelectFabri(ItemFabricavel item){
        icone_fabricavel.sprite = item.item.icone;
        nome_fabricavel_txt.text = item.item.nome;

        fabricavel_select = item;
        GenerateReceita(item);
    }

    public void OpenInventory(){
        if(fabricavel_select != null)
            InventoryManager.instance.OpenBtn();
    }

    public bool GenerateItem(ref ItemInventory item){
        if(fabricavel_select != null){
            foreach(var f in fabricavel_select.materials){
                if(f.mats == item.item.mat){
                    if(itens == null){
                        var it = Instantiate(craft_Prefab,craft_Parent);

                        it.GetComponent<ItemCraft>().Style(item.item,item.qtd,this);

                        //Remove do inventario manualmente
                        item.DecrementItem(item.qtd);

                        //Adiciona o item a lista
                        itens = it.GetComponent<ItemCraft>();
                        return true;
                    }else{
                        if(itens.item == item.item){
                        //Pode adicionar tudo
                        itens.Add(item.qtd);

                        //Retirar do inventario manualmente
                        item.DecrementItem(item.qtd);
                        return true;
                    }
                    }
                }
            }
        }

        print("Nenhuma Receita Selecionada");
        return false;
    }

    public void GenerateReceita(ItemFabricavel item){
        //Remove todos os itens antigos
        if(receitaItemParent.childCount > 0)
            Destroy(receitaItemParent.GetChild(0).gameObject);

        var receita = Instantiate(receitaItemPrefab,receitaItemParent);
        receitaItem = receita.GetComponent<ReceitaItem>();

        receitaItem.Style(item.materials[0]);
    }

    public List<ReceitaItem> Receitas(){
        List<ReceitaItem> rec = new List<ReceitaItem>
        {
            receitaItem
        };
        
        return rec;
    }


    #region Fabricar

    public void Fabricar(){
        if(CheckManufacturation()){
            if(InventoryManager.instance.GenerateItem(fabricavel_select.item,fabricavel_select.qtd??1)){}
            else{
                InventoryManager.instance.DropItem(FindFirstObjectByType<Character_Controller>().origin,fabricavel_select.item,fabricavel_select.qtd??1,new Vector2(5f,10f),fabricavel_select.item.life);
            }

            RemoveItens();
            RemoveIt();
        }
    }
    

    bool CheckManufacturation(){
        if(itens)return itens.qtd >= receitaItem.qtd ? true : false;
        else return false;
    }

    void RemoveItens(){
        if(itens){
            if(itens.qtd >= receitaItem.qtd){
                itens.DecrementItem(receitaItem.qtd);
                return;
            }
        }
    }

    void RemoveIt(){
        itens.Remove();
    }

    #endregion

    #region Close


    public void Close(){
        if(itens != null){
            InventoryManager.instance.GenerateItem(itens.item,itens.qtd);
            Destroy(itens.gameObject);
        }
        
        if(receitaItem != null){
            Destroy(receitaItem.gameObject);
            receitaItem = null;
        }

        fabricavel_select = null;
    }

    #endregion

}
