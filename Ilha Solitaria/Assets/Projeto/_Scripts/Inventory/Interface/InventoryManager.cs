using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory")]
    [SerializeField] private GameObject inventory_;

    [Header("Create Slots")]
    [SerializeField] private GameObject slot_prefab;
    [SerializeField] private Transform slot_parent;
    [SerializeField] private int qtd_slot = 18;

    public List<SlotController> slots;

    [Header("Create Item in Inventory")]
    [SerializeField] private GameObject item_prefab;

    [Header("Collectable Interface")]
    [SerializeField] private GameObject interface_collect;

    [SerializeField] private Image icon_collect;

    [SerializeField] private TextMeshProUGUI name_collect;
    [SerializeField] private TextMeshProUGUI qtd_collect;
    [SerializeField] private TextMeshProUGUI desc_collect;

    [Header("Dropar Item")]
    [SerializeField] private GameObject itemPrefab;

    [HideInInspector]
    public List<ItemInventory> itensInventory = new List<ItemInventory>();
    //
    //
    //
    //
    //
    [Header("Crafting Inventory")]
    [SerializeField] private GameObject crafting_inventory;
    [SerializeField] private Image icone_btn;
    [SerializeField] private Sprite[] icons_;

    private bool enable_inventory_crafting;
    private bool button_crafting;


    void Awake()
    {
        instance = (instance == null) ? this : instance;
        GenerateSlots();
    }

    void GenerateSlots(){
        for (int i = 0; i < qtd_slot; i++)
        {
            var slot = Instantiate(slot_prefab,slot_parent);
            slots.Add(slot.GetComponent<SlotController>());
        }
    }

    public bool GenerateItem(Item item,int qtd){
        itensPrefabs.Clear();

        if(item.group){
            foreach(var slot in slots){
                if(slot.full && slot.itemInv.item == item){
                    if(slot.itemInv.qtd < item.maxGroup){
                        if((slot.itemInv.qtd + qtd) <= item.maxGroup){
                            AddItem(slot,qtd);
                            return true;
                        }else{
                            var qt = slot.itemInv.qtd + qtd - item.maxGroup;
                            AddItem(slot,qt);

                            qtd = qt;
                        }
                    }
                }
            }

            if(qtd > item.maxGroup){
                CreateItem(item,item.maxGroup);
                qtd -= item.maxGroup;
            
                return CreateItem(item,qtd);
            }
        }

        if(qtd > 1 && !item.group){
            while(qtd >= 1)
            {
                CreateItem(item,1);
                qtd--;
            }
            return true;
        }

        return CreateItem(item,qtd);
        
    }
    public bool GenerateItem(Item item,int qtd,ItemPrefab itemPrefab){
        itensPrefabs.Clear();
        
        if(item.group){
            print("group");
            foreach(var slot in slots){
                if(slot.full && slot.itemInv.item == item){
                    print("slot cheio e item igual");
                    if(slot.itemInv.qtd < item.maxGroup){
                        print("slot cheio e item igual e item com espaço");
                        if((slot.itemInv.qtd + qtd) <= item.maxGroup){
                            AddItem(slot,qtd);
                            print("da para adicionar tudo, adicionado a um item existente : " + qtd);
                            return true;
                        }else{
                            var qt = qtd - slot.itemInv.qtd;
                            var sobra = item.maxGroup - qt;
                            AddItem(slot,qt);
                            itemPrefab.qtd -= qt;

                            print("adicionado a um item existente que não da para adicionar tudo : " + qtd);

                            qtd = sobra;
                            break;
                        }
                    }
                }
            }

            if(qtd > item.maxGroup){
                while(qtd > item.maxGroup){
                    CreateItem(item,item.maxGroup,itemPrefab);
                    qtd -= item.maxGroup;
                }
                
                print("criado varios itens no inventario : " + qtd);
                return CreateItem(item,qtd,itemPrefab);
            }
        }

        if(qtd > 1 && !item.group){
            while(qtd >= 1)
            {
                CreateItem(item,1,itemPrefab);
                qtd--;
            }
            return true;
        }
        
        return CreateItem(item,qtd,itemPrefab);
    }

    void AddItem(SlotController slot,int qtd){
        slot.itemInv.AddItem(qtd);
        print(qtd);
    }

    bool CreateItem(Item item,int qtd,ItemPrefab itemPrefab){
        foreach(var slot in slots){
            if(!slot.full){
                Gerar(item,qtd,slot,itemPrefab.life_);
                itemPrefab.qtd -= qtd;
                print(qtd);
                return true;
            }
        }
        print("Inventario Cheio");
        return false;
        
    }
    bool CreateItem(Item item,int qtd){
        foreach(var slot in slots){
            if(!slot.full){
                print(qtd);
                Gerar(item,qtd,slot);
                return true;
            }
        }

        print("Inventario Cheio");
        return false;
    }

    void Gerar(Item item,int qtd,SlotController slot){
        var it = Instantiate(item_prefab,slot.gameObject.transform);
        it.GetComponent<ItemInventory>().Style(item,qtd);
        it.GetComponent<ItemInventory>().slotParent = slot;

        slot.full = true;
        slot.itemInv = it.GetComponent<ItemInventory>();

        itensInventory.Add(it.GetComponent<ItemInventory>());
    }
    void Gerar(Item item,int qtd,SlotController slot,int life_){
        var it = Instantiate(item_prefab,slot.gameObject.transform);
        it.GetComponent<ItemInventory>().Style(item,qtd,life_);
        it.GetComponent<ItemInventory>().slotParent = slot;

        slot.full = true;
        slot.itemInv = it.GetComponent<ItemInventory>();

        itensInventory.Add(it.GetComponent<ItemInventory>());
    }

    public void DropItem(Transform target, Item item, int qtd,Vector2 velocity,int life_){
        //Criar o item no chão
        Vector3 pos = new(target.position.x,target.position.y + Random.Range(-.15f,.15f));

        var prefab = Instantiate(itemPrefab,pos,Quaternion.identity);
        prefab.GetComponent<ItemPrefab>().qtd = qtd;

        prefab.GetComponent<ItemPrefab>().item = item;
        prefab.GetComponent<SpriteRenderer>().sprite = item.icone;

        prefab.GetComponent<ItemPrefab>().LifeSet(life_);
        prefab.GetComponent<ObjFloat>().enabled = false;

        if(target.GetComponentInParent<Character_Controller>()){
            if(!Buscador.buscar.player.isFacing)
                prefab.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(velocity.x,velocity.y),ForceMode2D.Impulse);
            else
                prefab.GetComponent<Rigidbody2D>().AddForce(Vector3.left * Random.Range(velocity.x,velocity.y),ForceMode2D.Impulse);
        }else{
            if(!Buscador.buscar.player.isFacing)
                prefab.GetComponent<Rigidbody2D>().AddForce(Vector3.left * Random.Range(velocity.x,velocity.y),ForceMode2D.Impulse);
            else
                prefab.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(velocity.x,velocity.y),ForceMode2D.Impulse);
        }

        StartCoroutine(FloatObj(prefab));
    }

    IEnumerator FloatObj(GameObject prefab){
        yield return new WaitForSeconds(.8f);

        if(prefab != null && prefab.GetComponent<ObjFloat>())
            prefab.GetComponent<ObjFloat>().enabled = true;
    }

    #region Collectable Interface

    public void EnableCollectable(){
        interface_collect.SetActive(true);
    }

    public void DesableCollectable(){
        interface_collect.SetActive(false);
    }

    public void Collectable(string nome,string desc,Sprite icon,int qtd){
        name_collect.text = nome;
        desc_collect.text = desc;
        qtd_collect.text = qtd.ToString("00");

        icon_collect.sprite = icon;
    }


    #endregion


    #region Open Inventory 

    public void Close(){
        crafting_inventory.GetComponent<CraftingInventory>().Close();

        enable_inventory_crafting = false;
        crafting_inventory.SetActive(enable_inventory_crafting);
        icone_btn.sprite = enable_inventory_crafting ? icons_[0] : icons_[1];

        inventory_.SetActive(false);
    }

    public void Open(){
        inventory_.SetActive(true);

        foreach(var it in itensInventory){
            it.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void OpenBtn(CraftingManager craft){
        inventory_.SetActive(true);

        foreach(var it in itensInventory){
            it.GetComponent<Button>().onClick.RemoveAllListeners();
            it.GetComponent<Button>().onClick.AddListener(() => InvokeBtn(it,craft));
        }
    }

    void InvokeBtn(ItemInventory it,CraftingManager craft){
        if(it.item.type == TypeItem.craft){
            craft.AddItens(it,it.qtd);
        }
    }


    #endregion


    #region Agrupar Itens

    [HideInInspector] public List<ItemPrefab> itensPrefabs = new List<ItemPrefab>();
    [HideInInspector] public List<ItemPrefab> itensAgrupados = new List<ItemPrefab>();

    public void Agrupar(ItemPrefab it) {
        // Verifica se o item já está na lista
        if (!itensPrefabs.Contains(it)) {
            itensPrefabs.Add(it);
        }

        StartCoroutine(Agrup());
    }

    IEnumerator Agrup() {
        yield return new WaitForSeconds(.2f);

        if (itensPrefabs.Count == 0) yield break; // Se não houver itens, sai do método

        // Pega o primeiro item como referência
        ItemPrefab primeiroItem = itensPrefabs[0];

        if(itensAgrupados.Count > 0){
            foreach (var i in itensPrefabs) {
                foreach(var j in itensAgrupados){
                    if (i != primeiroItem && i != j) {
                        // Move os itens para a posição do primeiro item
                        i.Mov(primeiroItem.transform.position, false);
                        // Adiciona a quantidade do item atual ao primeiro item
                        primeiroItem.qtd += i.qtd;
                        itensAgrupados.Add(i);
                        break;
                    }
                }  
            }
        }else{
            foreach (var i in itensPrefabs) {
                if (i != primeiroItem) {
                    // Move os itens para a posição do primeiro item
                    i.Mov(primeiroItem.transform.position, false);
                    // Adiciona a quantidade do item atual ao primeiro item
                    primeiroItem.qtd += i.qtd;
                    itensAgrupados.Add(i);
                    break;
                }
            }
        }

        itensAgrupados.Clear();
        itensPrefabs.Clear();
    }

    #endregion



    #region Crafting Inventory

    public void BtnCrafting(){
        if(!button_crafting){
            enable_inventory_crafting = !enable_inventory_crafting;
            StartCoroutine(ButtonCrafting());
            
            if(!enable_inventory_crafting)
                FindFirstObjectByType<CraftingInventory>().Close();

            icone_btn.sprite = enable_inventory_crafting ? icons_[0] : icons_[1];
        }
    }

    IEnumerator ButtonCrafting(){
        button_crafting = true;

        if(!enable_inventory_crafting) crafting_inventory.GetComponent<Animator>().SetTrigger("close");
        else crafting_inventory.SetActive(enable_inventory_crafting);

        yield return new WaitForSeconds(.8f);

        if(!enable_inventory_crafting) crafting_inventory.SetActive(enable_inventory_crafting);

        button_crafting = false;
    }


    #endregion


    #region Interface Inventory

    public void Organizar(){
        //Destroi todos os itens do inventario
        for (int i = 0; i < itensInventory.Count; i++) {
            if(itensInventory[i].slotParent && !itensInventory[i].slotParent.hand)
                Destroy(itensInventory[i].gameObject);
        }

        //Deixa todos os slots vazios
        for (int i = 0; i < slots.Count; i++) slots[i].full = false;

        //Cria lista temporaria
        List<ItemInventory> listTemp = new List<ItemInventory>();
        ItemInventory itemInHand = new ItemInventory();

        foreach(var i in itensInventory){
            if(i.slotParent && i.slotParent.hand){
                itemInHand = i;
            }else
                listTemp.Add(i);
        }

        //Limpa a lista do inventario
        itensInventory.Clear();

        //Gera novamente
        foreach(ItemInventory item in listTemp){
            GenerateItem(item.item,item.qtd);
        }

        itensInventory.Add(itemInHand);
        listTemp.Clear();
    }

    #endregion


    #region Busca 

    public ItemInventory Busca(Item item){
        foreach(var i in itensInventory){
            if(i.item == item){
                return i;
            }
        }

        return null;
    }

    public ItemInventory Busca(Materials mat){
        foreach(var i in itensInventory){
            if(i.item.type == TypeItem.craft && i.item.mat == mat){
                return i;
            }
        }

        return null;
    }


    #endregion
}

public enum TypeItem{
    craft,
    tool,
    plant,
    nulo
}
