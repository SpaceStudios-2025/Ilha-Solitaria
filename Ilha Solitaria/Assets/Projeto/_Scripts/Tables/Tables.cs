using UnityEngine;

public class Tables : MonoBehaviour, ICollectable
{
    [SerializeField] private int interface_;


    public void Interact(){
        Buscador.buscar.interfaces_tables[interface_].SetActive(true);
    }
}
