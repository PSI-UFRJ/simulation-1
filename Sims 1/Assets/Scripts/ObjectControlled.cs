using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlled : MonoBehaviour
{
    public GameObject objectControlled = null; // Guarda o objeto a ser controlado
    [SerializeField]
    private Rigidbody2D rb; // Guarda o Rigibbody do objeto a ser controlado
    private Color32 originalObjColor;
    private int redCol;
    private int greenCol;
    private int blueCol;


    [SerializeField]
    private UnityEngine.UI.Scrollbar scrollbar; // Guarda a instância do scrollbar
    [SerializeField]
    private Vector3 scale; // Guarda a escala atual
    [SerializeField]
    private Vector3 baseScale; // Guarda a escala base
    public int sizeScaler = 1;

    public void Update()
    {
        float area = CalculateArea();
        
        if(area != -1)
        {
            Debug.Log("Area do objeto selecionado: " + area);
        }
        
    }

    public void SelectObject(GameObject selectedObj)
    {
        // Sanity check
        if (selectedObj == null || selectedObj == objectControlled)
        {
            return;
        }

        // ----------------------------------------------- //
        // - Objeto clicado não é o que está selecionado - //
        // ----------------------------------------------- //

        // Reseta a cor do objeto que estava selecionado anteriormente
        if (objectControlled != null)
        {
            ResetColor(); // Reseta a cor
            UnableChildSprite(); // Desativa o realce do contorno
        }

        ChangeColor(selectedObj); // Muda a cor do objeto selecionado;
        EnableChildSprite(selectedObj); // Ativa o realce do contorno

        objectControlled = selectedObj; // Guarda a referência para o novo objeto selecionado
    }

    /// <summary>
    /// Desseleciona o objeto controlado
    /// </summary>
    public void UnselectObject()
    {
        // Sanity check
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus())
        {
            return;
        }

        ResetColor(); // Reseta a cor
        UnableChildSprite(); // Desativa o realce do contorno

        objectControlled = null; // Desfaz a referência do atual objeto controlado
    }

    /// <summary>
    /// Reseta a cor do objeto controlado
    /// </summary>
    private void ResetColor()
    {
        objectControlled.GetComponent<SpriteRenderer>().color = originalObjColor;
    }

    /// <summary>
    /// Desabilita o sprite do filho do objeto controlado
    /// </summary>
    private void UnableChildSprite()
    {
        GameObject selectedObjectChild;

        // Sanity check
        if(objectControlled.transform.childCount == 0)
        {
            Debug.Log("Objeto não apresenta um filho");
            return;
        }

        selectedObjectChild = objectControlled.transform.GetChild(0).gameObject; // Pega o objeto que da a cor amarela indicando qual era o objeto clicado
        selectedObjectChild.GetComponent<SpriteRenderer>().enabled = false; // Desativa o sprite renderer para aparecer o contorno
        selectedObjectChild.GetComponent<SpriteRenderer>().sortingOrder = selectedObjectChild.GetComponent<SpriteRenderer>().sortingOrder - 2;
        objectControlled.GetComponent<SpriteRenderer>().sortingOrder = objectControlled.GetComponent<SpriteRenderer>().sortingOrder - 2;
    }

    /// <summary>
    /// Habilita o sprite do filho do atual objeto selecionado
    /// </summary>
    /// <param name="selectedObj"></param>
    private void EnableChildSprite(GameObject selectedObj)
    {
        GameObject selectedObjectChild;

        // Sanity check
        if (selectedObj.transform.childCount == 0)
        {
            Debug.Log("Objeto não apresenta um filho");
            return;
        }

        selectedObjectChild = selectedObj.transform.GetChild(0).gameObject; // Pega o objeto que da a cor amarela indicando que foi clicado
        selectedObjectChild.GetComponent<SpriteRenderer>().enabled = true; // Ativa o sprite renderer para aparecer o contorno
        selectedObj.GetComponent<SpriteRenderer>().sortingOrder = selectedObj.GetComponent<SpriteRenderer>().sortingOrder + 2;
        selectedObjectChild.GetComponent<SpriteRenderer>().sortingOrder = selectedObjectChild.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }

    /// <summary>
    /// Muda a cor do objeto selecionado
    /// </summary>
    /// <param name="selectedObj"></param>
    private void ChangeColor(GameObject selectedObj)
    {
        originalObjColor = selectedObj.GetComponent<SpriteRenderer>().color; // Pega a cor do novo objeto clicado
        redCol = originalObjColor.r - 30;
        greenCol = originalObjColor.g - 30;
        blueCol = originalObjColor.b - 30;
        selectedObj.GetComponent<SpriteRenderer>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255); // Troca para a cor "selecionado"
    }

    /// <summary>
    /// Muda a escala do objeto controlado
    /// </summary>
    /// <param name="newScale"></param>
    public void ChangeScale(float newScale)
    {
        // Sanity check
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus())
        {
            return;
        }

        rb = objectControlled.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Impede que o objeto rotacione enquanto é escalado
        scale = baseScale + new Vector3(newScale * sizeScaler, newScale * sizeScaler, newScale * sizeScaler); // Gera a nova escala baseado na movimentação do scrollbar (value)
        objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        rb.freezeRotation = false;
        objectControlled.GetComponent<UserClick>().lastScrollbarValue = scrollbar.value; // Guarda no objeto controlado o último valor no scrollbar
    }

    /// <summary>
    /// Deleta o objeto controlado
    /// </summary>
    public void DeleteObject()
    {
        // Sanity check
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus())
        {
            return;
        }

        Destroy(objectControlled);
    }

    /// <summary>
    /// Calcula a área do objeto controlado
    /// </summary>
    public float CalculateArea()
    {
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus()) // Verifica se há algum obj selecionado e se sim, se está no workspace
        {
            return -1;
        }

        float result;

        if (objectControlled.GetComponent<CircleCollider2D>() != null) // Verifica se a forma é um círculo
        {
            float radius = objectControlled.GetComponent<CircleCollider2D>().radius; // Pega o raio do círculo do collider
            result = CalculateAreaCircle(radius);
        }
        else if (objectControlled.GetComponent<PolygonCollider2D>() != null) // Verifica se a forma é um polígono
        {
            Vector2[] points = objectControlled.GetComponent<PolygonCollider2D>().points; // Pega os pontos que foram o collider do polígono
            result = CalculateAreaPolygon(points);
        }
        else
        {
            return -1;
        }

        return result;
    }

    /// <summary>
    /// Calcula a área de um polígono
    /// </summary>
    /// <param name="v"></param>
    private float CalculateAreaPolygon(Vector2[] v)
    {
        float temp = 0;
        float result = 0;

        Debug.Log("vertices do obj controlado: " + v.GetValue(0));

        Vector2[] vertices = (Vector2[])v.Clone();

        for(int i = 0; i < vertices.Length; i++) // Escalona o objeto pra ter os pontos "reais"
        {
            vertices[i].x = vertices[i].x * objectControlled.transform.localScale.x;
            vertices[i].y = vertices[i].y * objectControlled.transform.localScale.y;
        }

        Debug.Log("vertices do obj controlado: " + vertices.GetValue(0));

        for (int i = 0; i < vertices.Length; i++) // Faz o cálculo da área de um polígono regular qualquer
        {   
            if (i != vertices.Length - 1)
            {
                float mulA = vertices[i].x * vertices[i + 1].y;
                float mulB = vertices[i + 1].x * vertices[i].y;
                temp = temp + (mulA - mulB);
            }
            else
            {
                float mulA = vertices[i].x * vertices[0].y;
                float mulB = vertices[0].x * vertices[i].y;
                temp = temp + (mulA - mulB);
            }
        }
        temp *= 0.5f;
        result = Mathf.Abs(temp); // Deixa positivo

        return result;
    }

    /// <summary>
    /// Calcula a área de um círculo
    /// </summary>
    /// <param name="v"></param>
    private float CalculateAreaCircle(float radius)
    {
        
        float relativeRadius = radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala
        Debug.Log("raio relativo do obj controlado: " + relativeRadius);
        Debug.Log("raio do obj controlado: " + radius);
        return Mathf.PI * relativeRadius * relativeRadius;
    }

}
