using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IShape 
{
    void ChangeArea(float a);
    void ChangePerimeter(float p);
    float CalculateArea();
    float CalculatePerimeter();
    Sprite[] GetSprites();
    string GetShapeName();
}
