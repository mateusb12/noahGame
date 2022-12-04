using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golpeador : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Projectil;
    public Transform player;
    [SerializeField] Animator Ani;
    [SerializeField] Transform bulletpoint;
    [SerializeField] GameObject esse;
    [SerializeField] GameObject Particles;
    [SerializeField] GameObject arm;
    [SerializeField] Collider arma;
    [SerializeField] Rigidbody armarig;
    public Collision collision3;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkpoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    float hits = 0;
    float randomX, randomZ;
    bool alreadyattacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool armable;

    WallCollision wallcol2;
    [SerializeField] GameObject golpeador;

    FreetheGates gatesref;
    [SerializeField] GameObject gates;
    private void Awake()
    {
        player = GameObject.Find("Noah").transform;
        agent = GetComponent<NavMeshAgent>();

        Ani.SetBool("Walk2", true);

        wallcol2 = golpeador.GetComponent<WallCollision>();
        gatesref = gates.GetComponent<FreetheGates>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

       // if(wallcol2.wallcol == false)
       // {
       //     Debug.Log("walcol");
       // }

    }

    void aramableOn()
    {
        armable = true;
        arma.enabled = true;
        arm.tag = "balah";
        // armarig.isKinematic = true;
    }
    void aramableOff()
    {
        armable = false;
        arma.enabled = false;
        arm.tag = "Untagged";
        // armarig.isKinematic = false;
    }
    void recover()
    {
        arm.tag = "balah";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Golpe")
        {

            arm.tag = "Untagged";
            aramableOff();
            Ani.SetBool("shooting2", false);
            Ani.SetBool("Walk2", false);
            Ani.SetBool("defeat", true);
            alreadyattacked = false;
            hits++;
            Ani.SetBool("shooting2", false);


            if (hits > 3)
            {
                aramableOff();
                arm.tag = "Untagged";
                Invoke("Ps", 1.3f);
                Destroy(esse, 1.5f);
                armable = false;
                arma.enabled = false;
            }
            else
            {
                Invoke("recover", 0.4f);
            }

        }
    }

    void hitanimfalse()
    {
        Ani.SetBool("defeat", false);
    }
    private void Ps()
    {
        Instantiate(Particles, transform.position, Quaternion.identity);
        gatesref.destroyGate++;
    }

    private void Patroling()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        // Debug.Log("patrol");
        arma.enabled = false;
       // armarig.isKinematic = false;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        if (wallcol2.wallcol == true)
        {
            Debug.Log("FOIIIIII");
            walkPointSet = false;
            wallcol2.wallcol = false;
        }

    }
    private void SearchWalkPoint()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        arma.enabled = false;

        randomZ = Random.Range(-walkPointRange, walkPointRange);
        randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
       // Debug.Log("voidSearchWalkpoint");
       

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
           // Debug.Log("ifSearchWalkpoint");
        }
    }

    private void ChasePlayer()
    {
       
         Ani.SetBool("Walk2", true);
        Ani.SetBool("shooting2", false);

        agent.SetDestination(player.transform.position);

        arma.enabled = false;
       // armarig.isKinematic = false;
        // transform.LookAt(player);
    }
    private void AttackPlayer()
    {
 
        Ani.SetBool("shooting2", true);

       // agent.SetDestination(player.transform.position );

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!alreadyattacked)
        {
            Rigidbody rb = Instantiate(Projectil, bulletpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            rb.AddForce(transform.up * -0.5f, ForceMode.Impulse);


            alreadyattacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
      
    }

    private void ResetAttack()
    {
        Ani.SetBool("shooting2", false);
        alreadyattacked = false;
    }
}


