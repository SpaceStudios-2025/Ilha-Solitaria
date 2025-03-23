using UnityEngine.UI;
using UnityEngine;

public class FabricavelSlot : MonoBehaviour{
    [Header("Info")]
    [SerializeField] private Image icone;
    private ItemFabricavel item;

    public void Style(Sprite icon, ItemFabricavel fabri){
        icone.sprite = icon;
        item = fabri;
    }

    public void SelectManufacturable(){
        FindFirstObjectByType<CraftingManager>().SelecionarFabricavel(item);
    }
}