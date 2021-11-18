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
        if (selectedObj == null)
        {
            return;
        }

        if (selectedObj != objectControlled) // Se o objeto clicado não é o que já está selecionado
        {
            if (objectControlled != null) // Se já foi selecionado algum objeto
            {
                objectControlled.GetComponent<SpriteRenderer>().color = originalObjColor; // Reseta a cor inicial dele
            }
            originalObjColor = selectedObj.GetComponent<SpriteRenderer>().color; // Pega a cor do novo objeto clicado
            redCol = originalObjColor.r - 30;
            greenCol = originalObjColor.g - 30;
            blueCol = originalObjColor.b - 30;
            selectedObj.GetComponent<SpriteRenderer>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255); // Troca para a cor "selecionado"
            objectControlled = selectedObj; // Guarda a referência para o novo objeto selecionado
        }

    }

    public void ChangeScale(float newScale)
    {
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

    public void DeleteObject()
    {
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus())
        {
            return;
        }

        Destroy(objectControlled);
    }

    public void UnselectObject()
    {
        if (objectControlled == null || !objectControlled.GetComponent<UserClick>().GetWorkspaceStatus())
        {
            return;
        }

        objectControlled.GetComponent<SpriteRenderer>().color = originalObjColor; // Reseta a cor inicial dele

        objectControlled = null;
    }
}
