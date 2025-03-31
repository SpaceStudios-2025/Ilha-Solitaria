using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuFabricarPlanta : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public FabricavelPrefab father;

    [Header("Generate Items")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    [HideInInspector] public List<ItemFab> itensFab = new List<ItemFab>();

    void Start() => GenerateItens();

    void GenerateItens(){
        for (int i = 0; i < parent.childCount; i++) Destroy(parent.GetChild(i).gameObject);

        foreach(var i in item.planta.materials){
            var it = Instantiate(prefab,parent);
            it.GetComponent<ItemFab>().Style(i,father);

            itensFab.Add(it.GetComponent<ItemFab>());
        }
    }

    public void Check(){
        if(itensFab.Count <= 0){
            father.Fabricar();
            gameObject.SetActive(false);
        }
    }
}
