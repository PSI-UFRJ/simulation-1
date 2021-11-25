using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserClick : MonoBehaviour
{
    public GameObject prefab;
    public bool isTemplate;

    private bool canMove; // Guarda a informação se pode mover o objeto
    private bool dragging; // Guarda a informação se o objeto está sendo arrastado
    private bool isInWorkspace; // Guarda a informação se o objeto que está na área de trabalho
    private bool enteredWorkspace; // Guarda a informação se o objeto que entrou na área de trabalho
    private bool insideToolsPanel; // Guarda a informação se o objeto se encontra na área de ferramentas
    private Vector3 initialPosition;

    private Collider2D collider; // Guarda o collider do Player

    [SerializeField]
    private GameObject controlObject; // Guarda referência para o GameObject pai dos Players
    private ObjectControlled control;

    [SerializeField]
    private GameObject workspace;

    [SerializeField]
    private LayerMask clickableLayer; // Guarda referência para a layer que indica quais objetos são clicáveis

    [SerializeField]
    //private UnityEngine.UI.Scrollbar sizeScrollbar; // Guarda referência para o scrollbar
    private UnityEngine.UI.Slider sizeSlider; // Guarda referência para o slider
    [SerializeField]
    private UnityEngine.UI.Text sizeText;

    //public float lastScrollbarValue; // Guarda o valor do scrollbar na última vez que este Player foi escalonado
    public float lastSliderValue; // Guarda o valor do scrollbar na última vez que este Player foi escalonado

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
    /// Executa as ações necessárias quando ocorre o clique do mouse
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

            if (enteredWorkspace)
            {
                Debug.Log("Objeto entrou na workspace");

                this.GetComponent<SpriteRenderer>().sortingOrder = workspace.GetComponent<SpriteRenderer>().sortingOrder + 2;
                Debug.Log("userClick - pai: " + this.GetComponent<SpriteRenderer>().sortingOrder);
                this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = workspace.GetComponent<SpriteRenderer>().sortingOrder + 1;
                Debug.Log("userClick - filho: " + this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder);
            }
            control.SelectObject(this.gameObject); // Informa ao controller que ele é o objeto selecionado e troca a cor do obj
            #region SizeController
            sizeSlider.value = lastSliderValue; // Altera o slider para o último valor
            sizeText.text = "" + (lastSliderValue + 1);
            #endregion

            #region DragAndDrop
            canMove = true;
            CreateTemplate();
            #endregion
        }
        else if (Physics2D.OverlapPoint(mousePos) == workspace.GetComponent<BoxCollider2D>()) // Usuário clicou em um espaço vazio do workspace
        {
            if (control != null)
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
    /// Executa as ações necessárias quando ocorre o desclique do mouse
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

        // Dentro da hierarquia dos objetos da cena, definimos quem é o seu pai
        obj.transform.SetParent(this.transform.parent);
        objChild.transform.SetParent(obj.transform);

        // Definimos sua posição
        obj.transform.position = this.transform.position;
        objChild.transform.position = obj.transform.position;

        // Definimos os componentes que esse novo objeto precisa ter para que o seu script funcione
        obj.GetComponent<UserClick>().prefab = this.GetComponent<UserClick>().prefab;
        obj.GetComponent<UserClick>().workspace = this.GetComponent<UserClick>().workspace;

        // O novo objeto passa a ser o template
        obj.GetComponent<UserClick>().isTemplate = true;

        initialPosition = this.transform.position;

        // O atual objeto deixa de ser o template e, agora, podemos movimentá-lo
        this.isTemplate = false;
    }

    private void InitGameObjects()
    {
        controlObject = this.transform.parent.gameObject;
        //sizeScrollbar = GameObject.Find("Scrollbar").GetComponent<UnityEngine.UI.Scrollbar>();
        sizeSlider = GameObject.Find("ChangeSizeSlider").GetComponent<UnityEngine.UI.Slider>();
        sizeText = GameObject.Find("ChangeSizeValueTxt").GetComponent<UnityEngine.UI.Text>();
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
        isInWorkspace = false;
        enteredWorkspace = false;
    }

    private void MoveToWorkspaceCenter()
    {
        Collider2D workspaceCollider = workspace.GetComponent<Collider2D>();
        Vector3 workspaceCenterPos = workspaceCollider.bounds.center;

        //Fix axis Z
        workspaceCenterPos.z = this.transform.position.z;

        StartCoroutine(MoveSmoothly(workspaceCenterPos));
    }

    IEnumerator MoveSmoothly(Vector3 destination)
    {
        float currentMovementTime = 0f;
        float totalMovementTime = 0.5f;
        Vector3 origin = transform.position;

        while (transform.position.x != destination.x && transform.position.y != destination.y)
        {
            currentMovementTime += Time.deltaTime;
            transform.position = Vector3.Lerp(origin, destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }

    private void MoveToClosestWorkspace(Collider2D collision)
    {
        Vector2 closetPos = collision.ClosestPoint(new Vector2(this.transform.position.x, this.transform.position.y));
        this.transform.position = new Vector3(closetPos.x, closetPos.y, this.transform.position.z); // Transporta o objeto para a região mais próxima do workspace
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
            isInWorkspace = true;
        }
        else if (collision.CompareTag("ToolsPanel"))
        {
            insideToolsPanel = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            Debug.Log("mamae cheguei");
            enteredWorkspace = true;
        }
    }
    #endregion

    #region Helper Methods
    public bool GetWorkspaceStatus()
    {
        return isInWorkspace;
    }
    #endregion
}
