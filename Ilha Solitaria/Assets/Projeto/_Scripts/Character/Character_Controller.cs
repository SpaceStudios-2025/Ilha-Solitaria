using System.Collections;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float speedWalk;
    [SerializeField] private float speedRun;

    [Space]
    [SerializeField] private GameObject collider_;

    private float speed;
    [HideInInspector] public bool isFacing;

    private Animator anim;
    
    [Header("Colletable")]
    [SerializeField] private float range_Collect;
    [SerializeField] private LayerMask layer_Interact;

    public Transform origin;
    public Transform pes;

    [Header("Hand")]
    public SlotController slotHand;
    public Tools tool;

    bool mov = true;

    [Header("Particles")]
    [SerializeField] private GameObject particle_action;

    void Start()
    {
        speed = speedWalk;
        anim = GetComponent<Animator>();
        mov = true;
    }

    void LateUpdate()
    {
        CharacterMove();
        ColliderCollect();

        Action();
    }

    #region Actions

    public void Action(){
        if(Input.GetKeyDown(KeyCode.Space) && slotHand.full){
            anim.SetTrigger("Tool");
            foreach(var i in ToolsManager.instance.tools){
                if(i.tool == slotHand.itemInv.item.tool){
                    anim.SetInteger("tools",i.animation);
                    StartCoroutine(Moviment());
                    break;
                }
            }
        }
    }

    public void Particle_Action(){
        Instantiate(particle_action,collider_.transform.position,Quaternion.identity);
    }

    IEnumerator Moviment(){
        mov = false;
        yield return new WaitForSeconds(.8f);
        mov = true;
    }

    public void ToolInHand(int tol){
        tool = (Tools)tol;
    }

    #endregion

    #region Movimentar
    void CharacterMove(){
        if(mov){
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 mov = new(x,y);
            mov = mov.normalized;

            if(mov != Vector3.zero){
                if(Input.GetKey(KeyCode.LeftShift)){
                    speed = speedRun;
                    anim.SetInteger("transition",2);
                }else{
                    speed = speedWalk;
                    anim.SetInteger("transition",1);
                }
                anim.SetFloat("x",x);
                anim.SetFloat("y",y);
            }else{
                anim.SetInteger("transition",0);
            }

            if(mov.x < 0 && !isFacing){
                Flip();
            }else if(mov.x > 0 && isFacing){
                Flip();
            }
            

            transform.position += mov * speed * Time.deltaTime;
        }
    }

    void Flip(){
        isFacing = !isFacing;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    #endregion

    #region Coletaveis

    void ColliderCollect(){
        Collider2D[] hit = Physics2D.OverlapCircleAll(origin.transform.position,range_Collect,layer_Interact);
        
        if(hit.Length > 0){
            if(hit[0].GetComponent<ItemPrefab>()){
                InventoryManager.instance.EnableCollectable();
                var itemPref = hit[0].GetComponent<ItemPrefab>();

                string nome = itemPref.item.nome;
                string desc = itemPref.item.describe;
                int qtd = itemPref.qtd;
                Sprite icon = itemPref.item.icone;

                InventoryManager.instance.Collectable(nome,desc,icon,qtd);
            }

            if(Input.GetKeyDown(KeyCode.E)) hit[0].GetComponent<ICollectable>().Interact();

            if(hit[0].transform.position.y > pes.position.y){
                hit[0].GetComponent<Renderer>().sortingLayerName = "back";
            }else{
                hit[0].GetComponent<Renderer>().sortingLayerName = "front";
            }
            
        }else{
            InventoryManager.instance.DesableCollectable();
        }
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin.transform.position,range_Collect);
    }

}
