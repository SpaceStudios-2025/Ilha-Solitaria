using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFabricavelPrefab : MonoBehaviour
{
    [Header("Style")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI qtd_txt;

    private int qtd;
    private FabricavelPrefab father;

    [HideInInspector] public Materials mat;

    public void Style(Sprite img, int qtd,Materials mat,FabricavelPrefab father){
        icon.sprite = img;
        qtd_txt.text = qtd.ToString("00");

        this.qtd = qtd;
        this.father = father;
        this.mat = mat;
    }

    public void Decrement(){
        qtd--;
        qtd_txt.text = qtd.ToString("00");

        if(qtd <= 0){
            father.itensFabricaveis.Remove(this);
            Destroy(gameObject);
        }
    }
}
