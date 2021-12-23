using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diamond : MonoBehaviour, IShape
{
    private Color color;
    private PolygonCollider2D collider;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    public static float initialSizeSide             = 1;
    public static float initialAreaSide             = 0.87f;
    public static float initialLargerDiagonalSide   = 1.73f;
    public static float initialMinorDiagonalSide    = 1;
    public static float initialPerimeterSide        = 4;

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeSideSizeController", initialSizeSide },
        {"ShapeAreaSizeController", initialAreaSide },
        {"ShapeLargerDiagonalSizeController", initialLargerDiagonalSide },
        {"ShapeMinorDiagonalSizeController", initialMinorDiagonalSide },
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
        return (CalculateLargerDiagonal(objectControlled) * CalculateMinorDiagonal(objectControlled)) / 2;
    }

    private float GetLargerDiagonal(GameObject objectControlled)
    {
        Vector2[] points = objectControlled.GetComponent<PolygonCollider2D>().points; // Pega os pontos que foram o collider do polígono

        float diagonalOne = CalculateDistance(points, 0, 2);
        float diagonalTwo = CalculateDistance(points, 1, 3);

        return (diagonalOne >= diagonalTwo) ? diagonalOne : diagonalTwo;
    }

    private float GetMinorDiagonal(GameObject objectControlled)
    {
        Vector2[] points = objectControlled.GetComponent<PolygonCollider2D>().points; // Pega os pontos que foram o collider do polígono

        float diagonalOne = CalculateDistance(points, 0, 2);
        float diagonalTwo = CalculateDistance(points, 1, 3);

        return (diagonalOne <= diagonalTwo) ? diagonalOne : diagonalTwo;
    }

    private float CalculateDistance(Vector2[] points, int firstIndex, int secondIndex)
    {
        float distanceX = points[firstIndex].x - points[secondIndex].x;
        float distanceY = points[firstIndex].y - points[secondIndex].y;

        return (float)Math.Sqrt(Math.Abs(distanceX + distanceY));
    }

    public float CalculateLargerDiagonal(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x;
    }

    public float CalculateMinorDiagonal(GameObject objectControlled)
    {
        return (float)(objectControlled.transform.localScale.x * Math.Sqrt(3)) / 3;
    }

    public float CalculateSide(GameObject objectControlled)
    {
        return CalculateMinorDiagonal(objectControlled);
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        return 4 * CalculateMinorDiagonal(objectControlled);
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
            {"ShapeAreaSizeController", CalculateArea(objectControlled) },
            {"ShapeLargerDiagonalSizeController", CalculateLargerDiagonal(objectControlled) },
            {"ShapeMinorDiagonalSizeController", CalculateMinorDiagonal(objectControlled) },
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
            scale = new Vector3(size * sizeScaler * (float)Math.Sqrt(3), size * sizeScaler * (float)Math.Sqrt(3), size * sizeScaler * (float)Math.Sqrt(3)); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        if (slideName.IndexOf("area", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3((float)Math.Sqrt(size * sizeScaler * 2 * (float)Math.Sqrt(3)), (float)Math.Sqrt(size * sizeScaler * 2 * (float)Math.Sqrt(3)), (float)Math.Sqrt(size * sizeScaler * 2 * (float)Math.Sqrt(3))); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        if (slideName.IndexOf("largerdiagonal", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler, size * sizeScaler, size * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        if (slideName.IndexOf("minordiagonal", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler * (float)Math.Sqrt(3), size * sizeScaler * (float)Math.Sqrt(3), size * sizeScaler * (float)Math.Sqrt(3)); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        if (slideName.IndexOf("perimeter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler * (float)Math.Sqrt(3) / 4, size * sizeScaler * (float)Math.Sqrt(3) / 4, size * sizeScaler * (float)Math.Sqrt(3) / 4); // Gera a nova escala baseado na movimentação do slider (value)
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
                return (int)DiamondSprite.Selected;
            case "side":
                return (int)DiamondSprite.Side;
            case "largerdiagonal":
                return (int)DiamondSprite.LargerDiagonal;
            case "minordiagonal":
                return (int)DiamondSprite.MinorDiagonal;
            case "perimeter":
                return (int)DiamondSprite.Perimeter;
            case "area":
                return (int)DiamondSprite.Area;
            default:
                return (int)DiamondSprite.Default;
        }
    }

    public enum DiamondSprite
    {
        Default         = 0,
        Selected        = 1,
        Side            = 2,
        LargerDiagonal  = 3,
        MinorDiagonal   = 4,
        Perimeter       = 5,
        Area            = 6
    }
}
