using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreetheGates : MonoBehaviour
{
    [SerializeField] GameObject thiss;
    [SerializeField] GameObject PS;
    [SerializeField] GameObject gates;
    FreetheGates gatesref;

    public int destroyGate = 0;
    
    void Start()
    {
        gatesref = gates.GetComponent<FreetheGates>();
    }

    // Update is called once per frame
    void Update()
    {
       if(destroyGate == 4)
        {
            Destroy(thiss);
            Instantiate(PS, transform.position, Quaternion.identity);
        }

       if(gatesref.destroyGate == 4)
        {
            Destroy(thiss);
            Instantiate(PS, transform.position, Quaternion.identity);
        }
    }
}
