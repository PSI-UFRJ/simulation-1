using System;
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
    private GameObject shape; // Guarda referência para o GameObject pai dos Players
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

    // Item1: offset do eixo x; Item2: offset do eixo y
    private Tuple<float, float> shapeOffset; //Guarda o cálculo do offset a fim de permitir que o movimento das formas acompanhe de forma mais precisa o mouse

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
        ExecuteMouseButtonDownActions();

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (dragging)
        {
            // Desloca o objeto de forma suave baseado no offset calculado
            this.transform.position = new Vector3(mousePos.x - shapeOffset.Item1, mousePos.y - shapeOffset.Item2, this.transform.position.z);

            if (enteredWorkspace && control.GetObjectControlled() == this.gameObject)
            {
                // Alterar order in layer quando entrar no workspace. Usamos isso para o sprite se manter sobre a estrutura da simulação.
                this.GetComponent<SpriteRenderer>().sortingOrder = workspace.GetComponent<SpriteRenderer>().sortingOrder + 3;
            }
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

            control.UnselectObject(this.gameObject, false);
            control.SelectObject(this.gameObject); // Informa ao controller que ele é o objeto selecionado e troca a cor do obj
            #region SizeController
            sizeSlider.value = lastSliderValue; // Altera o slider para o último valor
            sizeText.text = "" + (lastSliderValue + 1); // Altera o slider para o último texto
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
                control.UnselectObject(this.gameObject, true);
            }
        }
        else
        {
            canMove = false;
        }

        if (canMove)
        {
            dragging = true;
            // Salva o offset do eixo x e do eixo y
            shapeOffset = new Tuple<float, float>(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);
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

        this.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 3;

        GameObject obj = Instantiate(prefab);

        // Dentro da hierarquia dos objetos da cena, definimos quem é o seu pai
        obj.transform.SetParent(this.transform.parent);

        // Definimos sua posição
        obj.transform.position = this.transform.position;

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
        shape = this.transform.parent.gameObject;;
        sizeSlider = GameObject.Find("ChangeSizeSlider").GetComponent<UnityEngine.UI.Slider>();
        sizeText = GameObject.Find("ChangeSizeValueTxt").GetComponent<UnityEngine.UI.Text>();
    }

    private void InitComponents()
    {
        collider = this.GetComponent<Collider2D>();
        control = shape.GetComponent<ObjectControlled>();
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
            enteredWorkspace = true;
        }
    }
    #endregion

    #region Helper Methods
    public bool GetWorkspaceStatus()
    {
        return isInWorkspace;
    }

    public GameObject GetWorkspace()
    {
        return workspace;
    }
    #endregion
}
