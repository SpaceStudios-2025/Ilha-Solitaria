using UnityEngine.UI;
using UnityEngine;

public class FabricavelSlot : MonoBehaviour{
    [Header("Info")]
    [SerializeField] private Image icone;
    private ItemFabricavel item;

    [HideInInspector] public ICrafting crafting;

    public void Style(Sprite icon, ItemFabricavel fabri, ICrafting craft){
        icone.sprite = icon;
        item = fabri;
        crafting = craft;
    }

    public void SelectManufacturable(){
        crafting.SelecionarFabricavel(item);
    }
}