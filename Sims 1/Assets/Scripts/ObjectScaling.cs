using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaling : MonoBehaviour
{

    public Vector3 scale;
    public Vector3 baseScale;
    public Rigidbody2D rb;
    public int sizeScaler = 1;

    public void ChangeScale(float newScale){
        scale = baseScale + new Vector3(newScale * sizeScaler, newScale * sizeScaler, newScale * sizeScaler);
        this.transform.localScale = scale;
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
