using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diamond : MonoBehaviour, IShape
{
    private Color color;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeArea(float a)
    {

    }

    public void ChangePerimeter(float p)
    {

    }

    public float CalculateArea()
    {
        float area = 0;
        return area;
    }

    public float CalculatePerimeter()
    {
        float perimeter = 0;
        return perimeter;
    }

    public Sprite[] GetSprites()
    {
        return sprites;
    }

    public string GetShapeName()
    {
        return this.GetType().Name;
    }
}
