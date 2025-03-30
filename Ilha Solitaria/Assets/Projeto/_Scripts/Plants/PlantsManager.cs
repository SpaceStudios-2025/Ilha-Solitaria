using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantsManager : MonoBehaviour
{
    [Header("Interface")]
    [SerializeField] private GameObject interface_;

    [SerializeField] private Image icone_;
    [SerializeField] private TextMeshProUGUI nome_txt;
    [SerializeField] private TextMeshProUGUI describe_txt;

    [SerializeField] private GameObject prefab_Planta;

    private ItemInventory item;

    public void Style(ItemInventory item){
        interface_.SetActive(true);

        icone_.sprite = item.item.planta.img;
        nome_txt.text = item.item.nome;
        describe_txt.text = item.item.describe;

        this.item = item;
    }

    public void Desable(){
        interface_.SetActive(false);
    }

    public void GenerateItem(){
        // var planta = Instantiate(item.planta.prefab,new Vector3(0,0,0),Quaternion.identity);
        prefab_Planta.SetActive(true);
        prefab_Planta.GetComponent<PrefabPlanta>().Style(item);
    }
}
