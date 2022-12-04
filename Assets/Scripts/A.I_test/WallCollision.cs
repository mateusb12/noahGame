using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    
    public bool wallcol;

    void Start()
    {
       
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision3)
    {
        if(collision3.gameObject.tag == "Cenario")
        {
           // Debug.Log("COL");
            
            wallcol = true;
        }
        else
        {
            wallcol = false;
        }
    }
}
