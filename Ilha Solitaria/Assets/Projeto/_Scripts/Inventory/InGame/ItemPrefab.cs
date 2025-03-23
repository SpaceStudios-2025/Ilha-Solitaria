using UnityEngine;

public class ItemPrefab : MonoBehaviour, ICollectable
{
    public Item item;
    public int qtd = 1;

    Vector3 pos;
    bool mov;

    bool manter;

    void LateUpdate()
    {
        if(mov){
            transform.position = Vector3.MoveTowards(transform.position,pos,3f * Time.deltaTime);
            if(Vector2.Distance(transform.position,pos) < 0.1f){
                if(!manter){
                    InventoryManager.instance.itensPrefabs.Remove(this);
                    Destroy(gameObject);
                }else{
                    mov = false;
                }
            }
        }
    }

    public void Interact(){
        if(InventoryManager.instance.GenerateItem(item,qtd,this)){
            Destroy(gameObject);
        }
    }


    public void Mov(Vector3 pos,bool manter){
        mov = true;
        this.pos = pos;
        this.manter = manter;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         // Verifica se a ação já foi executada
        ItemPrefab otherItemPrefab = collision.GetComponent<ItemPrefab>();

        if (otherItemPrefab != null)
        {
            if (otherItemPrefab.item == item)
            {
                InventoryManager.instance.Agrupar(this);
            }
        }
    }
}
