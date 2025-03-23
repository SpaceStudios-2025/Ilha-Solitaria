using UnityEngine;

public class Tables : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject interface_;

    public void Interact(){
        interface_.SetActive(true);
    }
}
