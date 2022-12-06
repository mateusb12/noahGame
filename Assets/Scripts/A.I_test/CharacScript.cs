using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacScript : MonoBehaviour
{
    [SerializeField] private Animator animatorComponent;
    [SerializeField] GameObject cage;
    private Transform _characterTransform;
    private Rigidbody _characterRigidbody;
    
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode punchKey = KeyCode.P;
    public KeyCode kickKey = KeyCode.K;
    public KeyCode shootKey = KeyCode.J;

    public float inX;
    public float inZ;
    [FormerlySerializedAs("kicktime")] public float kickTime = 0;
    [FormerlySerializedAs("punchtime")] public float punchTime = 0;
    private Vector3 _verticalMovement;
    private Vector3 _verticalVelocity;
    public float moveSpeed;
    private int _tempo;

    private Health _healthComponent;

    [SerializeField] private float forceMagnitude;
    
    [FormerlySerializedAs("maincollider")] [FormerlySerializedAs("Maincollider")] [SerializeField] public CapsuleCollider mainCollider;
    [FormerlySerializedAs("pecollider")] [SerializeField] public Collider feetCollider;
    [FormerlySerializedAs("maocollider")] [SerializeField] public BoxCollider handCollider;
    [FormerlySerializedAs("maorig")] [SerializeField] public Rigidbody handRig;

    [FormerlySerializedAs("perna")] [SerializeField]
    private GameObject feet;
    [FormerlySerializedAs("mao")] [SerializeField]
    private GameObject hand;
    [FormerlySerializedAs("thisGuyrig")] [FormerlySerializedAs("ThisGuyrig")] [SerializeField]
    private GameObject thisGuyRig;
    [FormerlySerializedAs("bala")] [SerializeField]
    private GameObject bulletGameObject;
    [FormerlySerializedAs("Sword")] [SerializeField]
    private GameObject sword;
    [SerializeField] private GameObject ps;
    [FormerlySerializedAs("bulletpoint")] [SerializeField]
    private Transform bulletPoint;

    private float _hits = 0;

    private void Awake()
    {
        _healthComponent = GetComponent<Health>();
    }

    private void Start()
    {
        Getragdoolbits();
        RagdollOff();
        GameObject playeri = GameObject.FindGameObjectWithTag("Player");
       // _charController = Playeri.GetComponent<CharacterController>();
        _characterTransform = playeri.GetComponent<Transform>();
        animatorComponent = playeri.GetComponent<Animator>();
        _characterRigidbody = playeri.GetComponent<Rigidbody>();
        moveSpeed = 4f;
        animatorComponent.SetBool("kick", false);
        animatorComponent.SetBool("kick2", true);
    }

    private Rigidbody[] _ragrigid;
    private Collider[] _ragcollider;

    private void Getragdoolbits()
    {
        _ragrigid = thisGuyRig.GetComponentsInChildren<Rigidbody>();
        _ragcollider = thisGuyRig.GetComponentsInChildren<Collider>();
    }

    private void RagdollOff()
    {
        foreach (Collider col in _ragcollider)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rag in _ragrigid)
        {
            rag.isKinematic = true;
        }
    
        animatorComponent.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        handRig.isKinematic = false;
    }

    private void PeEnable()
    {
       // pecollider.enabled = true;
        feet.tag = "Golpe";
        feetCollider.enabled = true;
    }

    private void PeDisable()
    {
        feetCollider.enabled = false;
        feet.tag = "Untagged";
    }

    public void MaoEnable()
    {
        hand.tag = "Golpe";
        handCollider.enabled = true;
      
    }

    public void MaoDisable()
    {
        hand.tag = "Untagged";
        handCollider.enabled = false;

    }

    private void SwordEnable()
    {
        handCollider.center = new Vector3(-2.5f, 0, 0);
        handCollider.size = new Vector3(6, 0.5f, 1);
        sword.SetActive(true);
    }

    private void RagdollOn()
    {
        animatorComponent.enabled = false;

        foreach (Collider col in _ragcollider)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rag in _ragrigid)
        {
            rag.isKinematic = false;
        }

        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        _hits = 0;
    }
       private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Rosto_AI")
        {
            RagdollOn();
           // Instantiate(ps, characPosition, characRotation);
           // Debug.Log("pow");
        }
        if (collision.gameObject.tag == "balah")
        {
            MaoDisable();
            PeDisable();
            // _hits++;
            animatorComponent.SetBool("hit", true);
            Invoke("Hitfalse", 0.2f);
            _healthComponent.TakeDamage(gameObject, 10f);
            // Instantiate(ps, characPosition, characRotation);
            // Debug.Log("pow");
        }
        else
        {
           // Maincollider.enabled = true;
           // Invoke("disablebox", 0.7f);
        }

    }

       private void Hitfalse()
    {
        animatorComponent.SetBool("hit", false);
    }

       private void Disablebox()
    {
       // boxcollider.enabled = false;
    }
    // Update is called once per frame
    private void Update()
    {
       // maocollider.enabled = true;

       if (Input.GetKey(downKey))
        {
            SwordEnable();
            handCollider.enabled = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            RagdollOff();
        }

        inX = Input.GetAxis("Horizontal");
        inZ = Input.GetAxis("Vertical");

        Maist();

        //CORRER
        if (Input.GetKey(runKey))
        {
            animatorComponent.SetBool("run", true);
            animatorComponent.SetBool("run2", false);
            kickTime = 0;
           // inX = inX * 100;

            // Debug.Log("colon");
        }
        if (Input.GetKeyUp(runKey))
        {
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);

        }
        //CHUTE
        if (Input.GetKey(kickKey))
        {
           // inX = inX * 100;
        }
        if (Input.GetKeyDown(kickKey))
        {
            kickTime = 100;
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);
            animatorComponent.SetBool("kick", true);
            animatorComponent.SetBool("lbool", false);
            animatorComponent.SetBool("rbool", false);
        }
        else
        {
            kickTime = kickTime - 5f;
            //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
        }

        if (Input.GetKeyUp(kickKey))
        {
            animatorComponent.SetBool("kick", false);
        }

        if (kickTime < 0)
        {
            kickTime = 0;
        }
        if (kickTime > 20 )
        {
           // Invoke("PeEnable", 0.8f);
           // pecollider.enabled = true;
           // perna.tag = "balah";
        }
        else
        {
           // perna.tag = "Untagged";
        }

        //PUNCH
        if (Input.GetKeyDown(punchKey))
        {
            punchTime = 100;
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);
            animatorComponent.SetBool("punch", true);
            animatorComponent.SetBool("lbool", false);
            animatorComponent.SetBool("rbool", false);
            handRig.isKinematic = false;
           // inX = inX * 100;

        }
        else
        {
            punchTime = punchTime - 2f;
            //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(cage);
        }
        if (Input.GetKeyUp(punchKey))
        {
            animatorComponent.SetBool("punch", false);

        }

        if (punchTime < 0)
        {
            punchTime = 0;
        }
        if (punchTime > 5)
        {
            //  Invoke("maoEnable", 0.4f);
            //  maocollider.enabled = true;
            // perna.tag = "balah";
            handRig.isKinematic = false;

        }
        else
        {
            //mao.tag = "Untagged";
            handCollider.enabled = false;
        }

        //WALK
        if (Input.GetKey(upKey))
        {
            animatorComponent.SetBool("bool1", true);
            animatorComponent.SetBool("bool2", true);
           // inX = inX * 100;
           // tempo = 0;
        }

        if (Input.GetKeyUp(upKey)) 
        {

            animatorComponent.SetBool("bool1", false);
            animatorComponent.SetBool("bool2", false);
            _tempo = 0;
        }
        if (Input.GetKey(leftKey))
        {
            animatorComponent.SetBool("lbool", true);
            animatorComponent.SetBool("lbool2", false);
            inX = -inX * -100;
        }
        else 
        {
            animatorComponent.SetBool("lbool", false);
            animatorComponent.SetBool("lbool2", true);
        }
        if (Input.GetKey(rightKey))
        {
            animatorComponent.SetBool("rbool", true);
            animatorComponent.SetBool("rbool2", false);
            inX = inX * 100;
        }
        else
        {
            animatorComponent.SetBool("rbool", false);
            animatorComponent.SetBool("rbool2", true);  
        }
        if (_tempo > 160)
        {
           // anim.SetBool("run", true);
           // anim.SetBool("run2", false);
        }

        //AGACHAR
        if (Input.GetKeyDown(downKey))
        {
            animatorComponent.SetBool("Agacha", true);
            // Maincollider.enabled = false;
            mainCollider.height = 6;
            mainCollider.center = new Vector3(0, 3, 0);
        }
        if (Input.GetKeyUp(downKey))
        {
            animatorComponent.SetBool("Agacha", false);
           // Maincollider.enabled = true;
            mainCollider.height = 13;
            mainCollider.center = new Vector3(0, 7, 0);
        }
        if (Input.GetKey(downKey))
        {
           // inX = inX * 100;
        }
        else
        {
            animatorComponent.SetBool("Agacha", false);
        }

        //ATIRAR
        if (Input.GetKeyDown(shootKey))
        {
            animatorComponent.SetBool("run", false);
            animatorComponent.SetBool("run2", true);
            animatorComponent.SetBool("shoot", true);
            animatorComponent.SetBool("bool1", false);
            animatorComponent.SetBool("bool2", false);

            Invoke("Atira", 0.3f);
        }

        if (Input.GetKeyUp(shootKey))
        {
            animatorComponent.SetBool("shoot", false);

        }
        if (Input.GetKey(shootKey))
        {
               // inX = inX * 100;     
        }

        //Debug.Log(inX);
        if(inX >100 || inX < -100)
        {
            inX = 100;
        }

    }

    public void DeathMechanics()
    {
        if (_healthComponent.IsDead())
        {
            RagdollOn();
        }
    }

    private void Atira()
    {
        if (animatorComponent.GetBool("shoot") == true)
        { 
            Rigidbody rb = Instantiate(bulletGameObject, bulletPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 4f, ForceMode.Impulse);
            rb.AddForce(transform.up * -0.1f, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {

        _verticalMovement = _characterTransform.transform.forward * inZ *(1.5f * Time.deltaTime);
        
        _characterTransform.transform.Rotate(Vector3.up * inX * (1.5f * Time.deltaTime));

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.M))
            {
                _characterRigidbody.AddForce(_verticalMovement * moveSpeed * Time.deltaTime);
            _tempo = 0;

        }

    }

    private void Maist()
    {
        _tempo++;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody !=null 
          //  & Input.GetKey(KeyCode.K)//pra so ativar se ele chutar
            
            )
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }

    }

}
