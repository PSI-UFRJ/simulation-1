using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlled : MonoBehaviour
{
    public GameObject objectControlled = null; // Guarda o objeto a ser controlado
    [SerializeField]
    private Rigidbody2D rb; // Guarda o Rigibbody do objeto a ser controlado
    private Color32 originalObjColor;
    private int colorSelectedOffset = 30;

    [SerializeField]
    private Color32[] possibleObjColors;
    private int redCol;
    private int greenCol;
    private int blueCol;


    [SerializeField]
    //private UnityEngine.UI.Scrollbar scrollbar; // Guarda a instância do scrollbar
    private UnityEngine.UI.Slider sizeSlider; // Guarda a instância do slider
    [SerializeField]
    private UnityEngine.UI.Text changeSizeText;

    private UnityEngine.UI.Image colorDisplayImg;


    [SerializeField]
    private Vector3 scale; // Guarda a escala atual
    //[SerializeField]
    private Vector3 baseScale; // Guarda a escala base
    public int sizeScaler = 1;

    private void Start()
    {
        colorDisplayImg = GameObject.Find("ColorPickerImg").GetComponent<UnityEngine.UI.Image>();
    }

    public void Update()
    {
        float area = CalculateArea();
        
        if(area != -1)
        {
            //Debug.Log("Area do objeto selecionado: " + area);
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

        ChangeColorSelected(selectedObj); // Muda a cor do objeto selecionado;
        EnableChildSprite(selectedObj); // Ativa o realce do contorno

        objectControlled = selectedObj; // Guarda a referência para o novo objeto selecionado
        baseScale = selectedObj.GetComponent<UserClick>().prefab.transform.localScale;
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
    private void ChangeColorSelected(GameObject selectedObj)
    {
        originalObjColor = selectedObj.GetComponent<SpriteRenderer>().color; // Pega a cor do novo objeto clicado
        redCol = originalObjColor.r - colorSelectedOffset;
        greenCol = originalObjColor.g - colorSelectedOffset;
        blueCol = originalObjColor.b - colorSelectedOffset;
        selectedObj.GetComponent<SpriteRenderer>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255); // Troca para a cor "selecionado"
        colorDisplayImg.color = originalObjColor;
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

        scale = baseScale + new Vector3(newScale * sizeScaler, newScale * sizeScaler, newScale * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
        objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        changeSizeText.text = "" + (sizeSlider.value + 1);

        rb.freezeRotation = false;

        objectControlled.GetComponent<UserClick>().lastSliderValue = sizeSlider.value; // Guarda no objeto controlado o último valor no slider
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
    /// Deleta todos as formas
    /// </summary>
    public void DeleteAll()
    {
        foreach (Transform child in this.transform)
        {
            GameObject childGameObj = child.gameObject;

            if (childGameObj.GetComponent<UserClick>() != null && childGameObj.GetComponent<UserClick>().GetWorkspaceStatus())
            {
                Destroy(childGameObj);
            }
        }
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

        //Debug.Log("vertices do obj controlado: " + v.GetValue(0));

        Vector2[] vertices = (Vector2[])v.Clone();

        for(int i = 0; i < vertices.Length; i++) // Escalona o objeto pra ter os pontos "reais"
        {
            vertices[i].x = vertices[i].x * objectControlled.transform.localScale.x;
            vertices[i].y = vertices[i].y * objectControlled.transform.localScale.y;
        }

        //Debug.Log("vertices do obj controlado: " + vertices.GetValue(0));

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
    /// <param name="radius"></param>
    private float CalculateAreaCircle(float radius)
    {
        
        float relativeRadius = radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala
        //Debug.Log("raio relativo do obj controlado: " + relativeRadius);
        //Debug.Log("raio do obj controlado: " + radius);
        return Mathf.PI * relativeRadius * relativeRadius;
    }

    #region Color Controller
    /// <summary>
    /// Muda a cor da forma selecionada através do color controller (subindo na lista de cores)
    /// </summary>
    public void ChangeColorPickerUp()
    {
        int colorIndex = possibleObjColors.Length - 1;

        for (int i = 0; i < possibleObjColors.Length; i++) // Encontra o index da cor atual na lista
        {
            if (isColorsEqual(possibleObjColors[i], originalObjColor))
            {
                colorIndex = i;
                break;
            } 
        }

        originalObjColor = possibleObjColors[mod(colorIndex - 1, possibleObjColors.Length)]; // Troca o valor da cor original para a cor selecionada
        redCol = originalObjColor.r - colorSelectedOffset;
        greenCol = originalObjColor.g - colorSelectedOffset;
        blueCol = originalObjColor.b - colorSelectedOffset;
        objectControlled.GetComponent<SpriteRenderer>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255); // Troca para a cor selecionada no picker mas no estado "selecionado"
        colorDisplayImg.color = originalObjColor; // Coloca a cor selecionada no visor do picker

    }

    /// <summary>
    /// Muda a cor da forma selecionada através do color controller (descendo na lista de cores)
    /// </summary>
    public void ChangeColorPickerDown()
    {
        int colorIndex = 0;

        for (int i = 0; i < possibleObjColors.Length; i++) // Encontra o index da cor atual na lista
        {
            if (isColorsEqual(possibleObjColors[i], originalObjColor))
            {
                colorIndex = i;
                break;
            }
        }
        
        originalObjColor = possibleObjColors[mod(colorIndex + 1, possibleObjColors.Length)]; // Troca o valor da cor original para a cor selecionada
        redCol = originalObjColor.r - colorSelectedOffset;
        greenCol = originalObjColor.g - colorSelectedOffset;
        blueCol = originalObjColor.b - colorSelectedOffset;
        objectControlled.GetComponent<SpriteRenderer>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255); // Troca para a cor selecionada no picker mas no estado "selecionado"
        colorDisplayImg.color = originalObjColor; // Coloca a cor selecionada no visor do picker
    }
    #endregion

    #region Rotation Controller
    public void RotateLeft()
    {
        // Sanity check
        if (objectControlled == null)
        {
            return;
        }

        objectControlled.transform.eulerAngles += (Vector3.forward * 45);
    }

    public void RotateRight()
    {
        // Sanity check
        if (objectControlled == null)
        {
            return;
        }

        objectControlled.transform.eulerAngles += (Vector3.forward * -45);
    }

    #endregion

    #region Auxiliary Methods
    /// <summary>
    /// Verifica se duas cores são iguais comparando os valores RGB (o valor alfa não é comparado)
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    private bool isColorsEqual(Color32 c1, Color32 c2)
    {
        return (c1.r == c2.r) && (c1.g == c2.g) && (c1.b == c2.b);
    }

    /// <summary>
    /// Calcula o módulo
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int mod(int a, int b)
    {
        return (a % b + b) % b;
    }
    #endregion
}
