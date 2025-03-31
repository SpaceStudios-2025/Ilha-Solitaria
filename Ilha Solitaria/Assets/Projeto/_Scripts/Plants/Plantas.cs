using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planta", menuName = "Inventario/Planta")]
public class Plantas : ScriptableObject
{
    public Sprite img;
    public GameObject prefab;

    [Header("Materiais")]
    public List<Fabricar> materials = new List<Fabricar>();
}
