using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReceitaItem : MonoBehaviour
{
    [Header("Style")]
    [SerializeField] private Image icone;
    [SerializeField] private TextMeshProUGUI qtd_txt;

    [Header("Complete")]
    [SerializeField] private GameObject complete;

    [HideInInspector] public Materials mat;
    [HideInInspector] public int qtd;

    public void Style(Fabricar fabri){
        icone.sprite = FindFirstObjectByType<CraftingManager>().materials[(int)fabri.mats];
        qtd = fabri.quantity;
        qtd_txt.text = fabri.quantity.ToString("00");
        mat = fabri.mats;
    }

    public void Complete(){
        complete.SetActive(true);
    }

    public void NotComplete(){
        complete.SetActive(false);
    }
}

