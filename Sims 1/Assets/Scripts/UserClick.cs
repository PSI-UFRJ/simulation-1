using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserClick : MonoBehaviour
{
    public GameObject prefab;
    public bool isTemplate;

    private bool canMove; // Guarda a informa��o se pode mover o objeto
    private bool dragging; // Guarda a informa��o se o objeto est� sendo arrastado
    private bool enteredWorkspace; // Guarda a informa��o se o objeto entrou na �rea de trabalho
    private bool insideToolsPanel; // Guarda a informa��o se o objeto se encontra na �rea de ferramentas
    private Vector3 initialPosition;

    private Collider2D collider; // Guarda o collider do Player

    [SerializeField]
    private GameObject controlObject; // Guarda refer�ncia para o GameObject pai dos Players
    private ObjectControlled control;

    [SerializeField]
    private GameObject workspace;

    [SerializeField]
    private LayerMask clickableLayer; // Guarda refer�ncia para a layer que indica quais objetos s�o clic�veis

    [SerializeField]
    private UnityEngine.UI.Scrollbar sizeScrollbar; // Guarda refer�ncia para o scrollbar

    public float lastScrollbarValue; // Guarda o valor do scrollbar na �ltima vez que este Player foi escalonado

    // Start is called before the first frame update
    void Start()
    {
        InitGameObjects();
        InitComponents();
        InitStatusVariables();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ExecuteMouseButtonDownActions();

        if (dragging)
        {
            this.transform.position = new Vector3(mousePos.x, mousePos.y, this.transform.position.z);
        }

        ExecuteMouseButtonUpActions();
    }

    #region Auxiliary Methods
    /// <summary>
    /// Executa as a��es necess�rias quando ocorre o clique do mouse
    /// </summary>
    private void ExecuteMouseButtonDownActions()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Sanity check
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (collider == Physics2D.OverlapPoint(mousePos))
        {
            Debug.Log("Objeto entrou na workspace");

            control.SelectObject(this.gameObject); // Informa ao controller que ele � o objeto selecionado e troca a cor do obj
            #region SizeController
            sizeScrollbar.value = lastScrollbarValue; // Altera o scrollbar para o �ltimo valor
            #endregion

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, initialPosition.z + 1); // Desce um n�vel do eixo Z

            #region DragAndDrop
            canMove = true;
            CreateTemplate();
            #endregion
        }
        else if (Physics2D.OverlapPoint(mousePos) == workspace.GetComponent<BoxCollider2D>()) // Usu�rio clicou em um espa�o vazio do workspace
        {
            if(control != null)
            {
                Debug.Log("Deselecionando a cor");
                control.UnselectObject();
            }
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

    /// <summary>
    /// Executa as a��es necess�rias quando ocorre o desclique do mouse
    /// </summary>
    private void ExecuteMouseButtonUpActions()
    {
        // Sanity check
        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }

        canMove = false;
        dragging = false;

        if (!IsInsideWorkspace())
        {
            MoveToWorkspaceCenter();
            insideToolsPanel = false;
        }
    }

    private bool IsInsideWorkspace()
    {
        return !(insideToolsPanel && !isTemplate);
    }

    private void CreateTemplate()
    {
        // Sanity check
        if (!this.isTemplate)
        {
            return;
        }

        GameObject obj = Instantiate(prefab);
        GameObject objChild = obj.transform.GetChild(0).gameObject;

        // Dentro da hierarquia dos objetos da cena, definimos quem � o seu pai
        obj.transform.SetParent(this.transform.parent);
        objChild.transform.SetParent(obj.transform);

        // Definimos sua posi��o
        obj.transform.position = this.transform.position;
        objChild.transform.position = obj.transform.position;

        // Definimos os componentes que esse novo objeto precisa ter para que o seu script funcione
        obj.GetComponent<UserClick>().prefab = this.GetComponent<UserClick>().prefab;
        obj.GetComponent<UserClick>().workspace = this.GetComponent<UserClick>().workspace;

        // O novo objeto passa a ser o template
        obj.GetComponent<UserClick>().isTemplate = true;

        initialPosition = this.transform.position;

        // O atual objeto deixa de ser o template e, agora, podemos moviment�-lo
        this.isTemplate = false;
    }

    private void InitGameObjects()
    {
        controlObject = this.transform.parent.gameObject;
        sizeScrollbar = GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
    }

    private void InitComponents()
    {
        collider = GetComponent<Collider2D>();
        control = controlObject.GetComponent<ObjectControlled>();
    }

    private void InitStatusVariables()
    {
        canMove = false;
        dragging = false;
        enteredWorkspace = false;
    }
    
    private void MoveToWorkspaceCenter()
    {
        Collider2D workspaceCollider = workspace.GetComponent<Collider2D>();
        Vector2 closetPos = workspaceCollider.bounds.center;
        this.transform.position = new Vector3(closetPos.x, closetPos.y, this.transform.position.z); // Transporta o objeto para o centro do workspace
    }

    private void MoveToClosestWorkspace(Collider2D collision)
    {
        Vector2 closetPos = collision.ClosestPoint(new Vector2(this.transform.position.x, this.transform.position.y));
        this.transform.position = new Vector3(closetPos.x, closetPos.y, this.transform.position.z); // Transporta o objeto para a regi�o mais pr�xima do workspace
    }
    #endregion

    #region Collider Triggers
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            MoveToClosestWorkspace(collision);
            dragging = false;
        }
        else if (collision.CompareTag("WorkspaceContainer"))
        {
            MoveToWorkspaceCenter();
            dragging = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            insideToolsPanel = false;
            enteredWorkspace = true;
        }
        else if (collision.CompareTag("ToolsPanel"))
        {
            insideToolsPanel = true;
        }
    }
    #endregion

    #region Helper Methods
    public bool GetWorkspaceStatus()
    {
        return enteredWorkspace;
    }
    #endregion
}
