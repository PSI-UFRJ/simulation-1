using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IShape 
{
    public void ChangeSize(float controllerValue, Vector3 baseScale, Slider sizeSlider, Text changeSizeText);
    public void ChangeSprite(int index);
    public void ChangeArea(float a);
    public void ChangePerimeter(float p);
    public float CalculateArea();
    public float CalculatePerimeter();
}
