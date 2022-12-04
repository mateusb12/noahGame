using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOprimido : MonoBehaviour
{
    public Animator anim;
    [SerializeField] GameObject gates;
    FreetheGates gatesref;
    // Start is called before the first frame update
    void Start()
    {
        gatesref = gates.GetComponent<FreetheGates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gatesref.destroyGate == 4)
        {
            anim.SetBool("Victory", true);
        }
    }
}
