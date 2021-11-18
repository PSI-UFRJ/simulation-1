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
        EnableChieldSprite(selectedObj); // Ativa o realce do contorno

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
    }

    /// <summary>
    /// Habilita o sprite do filho do atual objeto selecionado
    /// </summary>
    /// <param name="selectedObj"></param>
    private void EnableChieldSprite(GameObject selectedObj)
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
}
