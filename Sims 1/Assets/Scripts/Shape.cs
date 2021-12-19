using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IShape 
{
    float CalculateArea(GameObject objectControlled);
    float CalculatePerimeter(GameObject objectControlled);
    Sprite[] GetSprites();
    string GetShapeName();
    Dictionary<string, float> GetMetrics(GameObject objectControlled);
    Dictionary<string, GameObject> GetMappedControllers();
    void SetMappedControllers(Dictionary<string, GameObject> mappedControllers);
    Dictionary<string, float> GetLastMetrics();
    void SetLastMetrics(Dictionary<string, float> lastMetrics);
    int GetSpriteIndex(string name);
    float GetReferenceValue();
    void SetScale(string slideName, float size, GameObject objectControlled);
}
