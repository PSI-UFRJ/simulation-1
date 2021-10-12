using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaling : MonoBehaviour
{

    public Vector3 scale;
    public Vector3 baseScale;
    public Rigidbody2D rb;

    public void ChangeScale(float newScale){
        //rb.freezeRotation = true;
        scale = baseScale + new Vector3(newScale*4, newScale*4, newScale*4);
        this.transform.localScale = scale;
        //rb.freezeRotation = false;
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
