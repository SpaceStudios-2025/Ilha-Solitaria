using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventario/Item")]
public class Item : ScriptableObject
{
    public string nome;
    public string describe;

    public Sprite icone;

    [Header("Type")]
    public TypeItem type;

    [Header("Agrupa?")]
    public bool group;
    [Range(0,32)]
    public int maxGroup;

    [Header("Crafting")]
    public Materials mat;

    [Header("Ferramentas")]
    public Tools tool;

    public int life;

    [Header("Planta")]
    public Plantas planta;
}
