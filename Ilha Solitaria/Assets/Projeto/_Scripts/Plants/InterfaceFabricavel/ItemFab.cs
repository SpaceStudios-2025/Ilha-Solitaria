using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFab : MonoBehaviour{
    [Header("Style")]
    [SerializeField] private Image icone;
    [SerializeField] private TextMeshProUGUI nome_txt;
    [SerializeField] private TextMeshProUGUI qtdMax_txt;
    [SerializeField] private TextMeshProUGUI qtd_txt;

    [HideInInspector] public int qtd;
    private int qtdMax;

    private Materials mat;

    private FabricavelPrefab father;

    public void Style(Fabricar fab,FabricavelPrefab father){
        icone.sprite = Buscador.buscar.crafting.materials[(int)fab.mats];
        nome_txt.text = fab.mats.ToString("");

        qtdMax_txt.text = "Total: " + fab.quantity.ToString("00");
        qtd_txt.text = qtd.ToString("00");

        qtdMax = fab.quantity;
        mat = fab.mats;
        this.father = father;
    }

    void AddQtd(){
        qtd++;
        qtd_txt.text = qtd.ToString("00");

        foreach(var i in father.itensFabricaveis){
            if(i.mat == mat){
                i.Decrement();
                break;
            }
        }

        if(qtd >= qtdMax){
            Buscador.buscar.interfaces_tables[1].GetComponent<MenuFabricarPlanta>().itensFab.Remove(this);

            Buscador.buscar.interfaces_tables[1].GetComponent<MenuFabricarPlanta>().Check();
            Destroy(gameObject);
        }
    }

    public void AddButton(){
        if(InventoryManager.instance.Busca(mat)){
            AddQtd();

            var itemInv = InventoryManager.instance.Busca(mat);
            itemInv.DecrementItem(1);
        }

        //Remove do Inventario
    }
}