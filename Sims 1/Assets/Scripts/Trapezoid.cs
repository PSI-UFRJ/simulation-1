using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trapezoid : MonoBehaviour, IShape
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

    public void ChangeSize(float controllerValue, Vector3 baseScale, Slider sizeSlider, Text changeSizeText)
    {

    }

    public void ChangeSprite(int index)
    {
        if ((index < sprites.Length) && (index >= 0))
        {
            spriteRenderer.sprite = sprites[index];
        }

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
}
