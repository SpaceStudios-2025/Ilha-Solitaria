using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Fabricavel",menuName = "Crafting/Fabricavel")]
public class ItemFabricavel : ScriptableObject
{
    [Header("Info")]
    public Item item;

    [HideInInspector] public int? qtd;

    [Header("Materiais")]
    public List<Fabricar> materials = new List<Fabricar>();
}
