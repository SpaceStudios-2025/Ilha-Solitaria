using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private int lifeTotal;

    bool hit_;
    bool fall;

    [Header("Life")]
    [SerializeField] private GameObject canvas_obj;

    [SerializeField] private Slider life_slider;
    [SerializeField] private TextMeshProUGUI max_txt;
    [SerializeField] private TextMeshProUGUI life_txt;

    void Start(){
        lifeTotal = Random.Range(lifemin,lifemax);
        life = lifeTotal;
    }

    void LateUpdate()
    {
        if(!fall){
            hit = Physics2D.OverlapBoxAll(target.position,size,0f,actionPlayer);
            if(hit.Length > 0 && !hit_){
                if(FindFirstObjectByType<Character_Controller>().tool == tool){
                    Hit();
                }
            }
        }
    }

    IEnumerator Delay(){
        hit_ = true;
        yield return new WaitForSeconds(.5f);
        hit_ = false;
    }

    public void Hit(){
        life--;

        FindFirstObjectByType<Character_Controller>().slotHand.itemInv.HitTool();

        int drop = Random.Range(min,max);
        InventoryManager.instance.DropItem(transform,item[Random.Range(0,item.Length)],drop,velocity,0);

        StartCoroutine(Delay());

        if(life <= 0){
            DestroyObj();
        }else{
            StartCoroutine(LifeCoroutine());
        }
    }

    IEnumerator LifeCoroutine(){
        canvas_obj.SetActive(true);
        life_slider.maxValue = lifeTotal;
        life_slider.value = life;

        life_txt.text = life.ToString("00");
        max_txt.text = "/" + lifeTotal.ToString("00");

        yield return new WaitForSeconds(2f);
        canvas_obj.SetActive(false);
    }

    void DestroyObj(){
        GetComponent<Animator>().SetTrigger("fall");
        fall = true;
        canvas_obj.SetActive(false);

        if(destroyItem != null) InventoryManager.instance.DropItem(transform,destroyItem,Random.Range(min,max),velocity,0);
        
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
