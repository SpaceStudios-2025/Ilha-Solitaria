using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddItem : MonoBehaviour
{
    [Header("Style")]
    [SerializeField] private Image icone;
    [SerializeField] private TextMeshProUGUI qtd_txt;
    [SerializeField] private TextMeshProUGUI max_txt;
    [SerializeField] private TextMeshProUGUI nome_txt;

    [SerializeField] private Slider slider;

    private int qtd;
    private ItemInventory item;

    [HideInInspector] public ICrafting crafting;

    public void Style(ItemInventory item,int max,ICrafting craft){
        icone.sprite = item.item.icone;
        nome_txt.text = item.item.nome;
        max_txt.text = max.ToString();

        this.item = item;

        qtd_txt.text = "01";

        slider.value = 1;
        slider.maxValue = max;

        qtd = 1;

        crafting = craft;
    }

    public void OnSliderValueChanged(float value)
    {
        // Atualiza a variável com o novo valor do Slider
        qtd = (int)value;
        qtd_txt.text = qtd.ToString("00");
    }

    public void Enviar(){
        if(crafting.GenerateItem(ref item,qtd)){
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
