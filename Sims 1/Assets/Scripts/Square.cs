using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour, IShape
{
    private Color color;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    public List<GameObject> controllers;
    public Dictionary<string, GameObject> mappedControllers = new Dictionary<string, GameObject>();

    private Dictionary<string, float> lastMetrics = new Dictionary<string, float>() {
    };

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateArea(GameObject objectControlled)
    {
        float area = 0;
        return area;
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

        return new Dictionary<string, float>();
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
}
