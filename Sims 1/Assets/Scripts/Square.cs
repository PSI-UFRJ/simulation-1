using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour, IShape
{
    private Color color;
    private BoxCollider2D collider;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    public static float initialSizeSide = 1;
    public static float initialSizeDiagonal = 1.41f;
    public static float initialSizePerimeter = 4f;
    public static float initialSizeArea = 1f;

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private List<string> curiosityCollection = new List<string>()
    {
        "Todo quadrado é um losango mas nem todo losango é um quadrado.",
        "A relação de proporção entre o perímetro e a área é quadrática: a área é igual a 1/16 do quadrado do perímetro.",
        "Um quadrado tem uma área maior que qualquer outro quadrilátero de mesmo perímetro.",
        "Existem 4 formas de dividir um quadrado em partes iguais."
    };

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeSideSizeController", initialSizeSide },
        {"ShapeDiagonalSizeController", initialSizeDiagonal },
        {"ShapePerimeterSizeController", initialSizePerimeter },
        {"ShapeAreaSizeController", initialSizeArea }
    };

    // Start is called before the first frame update
    void Start()
    {
        collider = this.gameObject.GetComponent<BoxCollider2D>();
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
        return (float)Math.Pow(objectControlled.transform.localScale.x, 2);
    }

    public float CalculateSide(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x;
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        return 4 * objectControlled.transform.localScale.x;
    }

    public float CalculateDiagonal(GameObject objectControlled)
    {
        return (float)(objectControlled.transform.localScale.x * Math.Sqrt(2)); 
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
            {"ShapeDiagonalSizeController", CalculateDiagonal(objectControlled) },
            {"ShapePerimeterSizeController", CalculatePerimeter(objectControlled) },
            {"ShapeAreaSizeController", CalculateArea(objectControlled) }
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
        else if(slideName.IndexOf("diagonal", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3((float)(size * sizeScaler/(Math.Sqrt(2))), (float)(size * sizeScaler / (Math.Sqrt(2))), (float)(size * sizeScaler / (Math.Sqrt(2)))); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("perimeter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3((size * sizeScaler) / 4, (size * sizeScaler) / 4, (size * sizeScaler) / 4); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("area", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3((float)Math.Sqrt(size * sizeScaler), (float)Math.Sqrt(size * sizeScaler), (float)Math.Sqrt(size * sizeScaler)); // Gera a nova escala baseado na movimentação do slider (value)
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
                return (int)SquareSprite.Selected;
            case "side":
                return (int)SquareSprite.Side;
            case "diagonal":
                return (int)SquareSprite.Diagonal;
            case "perimeter":
                return (int)SquareSprite.Perimeter;
            case "area":
                return (int)SquareSprite.Area;
            default:
                return (int)SquareSprite.Default;
        }
    }

    public string GetCuriosity()
    {
        System.Random r = new System.Random();

        int index = r.Next(curiosityCollection.Count);

        return curiosityCollection[index];
    }

    public enum SquareSprite
    {
        Default     = 0,
        Selected    = 1,
        Side        = 2,
        Diagonal    = 3,
        Perimeter   = 4,
        Area        = 5
    }
}
