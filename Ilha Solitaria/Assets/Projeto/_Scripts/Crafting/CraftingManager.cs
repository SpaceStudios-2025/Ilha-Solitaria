using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [Header("Fabricaveis")]
    [SerializeField] private GameObject fabriPrefab;
    [SerializeField] private Transform fabriParent;

    public List<ItemFabricavel> fabricaveis = new List<ItemFabricavel>();

    [Header("Interface")]
    [SerializeField] private Image icone;
    [SerializeField] private TextMeshProUGUI nome_txt;

    [Header("Receita")]
    [SerializeField] private GameObject receitaPrefab;
    [SerializeField] private Transform receitaParent;

    [Header("Input")]
    [SerializeField] private GameObject addItem_interface;

    [Header("Itens In Crafting")]
    [SerializeField] private GameObject itenPrefab;
    [SerializeField] private Transform itenParent;

    public List<ItemCraft> itensCraft = new List<ItemCraft>();
    [HideInInspector] public List<ReceitaItem> receitas = new List<ReceitaItem>();
 
    [Space]
    [Space]
    [Header("Enum Materials")]
    public Sprite[] materials;


    [HideInInspector] public ItemFabricavel fabricavel_select;

    void Awake()
    {
        //Gerar os Itens Fabricaveis
        GenerateFabricaveis();
    }

    public void GenerateFabricaveis(){
        foreach(var fabrica in fabricaveis){
            var fab = Instantiate(fabriPrefab,fabriParent);
            fab.GetComponent<FabricavelSlot>().Style(fabrica.item.icone,fabrica);
        }
    }

    public void SelecionarFabricavel(ItemFabricavel item){
        if(item != fabricavel_select){
            SelectFabri(item);
        }
    }

    void SelectFabri(ItemFabricavel item){
        icone.sprite = item.item.icone;
        nome_txt.text = item.item.nome;

        fabricavel_select = item;
        GenerateReceita(item);
    }

    public void GenerateReceita(ItemFabricavel item){
        //Remove todos os itens antigos
        for (int i = 0; i < receitaParent.childCount; i++) Destroy(receitaParent.GetChild(i).gameObject);
        receitas.Clear();

        foreach(var r in item.materials){
            var receita = Instantiate(receitaPrefab,receitaParent);
            receita.GetComponent<ReceitaItem>().Style(r);
            receitas.Add(receita.GetComponent<ReceitaItem>());
        }
    }

    public void OpenInventory(){
        if(fabricavel_select != null)
            InventoryManager.instance.OpenBtn();
    }

    public void AddItens(ItemInventory item){
        addItem_interface.SetActive(true);
        FindFirstObjectByType<AddItem>().Style(item,item.qtd);
    }

    public bool GenerateItem(ref ItemInventory item,int qtd){
        if(fabricavel_select != null){
            foreach(var f in fabricavel_select.materials){
                if(f.mats == item.item.mat){
                    foreach(var c in itensCraft){
                        if(c.item == item.item){
                            //Pode adicionar tudo
                            c.Add(qtd);

                            //Retirar do inventario manualmente
                            item.DecrementItem(qtd);
                            return true;
                        }
                    }

                    var it = Instantiate(itenPrefab,itenParent);

                    it.GetComponent<ItemCraft>().Style(item.item,qtd);

                    //Remove do inventario manualmente
                    item.DecrementItem(qtd);

                    itensCraft.Add(it.GetComponent<ItemCraft>());
                    return true;
                }
            }
        }

        print("Nenhuma Receita Selecionada");
        return false;
    }

    public void Close(){
        foreach(var c in itensCraft){
            InventoryManager.instance.GenerateItem(c.item,c.qtd);
        }

        Clear();
        fabricavel_select = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Clear(){
        foreach(var c in itensCraft){
            Destroy(c.gameObject);
        }

        foreach(ReceitaItem receita in receitas){
            Destroy(receita.gameObject);
        }

        itensCraft.Clear();
        receitas.Clear();
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
        bool ret = false;

        if(itensCraft.Count >= receitas.Count){
            foreach(var i in itensCraft){
                foreach(var f in receitas){
                    if(i.item.mat == f.mat){
                        if(i.qtd < f.qtd){
                            ret = false;
                            break;
                        }else{
                            ret = true;
                        }
                    }
                }
            }
        }else{
            ret = false;
        }

        return ret;
    }

    void RemoveItens(){
        if(itensCraft.Count > 0){
            foreach(var i in itensCraft){
                foreach(var f in fabricavel_select.materials){
                    if(i.item.mat == f.mats){
                        if(i.qtd >= f.quantity){
                            i.DecrementItem(f.quantity);
                            break;
                        }
                    }
                }
            }
        }
    }

    void RemoveIt(){
        for (int i = 0; i < itensCraft.Count; i++){
            itensCraft[i].Remove();
        }
    }

    #endregion
}

public enum Materials : int{
    wood = 0,
    rock = 1,
    gold = 2,
    linha = 3,
    iron = 4
}

public enum Tools : int{
    axe = 0,
    pick = 1,
    sword = 2,
    nul = 3
}
