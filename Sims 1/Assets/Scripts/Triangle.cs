using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Triangle : MonoBehaviour, IShape
{
    private Color color;
    private PolygonCollider2D collider;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    public static float initialSizeSide      = 1;
    public static float initialHeightSide    = 1;
    public static float initialAreaSide      = 1;
    public static float initialPerimeterSide = 1;

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeSideSizeController", initialSizeSide },
        {"ShapeHeightSizeController", initialHeightSide },
        {"ShapeAreaSizeController", initialAreaSide },
        {"ShapePerimeterSizeController", initialPerimeterSide }
    };

    // Start is called before the first frame update
    void Start()
    {
        collider = this.gameObject.GetComponent<PolygonCollider2D>();
        transform = this.gameObject.GetComponent<Transform>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

        if (controllers != null)
        {
            controllersListToDict(mappedControllers, controllers);
        }
    }

    public float CalculateArea(GameObject objectControlled)
    {
        return (objectControlled.transform.localScale.x * objectControlled.transform.localScale.x) * (Mathf.Sqrt(3)/4);
    }

    public float CalculateHeight(GameObject objectControlled)
    {
        return (objectControlled.transform.localScale.x * Mathf.Sqrt(3))/2;
    }

    public float CalculateSide(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x;
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        return 3 * objectControlled.transform.localScale.x;
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
            {"ShapeSideSizeController",  CalculateSide(objectControlled)},
            {"ShapeHeightSizeController", CalculateHeight(objectControlled) },
            {"ShapeAreaSizeController", CalculateArea(objectControlled) },
            {"ShapePerimeterSizeController", CalculatePerimeter(objectControlled) }
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
        if (slideName.IndexOf("side", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler, size * sizeScaler, size * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("height", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler * (2/Mathf.Sqrt(3)), size * sizeScaler * (2 / Mathf.Sqrt(3)), size * sizeScaler * (2 / Mathf.Sqrt(3))); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("perimeter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler / 3, size * sizeScaler / 3, size * sizeScaler / 3); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("area", StringComparison.OrdinalIgnoreCase) != -1)
        {
            double insideSqrt = (size * sizeScaler * 4) / (Math.Sqrt(3));
            scale = new Vector3((float)Math.Sqrt(insideSqrt), (float)Math.Sqrt(insideSqrt), (float)Math.Sqrt(insideSqrt)); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
    }

    public float GetReferenceValue()
    {
        return 1;
    }

    public int GetSpriteIndex(string name)
    {
        name = name.ToLower();

        switch (name)
        {
            case "selected":
                return (int)TriangleSprite.Selected;
            case "side":
                return (int)TriangleSprite.Side;
            case "height":
                return (int)TriangleSprite.Height;
            case "perimeter":
                return (int)TriangleSprite.Perimeter;
            case "area":
                return (int)TriangleSprite.Area;
            default:
                return (int)TriangleSprite.Default;
        }
    }

    public enum TriangleSprite
    {
        Default     = 0,
        Selected    = 1,
        Side        = 2,
        Height      = 3,
        Perimeter   = 4,
        Area        = 5
    }
}
