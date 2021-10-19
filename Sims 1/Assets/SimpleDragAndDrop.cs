using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField]
    private Canvas      canvas;

    public GameObject   prefab;
    public bool         isTemplate;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isTemplate)
        {
            // Cria um novo objeto
            GameObject obj = Instantiate(prefab);

            // Dentro da hierarquia dos objetos da cena, definimos quem é o seu pai
            obj.transform.SetParent(canvas.transform);
            
            // Definimos sua posição
            obj.transform.position                              = this.transform.position;

            // Definimos os componentes que esse novo objeto precisa ter para que o seu script funcione
            obj.GetComponent<SimpleDragAndDrop>().canvas        = this.GetComponent<SimpleDragAndDrop>().canvas;
            obj.GetComponent<SimpleDragAndDrop>().prefab        = this.GetComponent<SimpleDragAndDrop>().prefab;

            // O novo objeto passa a ser o template
            obj.GetComponent<SimpleDragAndDrop>().isTemplate    = true;

            // O atual objeto deixa de ser o template e, agora, podemos movimentá-lo
            this.isTemplate = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTemplate)
        {
            this.transform.position = eventData.position;
        }
    }
}
