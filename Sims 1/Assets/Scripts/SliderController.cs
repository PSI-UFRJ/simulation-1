using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private UnityEngine.UI.Slider sizeSlider;

    public string sliderName;
    public string elementName;

    [SerializeField]
    private GameObject shapes;

    private ObjectControlled objectControlled;

    public void Start()
    {
        objectControlled = shapes.GetComponent<ObjectControlled>();

        //Adds a listener to the main slider and invokes a method when the value changes.
        sizeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        objectControlled.ChangeScale(sliderName, sizeSlider.value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        objectControlled.enterSlider(sliderName, sizeSlider.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        objectControlled.exitSlider();
    }
}
