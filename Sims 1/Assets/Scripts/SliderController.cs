using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IPointerDownHandler
{

    [SerializeField]
    private UnityEngine.UI.Slider sizeSlider;

    public string sliderName;

    [SerializeField]
    private GameObject shapes;

    private ObjectControlled objectControlled;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        sizeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        objectControlled = shapes.GetComponent<ObjectControlled>();
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {

        Debug.Log("Value Change sliderSize: " + sizeSlider.value);
        Debug.Log("Value Change sliderName: " + sliderName);

        objectControlled.ChangeScale(sliderName, sizeSlider.value);
        //ObjectControlled.SetNewScaleValue(sizeSlider.value, sliderName);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        objectControlled.enterSlider(sliderName, sizeSlider.value);
    }

    public void OnPointeUp(PointerEventData eventData)
    {
        objectControlled.exitSlider();
    }
}
