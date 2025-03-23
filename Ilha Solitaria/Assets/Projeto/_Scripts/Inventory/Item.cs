using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventario/Item")]
public class Item : ScriptableObject
{
    public string nome;
    public string describe;

    public Sprite icone;

    [Header("Agrupa?")]
    public bool group;
    [Range(0,32)]
    public int maxGroup;

    [Header("Crafting")]
    public bool craft;
    public Materials mat;

    [Header("Ferramentas")]
    public bool tools;
    public Tools tool;
}
