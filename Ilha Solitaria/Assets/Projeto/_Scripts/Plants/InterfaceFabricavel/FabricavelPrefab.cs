using System.Collections.Generic;
using UnityEngine;

public class FabricavelPrefab : MonoBehaviour, ICollectable
{
    [Header("Estilo")]
    private Item item;

    public void Style(Item item){
        this.item = item;
        GetComponent<SpriteRenderer>().sprite = this.item.planta.img;
        gameObject.AddComponent<PolygonCollider2D>();
    }

    [Header("Fabricaveis")]
    [SerializeField] private GameObject prefabFab;
    [SerializeField] private Transform parentFab;

    [HideInInspector] public List<ItemFabricavelPrefab> itensFabricaveis = new List<ItemFabricavelPrefab>();

    void Start()
    {
        GenerateItens();
    }

    public void GenerateItens(){
        foreach(var i in item.planta.materials){
            var prefab = Instantiate(prefabFab,parentFab);
            prefab.GetComponent<ItemFabricavelPrefab>().Style(Buscador.buscar.crafting.materials[(int)i.mats],i.quantity,i.mats,this);
        
            itensFabricaveis.Add(prefab.GetComponent<ItemFabricavelPrefab>());
        }
    }

    public void Interact(){
        Buscador.buscar.interfaces_tables[1].SetActive(true);
        Buscador.buscar.interfaces_tables[1].GetComponent<MenuFabricarPlanta>().item = item;
        Buscador.buscar.interfaces_tables[1].GetComponent<MenuFabricarPlanta>().father = this;
    }

    public void Fabricar(){
        Instantiate(item.planta.prefab,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
