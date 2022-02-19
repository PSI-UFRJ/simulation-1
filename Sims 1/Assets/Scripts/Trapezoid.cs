using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trapezoid : MonoBehaviour, IShape
{
    private Color color;
    private PolygonCollider2D collider;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    public static float initialLargerBaseSize   = 1;
    public static float initialLateralSide      = 0.82f;
    public static float initialMinorBaseSize    = 0.53f;
    public static float initialHeightSize       = 0.78f;
    public static float initialPerimeterSize    = 3.17f;
    public static float initialAreaSize         = 0.6f; 

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private List<string> curiosityCollection = new List<string>()
    {
        "O tamanho das duas diagonais são iguais",
        "Ângulos interiores que são adjacentes somam 180°",
        "A palavra trapézio vem do grego 'trapézion' que significa 'uma pequena mesa'. Também se refere a quadrilátero irregular.",
        "Ao rotacionar um trapézio isósceles em torno do seu eixo vertical que o divide em partes iguais gera-se um tronco de cone no 3D"
    };

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeLargerBaseSizeController", initialLargerBaseSize },
        {"ShapeMinorBaseSizeController", initialMinorBaseSize },
        {"ShapeHeightSizeController", initialHeightSize },
        {"ShapeAreaSizeController", initialAreaSize },
        {"ShapePerimeterSizeController", initialPerimeterSize }
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
        return (CalculateMinorBase(objectControlled) + CalculateLargerBase(objectControlled)) * CalculateHeightBase(objectControlled) / 2;
    }

    public float CalculateMinorBase(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x * initialMinorBaseSize;
    }

    public float CalculateLargerBase(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x;
    }

    public float CalculateHeightBase(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x * initialHeightSize;
    }

    public float CalculateLateralSide(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x * initialLateralSide;
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        return CalculateMinorBase(objectControlled) + CalculateLargerBase(objectControlled) + (2 * CalculateLateralSide(objectControlled));
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
            {"ShapeLargerBaseSizeController", CalculateLargerBase(objectControlled) },
            {"ShapeMinorBaseSizeController", CalculateMinorBase(objectControlled) },
            {"ShapeHeightSizeController", CalculateHeightBase(objectControlled) },
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
        if (slideName.IndexOf("largerbase", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler, size * sizeScaler, size * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("minorbase", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler / initialMinorBaseSize, size * sizeScaler / initialMinorBaseSize, size * sizeScaler / initialMinorBaseSize); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("height", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler / initialHeightSize, size * sizeScaler / initialHeightSize, size * sizeScaler / initialHeightSize); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("area", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3((float)Math.Sqrt((2 * size * sizeScaler) / (initialHeightSize * (1 + initialMinorBaseSize))), (float)Math.Sqrt((2 * size * sizeScaler) / (initialHeightSize * (1 + initialMinorBaseSize))), (float)Math.Sqrt((2 * size * sizeScaler) / (initialHeightSize * (1 + initialMinorBaseSize)))); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("perimeter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler / initialPerimeterSize, size * sizeScaler / initialPerimeterSize, size * sizeScaler / initialPerimeterSize); // Gera a nova escala baseado na movimentação do slider (value)
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
                return (int)TrapezoidSprite.Selected;
            case "height":
                return (int)TrapezoidSprite.Height;
            case "minorbase":
                return (int)TrapezoidSprite.MinorBase;
            case "largerbase":
                return (int)TrapezoidSprite.LargerBase;
            case "perimeter":
                return (int)TrapezoidSprite.Perimeter;
            case "area":
                return (int)TrapezoidSprite.Area;
            default:
                return (int)TrapezoidSprite.Default;
        }
    }

    public string GetCuriosity()
    {
        System.Random r = new System.Random();

        int index = r.Next(curiosityCollection.Count);

        return curiosityCollection[index];
    }

    public enum TrapezoidSprite
    {
        Default     = 0,
        Selected    = 1,
        Height      = 2,
        MinorBase   = 3,
        LargerBase  = 4,
        Perimeter   = 5,
        Area        = 6

    }
}
