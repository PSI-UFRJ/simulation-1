using System;
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
    private bool isInWorkspace; // Guarda a informa��o se o objeto que est� na �rea de trabalho
    private bool enteredWorkspace; // Guarda a informa��o se o objeto que entrou na �rea de trabalho
    private bool insideToolsPanel; // Guarda a informa��o se o objeto se encontra na �rea de ferramentas
    private Vector3 initialPosition;

    private Collider2D collider; // Guarda o collider do Player

    [SerializeField]
    private GameObject shape; // Guarda refer�ncia para o GameObject pai dos Players
    private ObjectControlled control;

    [SerializeField]
    private GameObject workspace;

    [SerializeField]
    private LayerMask clickableLayer; // Guarda refer�ncia para a layer que indica quais objetos s�o clic�veis

    [SerializeField]
    //private UnityEngine.UI.Scrollbar sizeScrollbar; // Guarda refer�ncia para o scrollbar
    private UnityEngine.UI.Slider sizeSlider; // Guarda refer�ncia para o slider
    [SerializeField]
    private UnityEngine.UI.Text sizeText;

    //public float lastScrollbarValue; // Guarda o valor do scrollbar na �ltima vez que este Player foi escalonado
    public float lastSliderValue; // Guarda o valor do scrollbar na �ltima vez que este Player foi escalonado

    // Item1: offset do eixo x; Item2: offset do eixo y
    private Tuple<float, float> shapeOffset; //Guarda o c�lculo do offset a fim de permitir que o movimento das formas acompanhe de forma mais precisa o mouse

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
                // Alterar order in layer quando entrar no workspace. Usamos isso para o sprite se manter sobre a estrutura da simula��o.
                this.GetComponent<SpriteRenderer>().sortingOrder = workspace.GetComponent<SpriteRenderer>().sortingOrder + 3;
            }
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

            control.UnselectObject(this.gameObject, false);
            control.SelectObject(this.gameObject); // Informa ao controller que ele � o objeto selecionado e troca a cor do obj
            #region SizeController
            sizeSlider.value = lastSliderValue; // Altera o slider para o �ltimo valor
            sizeText.text = "" + (lastSliderValue + 1); // Altera o slider para o �ltimo texto
            #endregion

            #region DragAndDrop
            canMove = true;
            CreateTemplate();
            #endregion
        }
        else if (Physics2D.OverlapPoint(mousePos) == workspace.GetComponent<BoxCollider2D>()) // Usu�rio clicou em um espa�o vazio do workspace
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

        this.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 3;

        GameObject obj = Instantiate(prefab);

        // Dentro da hierarquia dos objetos da cena, definimos quem � o seu pai
        obj.transform.SetParent(this.transform.parent);

        // Definimos sua posi��o
        obj.transform.position = this.transform.position;

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
