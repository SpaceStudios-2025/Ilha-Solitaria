using UnityEngine;

public class ItemPrefab : MonoBehaviour, ICollectable
{
    public Item item;
    public int qtd = 1;
    public int life_;

    Vector3 pos;
    bool mov;

    bool manter;

    void Awake()
    {
        life_ = item ? item.life : life_;
    }

    void LateUpdate()
    {
        if(mov && item.type != TypeItem.tool){
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

    public void LifeSet(int life){
        life_ = life;
        print("set");
    }


    public void Mov(Vector3 pos,bool manter){
        mov = true;
        this.pos = pos;
        this.manter = manter;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         // Verifica se a ação já foi executada
        if(item.type != TypeItem.tool){
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
}
