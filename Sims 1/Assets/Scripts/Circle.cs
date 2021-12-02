using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour, IShape
{
    // Start is called before the first frame update
    private float radius;
    private Color color;
    private Vector3 baseScale = new Vector3(1, 1, 1);
    private CircleCollider2D collider;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();
        transform = this.gameObject.GetComponent<Transform>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        radius = collider.radius * transform.localScale.x;
        color = spriteRenderer.color;
    }

    public void ChangeColor(Color c)
    {

    }

    public void ChangeSize(float controllerValue)
    {

    }

    public void ChangeSprite(int index)
    {
        if((index < sprites.Length) && (index >= 0))
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

    public void Rotate(int angle = 45)
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

    public void Delete()
    {

    }
}
