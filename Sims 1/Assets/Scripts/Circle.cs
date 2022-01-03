using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour, IShape
{
    // Start is called before the first frame update
    private Color color;
    private CircleCollider2D collider;
    private Transform transform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Vector3 scale; // Guarda a escala atual
    public int sizeScaler = 1;

    public static float initialSizeRadius = 0.5f;
    public static float initialSizeDiameter = 1;
    public static float initialSizePerimeter = 3.14f;
    public static float initialSizeArea = 0.78f;

    public List<GameObject> controllers = null;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private List<string> curiosityCollection = new List<string>() 
    {
        "Círculos são ditos congruentes quando os mesmos possuem raios iguais",
        "O diâmetro de um círculo é o maior segmento de reta entre dois pontos do círculo",
        "Um círculo pode ser circunscrito em um retângulo, trapézio, losango, triângulo",
        "Um círculo pode ser inscrito dentro de um quadrado, triângulo e losango",
        "Todos os pontos da circunferência apresentam a mesma distância em relação ao centro"

    } ; 

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeRadiusSizeController", initialSizeRadius },
        {"ShapeDiameterSizeController", initialSizeDiameter },
        {"ShapePerimeterSizeController", initialSizePerimeter },
        {"ShapeAreaSizeController", initialSizeArea }
    };

    void Start()
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();
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
        float relativeRadius = collider.radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala
        return Mathf.PI * relativeRadius * relativeRadius;
    }

    public float CalculateRadius(GameObject objectControlled)
    {
        return collider.radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala;
    }

    public float CalculatePerimeter(GameObject objectControlled)
    {
        float relativeRadius = collider.radius * objectControlled.transform.localScale.x; // Calcula o raio baseado na escala
        return 2 * Mathf.PI * relativeRadius;
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
            {"ShapeRadiusSizeController",  CalculateRadius(objectControlled) },
            {"ShapeDiameterSizeController",  CalculateDiameter(objectControlled) },
            {"ShapePerimeterSizeController",  CalculatePerimeter(objectControlled) },
            {"ShapeAreaSizeController",  CalculateArea(objectControlled) }
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
        else if (slideName.IndexOf("perimeter", StringComparison.OrdinalIgnoreCase) != -1)
        {
            scale = new Vector3(size * sizeScaler * (1/(float)Math.PI), size * sizeScaler * (1 / (float)Math.PI), size * sizeScaler * (1 / (float)Math.PI)); // Gera a nova escala baseado na movimentação do slider (value)
            objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
        }
        else if (slideName.IndexOf("area", StringComparison.OrdinalIgnoreCase) != -1)
        {
            double insideSqrt = (size * sizeScaler * 4) / (Math.PI);
            scale = new Vector3((float)Math.Sqrt(insideSqrt), (float)Math.Sqrt(insideSqrt), (float)Math.Sqrt(insideSqrt)); // Gera a nova escala baseado na movimentação do slider (value)
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
            case "perimeter":
                return (int)CircleSprite.Perimeter;
            case "area":
                return (int)CircleSprite.Area;
            default:
                return (int)CircleSprite.Default;
        }
    }

    public string GetCuriosity()
    {
        System.Random r = new System.Random();

        int index = r.Next(curiosityCollection.Count);

        return curiosityCollection[index];
    }

    public enum CircleSprite
    {
        Default     = 0,
        Selected    = 1,
        Radius      = 2,
        Diameter    = 3,
        Perimeter   = 4,
        Area        = 5
    } 
}
