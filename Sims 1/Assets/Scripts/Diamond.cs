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

    public static float initialSizeSide = 1;

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>()
    {
        {"ShapeSideSizeController", initialSizeSide }
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
        float area = 0;
        return area;
    }

    public float CalculateSide(GameObject objectControlled)
    {
        return objectControlled.transform.localScale.x;
    }


    public float CalculatePerimeter(GameObject objectControlled)
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

    public Dictionary<string, float> GetMetrics(GameObject objectControlled)
    {
        return new Dictionary<string, float>()  {
            {"ShapeSideSizeController",  CalculateSide(objectControlled)},
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
        scale = new Vector3(size * sizeScaler, size * sizeScaler, size * sizeScaler); // Gera a nova escala baseado na movimentação do slider (value)
        objectControlled.transform.localScale = scale; // Muda a escala local do objeto controlado
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
        MinorDiagonal   = 4
    }
}
