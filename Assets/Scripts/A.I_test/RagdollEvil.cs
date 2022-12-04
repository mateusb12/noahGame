using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEvil : MonoBehaviour
{
    [SerializeField] private Collider mainCollider2;
    [SerializeField] private Collider sword;
    [SerializeField] private GameObject ThisGuysRig2;
    [SerializeField] private Animator ThisGuysAnimator2;
    [SerializeField] GameObject rosto;
    public float tempo = 1;
    public bool active = false;
    public bool AtiradorragOn = false;
    float hits = 0;

    void Start()
    {
        GetRagdollBits2();
        RagdollModeOff2();
    }

    // Update is called once per frame
    void Update()
    {
        if (hits > 3)
        {
            RagdollModeOn2();
        }
        if(active == true)
        {
            timer();
        }
        if (tempo > 150)
        {
            RagdollModeOff2();
            active = false;
        }
        if(AtiradorragOn == true)
        {
            RagdollModeOn2();
        }
    }

    void timer()
    {
        tempo++;
    }

    void recover()
    {
        ThisGuysAnimator2.SetBool("defeat", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Golpe")
        {
           // if (Input.GetKey(KeyCode.K))
          //  {
               // Debug.Log("collide");
                hits++;
                ThisGuysAnimator2.SetBool("defeat", true);
                Invoke("recover", 0.2f);
            // RagdollModeOn2();
            // }
        }
    }
    Collider[] ragDollColliders2;
    Rigidbody[] limbsRigidbodies2;
    void GetRagdollBits2()
    {
        ragDollColliders2 = ThisGuysRig2.GetComponentsInChildren<Collider>();
        limbsRigidbodies2 = ThisGuysRig2.GetComponents<Rigidbody>();
    }
    void RagdollModeOn2()
    {
        tempo = 0;
        active = true;
        ThisGuysAnimator2.enabled = false;
        

        foreach (Collider col in ragDollColliders2)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rigid in limbsRigidbodies2)
        {
            rigid.isKinematic = false;
        }

        sword.enabled = false;
        mainCollider2.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void RagdollModeOff2()
    {
        
        foreach (Collider col in ragDollColliders2)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rigid in limbsRigidbodies2)
        {
            rigid.isKinematic = true;
        }

       // sword.enabled = true;
        ThisGuysAnimator2.enabled = true;
        mainCollider2.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }


}
