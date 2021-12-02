using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShape 
{
    public void ChangeColor(Color c);
    public void ChangeSize(float controllerValue);
    public void ChangeSprite(int index);
    public void ChangeArea(float a);
    public void ChangePerimeter(float p);
    public void Rotate(int angle=45);
    public float CalculateArea();
    public float CalculatePerimeter();
    public void Delete();

}
