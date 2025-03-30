using UnityEngine;

public class PrefabPlanta : MonoBehaviour
{
    public bool coll;
    bool distance;

    [Header("Colors")]
    [SerializeField] private Color colorDefault;
    [SerializeField] private Color colorColl;

    private ItemInventory item;

    void Awake()
    {
        coll = false;
        distance = false;
    }

    void LateUpdate()
    {
        // Obtém a posição do mouse em coordenadas de tela
        Vector3 mousePosition = Input.mousePosition;

        // Converte a posição do mouse para coordenadas do mundo
        mousePosition.z = Camera.main.nearClipPlane; // Ajusta a distância da câmera
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Define a posição do objeto
        transform.position = new Vector3(worldPosition.x, worldPosition.y,0f);

        if(coll) GetComponent<SpriteRenderer>().color = colorColl;
        else GetComponent<SpriteRenderer>().color = colorDefault;

        if(Input.GetMouseButtonUp(0)){
            Instante();
        }

        if(Vector2.Distance(Buscador.buscar.player.transform.position,transform.position) > 3f){
            coll = true;
            distance = true;
        }else{
            if(distance){
                coll = false;
                distance = false;
            }
        }
    }

    public void Instante(){
        if(!coll){
            Instantiate(item.item.planta.prefab,transform.position,Quaternion.identity);

            //Tira um item do inventario
            item.DecrementItem(1);
        }

        gameObject.SetActive(false);
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
    }

    public void Style(ItemInventory it){
        item = it;

        GetComponent<SpriteRenderer>().sprite = it.item.planta.img;

        gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {  
        if(collision.gameObject){
            coll = true;
        }else{
            coll = false;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject){
            coll = false;
        }
    }
}
