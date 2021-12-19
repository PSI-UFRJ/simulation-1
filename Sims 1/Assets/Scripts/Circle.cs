using System;
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

    public static float initialSizeRadius = 1;
    public static float initialSizeDiameter = 2;

    public List<GameObject> controllers = null;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeRadiusSizeController", initialSizeRadius }, { "ShapeDiameterSizeController", initialSizeDiameter }
    };

    void Start()
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();
        transform = this.gameObject.GetComponent<Transform>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        radius = collider.radius * transform.localScale.x;
        color = spriteRenderer.color;

        if (controllers != null)
        {
            controllersListToDict(mappedControllers, controllers);
        }
    }

    public float CalculateArea(GameObject objectControlled)
    {
        float area = 0;
        return area;
    }

    public float CalculateRadius(GameObject objectControlled)
    {
        return collider.radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala;
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        float perimeter = 0;
        return perimeter;
    }

    public float CalculateDiameter(GameObject objectControlled)
    {
        return CalculateRadius(objectControlled) * 2;
    }

    public Sprite[] GetSprites()
    {
        return sprites;
    }

    public string GetShapeName()
    {
        return this.GetType().Name;
    }

    public Dictionary<string, float> GetMetrics(GameObject objectControlled)
    {
        return new Dictionary<string, float>()  {
            {"ShapeRadiusSizeController",  CalculateRadius(objectControlled)},
            {"ShapeDiameterSizeController",  CalculateDiameter(objectControlled)}
        };
    }

    private void controllersListToDict(Dictionary<string, GameObject> mappedControllers, List<GameObject> controllers)
    {
        foreach (GameObject gameObj in controllers)
        {
            mappedControllers.Add(gameObj.name.ToString().Replace("ControlPanel", "").Trim(), gameObj);
        }
    }

    public Dictionary<string, GameObject> GetMappedControllers()
    {
        return mappedControllers;
    }

    public void SetMappedControllers(Dictionary<string, GameObject> mappedControllers)
    {
        this.mappedControllers = mappedControllers;
    }

    public Dictionary<string, float> GetLastMetrics()
    {
        return lastMetrics;
    }

    public void SetLastMetrics(Dictionary<string, float> lastMetrics)
    {
        this.lastMetrics = lastMetrics;
    }

    public void SetScale(string slideName, float size, GameObject objectControlled)
    {
        if (slideName.IndexOf("radius", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler * 2, size * sizeScaler * 2, size * sizeScaler * 2); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("diameter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler, size * sizeScaler, size * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
    }

    public float GetReferenceValue()
    {
        return lastMetrics["ShapeDiameterSizeController"];
    }

    public int GetSpriteIndex(string name)
    {
        name = name.ToLower();

        switch (name)
        {
            case "selected":
                return (int)CircleSprite.Selected;
            case "radius":
                return (int)CircleSprite.Radius;
            case "diameter":
                return (int)CircleSprite.Diameter;
            default:
                return (int)CircleSprite.Default;
        }
    }

    public enum CircleSprite
    {
        Default     = 0,
        Selected    = 1,
        Radius      = 2,
        Diameter    = 3
    } 
}
