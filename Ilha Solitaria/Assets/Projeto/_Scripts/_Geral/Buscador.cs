using UnityEngine;

public class Buscador : MonoBehaviour
{
    public static Buscador buscar;

    [Header("Buscas")]
    public Character_Controller player;
    public GameObject[] interfaces_tables;
    public CraftingManager crafting;

    void Awake() => buscar = (buscar == null) ? this : buscar;
}
