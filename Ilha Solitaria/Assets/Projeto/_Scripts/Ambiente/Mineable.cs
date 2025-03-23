using System.Collections;
using UnityEngine;

public class Mineable : MonoBehaviour
{   
    [Header("Collisão")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 size;
    [SerializeField] private LayerMask actionPlayer;

    [Header("Drop")]
    [SerializeField] private Item[] item;

    [SerializeField] private Item destroyItem;
    [SerializeField] private int max;
    [SerializeField] private int min;

    [Space]
    [SerializeField] public Tools tool;
    [SerializeField] private Vector2 velocity;

    public Collider2D[] hit;

    public int lifemin;
    public int lifemax;

    private int life = 3;

    bool hit_;
    bool fall;

    void Start() => life = Random.Range(lifemin,lifemax);

    void LateUpdate()
    {
        if(!fall){
            hit = Physics2D.OverlapBoxAll(target.position,size,0f,actionPlayer);
            if(hit.Length > 0 && !hit_){
                if(FindFirstObjectByType<Character_Controller>().tool == tool){
                    life--;

                    int drop = Random.Range(min,max);
                    InventoryManager.instance.DropItem(transform,item[Random.Range(0,item.Length)],drop,velocity);

                    StartCoroutine(Delay());

                    if(life <= 0){
                        DestroyObj();
                    }
                }
            }
        }
    }

    IEnumerator Delay(){
        hit_ = true;
        yield return new WaitForSeconds(.5f);
        hit_ = false;
    }

    void DestroyObj(){
        GetComponent<Animator>().SetTrigger("fall");
        fall = true;

        if(destroyItem != null) InventoryManager.instance.DropItem(transform,destroyItem,Random.Range(min,max),velocity);
        
        Destroy(gameObject,4f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(target.position,size);
    }

    public void Destruct(){
        Destroy(gameObject);
    }
}
