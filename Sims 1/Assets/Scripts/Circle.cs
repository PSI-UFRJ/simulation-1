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

    public void ChangeSize(float controllerValue, Vector3 baseScale, Slider sizeSlider, Text changeSizeText)
    {
        rb = this.GetComponent<Rigidbody2D>();

        rb.freezeRotation = true; // Impede que o objeto rotacione enquanto é escalado

        scale = baseScale + new Vector3(controllerValue * sizeScaler, controllerValue * sizeScaler, controllerValue * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
        this.transform.localScale = scale; // Muda a escala local do objeto controlado
        changeSizeText.text = "" + (sizeSlider.value + 1);

        rb.freezeRotation = false;

        this.GetComponent<UserClick>().lastSliderValue = sizeSlider.value; // Guarda no objeto controlado o último valor no slider
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
