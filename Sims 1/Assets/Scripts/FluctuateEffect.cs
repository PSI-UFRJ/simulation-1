using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluctuateEffect : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    [SerializeField]  private Vector2 direction;
    private Collider2D collider;
    private Collider2D topBoundCollider;
    private Collider2D bottomBoundCollider;
    private Collider2D leftBoundCollider;
    private Collider2D rightBoundCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        topBoundCollider = GameObject.Find("TopBound").GetComponent<BoxCollider2D>();
        bottomBoundCollider = GameObject.Find("BottomBound").GetComponent<BoxCollider2D>();
        leftBoundCollider = GameObject.Find("LeftBound").GetComponent<BoxCollider2D>();
        rightBoundCollider = GameObject.Find("RightBound").GetComponent<BoxCollider2D>();

        collider = this.gameObject.GetComponent<Collider2D>();
    }


    private void FixedUpdate()
    {
        Fluctuate();
    }

    public void Fluctuate()
    {
        rb.velocity = direction * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision == topBoundCollider) || (collision == bottomBoundCollider))
        {
            direction = new Vector2(direction.x, -direction.y);
        }
        else if ((collision == leftBoundCollider) || (collision == rightBoundCollider))
        {
            direction = new Vector2(-direction.x, direction.y);
        }
    }
}
