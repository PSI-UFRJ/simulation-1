using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClick : MonoBehaviour
{
    public GameObject prefab;
    public bool isTemplate;

    private bool canMove; // Guarda a informa��o se pode mover o objeto
    private bool dragging; // Guarda a informa��o se o objeto est� sendo arrastado
    private bool enteredWorkspace; // Guarda a informa��o se o objeto entrou na �rea de trabalho

    private Collider2D collider; // Guarda o collider do Player

    [SerializeField]
    private GameObject controlObject; // Guarda refer�ncia para o GameObject pai dos Players
    private ObjectControlled control;

    [SerializeField]
    private LayerMask clickableLayer; // Guarda refer�ncia para a layer que indica quais objetos s�o clic�veis

    [SerializeField]
    private UnityEngine.UI.Scrollbar sizeScrollbar; // Guarda refer�ncia para o scrollbar

    public float lastScrollbarValue; // Guarda o valor do scrollbar na �ltima vez que este Player foi escalonado

    // Start is called before the first frame update
    void Start()
    {
        collider                = GetComponent<Collider2D>();
        controlObject           = this.transform.parent.gameObject;
        control                 = controlObject.GetComponent<ObjectControlled>();
        sizeScrollbar           = GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
        canMove                 = false;
        dragging                = false;
        enteredWorkspace        = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (collider == Physics2D.OverlapPoint(mousePos))
            {
                #region SizeController
                if (enteredWorkspace) // Se o objeto entrou na workspace
                {
                    control.SelectObject(this.gameObject); // Informa ao controller que ele � o objeto selecionado e troca a cor do obj
                    sizeScrollbar.value = lastScrollbarValue; // Altera o scrollbar para o �ltimo valor
                }
                
                #endregion

                #region DragAndDrop
                canMove = true;
                CreateTemplate();
                #endregion


            }
            else
            {
                canMove = false;
            }
            if (canMove)
            {
                dragging = true;
            }
            
        }

        #region DragAndDrop
        if (dragging)
        {
            this.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            dragging = false;
            
        }
        #endregion

    }

    private void CreateTemplate()
    {
        // Sanity check
        if (!this.isTemplate)
        {
            return;
        }

        GameObject obj = Instantiate(prefab);

        // Dentro da hierarquia dos objetos da cena, definimos quem � o seu pai
        obj.transform.SetParent(this.transform.parent);

        // Definimos sua posi��o
        obj.transform.position = this.transform.position;

        // Definimos os componentes que esse novo objeto precisa ter para que o seu script funcione
        obj.GetComponent<UserClick>().prefab = this.GetComponent<UserClick>().prefab;

        // O novo objeto passa a ser o template
        obj.GetComponent<UserClick>().isTemplate = true;

        // O atual objeto deixa de ser o template e, agora, podemos moviment�-lo
        this.isTemplate = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            Vector2 closetPos = collision.ClosestPoint(new Vector2(this.transform.position.x, this.transform.position.y));
            dragging = false;
            this.transform.position = new Vector3(closetPos.x, closetPos.y, this.transform.position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            enteredWorkspace = true;
        }
    }
}
