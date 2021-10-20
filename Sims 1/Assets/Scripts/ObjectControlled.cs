using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlled : MonoBehaviour
{
    public GameObject objectControlled = null; // Guarda o objeto a ser controlado
    public int        sizeScaler       = 1;

    [SerializeField]
    private UnityEngine.UI.Scrollbar scrollbar; // Guarda a instância do scrollbar
    [SerializeField]
    private Vector3 scale; // Guarda a escala atual
    [SerializeField]
    private Vector3 baseScale; // Guarda a escala base
    [SerializeField]
    private Rigidbody2D rb; // Guarda o Rigibbody do objeto a ser controlado
    
    public void ChangeScale(float newScale)
    {
        if (objectControlled == null)
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
}
