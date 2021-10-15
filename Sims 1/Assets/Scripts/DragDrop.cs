using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    public GameObject   prefab;
    public bool         isTemplate;

    private bool        canMove;
    private bool        dragging;
    private Collider2D  collider;

    void Start()
    {
        collider    = GetComponent<Collider2D>();
        canMove     = false;
        dragging    = false;
    }

    // Update is called once per frame
    void Update()
    {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {

                if (collider == Physics2D.OverlapPoint(mousePos))
                {
                    Debug.Log("Clicou na bolota");
                    canMove = true;
                    CreateTemplate();
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

            if (dragging)
            {
                this.transform.position = mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canMove  = false;
                dragging = false;
            }

    }

    private void CreateTemplate()
    {
        // Sanity check
        if (!this.isTemplate)
        {
            return;
        }

        Debug.Log("Criando obj");

        GameObject obj                          = Instantiate(prefab);

        // Dentro da hierarquia dos objetos da cena, definimos quem é o seu pai
        obj.transform.SetParent(this.transform.parent);

        // Definimos sua posição
        obj.transform.position                  = this.transform.position;

        // Definimos os componentes que esse novo objeto precisa ter para que o seu script funcione
        obj.GetComponent<DragDrop>().prefab     = this.GetComponent<DragDrop>().prefab;

        // O novo objeto passa a ser o template
        obj.GetComponent<DragDrop>().isTemplate = true;

        // O atual objeto deixa de ser o template e, agora, podemos movimentá-lo
        this.isTemplate                         = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("cheguei");
        if (collision.CompareTag("Workspace"))
        {
            Vector2 closetPos = collision.ClosestPoint(new Vector2(this.transform.position.x, this.transform.position.y));
            Debug.Log("passei no workspace");
            dragging = false;
            this.transform.position = new Vector3(closetPos.x, closetPos.y, this.transform.position.z);
            Debug.Log("Oieee");
        }
    }
}
