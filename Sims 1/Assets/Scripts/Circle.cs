using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IShape
{
    // Start is called before the first frame update
    private float radius;
    private Color color;
    private CircleCollider2D collider;
    private Transform transform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    void Start()
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();
        transform = this.gameObject.GetComponent<Transform>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        radius = collider.radius * transform.localScale.x;
        color = spriteRenderer.color;
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
