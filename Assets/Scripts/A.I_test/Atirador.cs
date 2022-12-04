using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Atirador : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Projectil;
    public Transform player;
   // [SerializeField] Rigidbody thisRB;
    [SerializeField] Animator Ani;
    [SerializeField] Transform bulletpoint;
    [SerializeField] GameObject esse;
    [SerializeField] GameObject Particles;


    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkpoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyattacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    WallCollision wallcol2;
    [SerializeField] GameObject atirador;

    RagdollEvil ragscript;

    FreetheGates gatesref;
    [SerializeField] GameObject gates;

    private void Awake()
    {
        player = GameObject.Find("Noah").transform;
        agent = GetComponent<NavMeshAgent>();

        Ani.SetBool("Walk2", true);
        wallcol2 = atirador.GetComponent<WallCollision>();
        ragscript = atirador.GetComponent<RagdollEvil>();
        gatesref = gates.GetComponent<FreetheGates>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Golpe")
        {
            Ani.SetBool("shooting2", false);
            Ani.SetBool("Walk2", false);
           // Ani.SetBool("defeat", true);

            //thisRB.AddForce(transform.forward * -9f, ForceMode.Impulse);
            Invoke("Ps", 1.3f);
            Destroy(esse,1.5f);
            gatesref.destroyGate++;
            ragscript.AtiradorragOn = true;
        }
    }

    private void Ps()
    {
        Instantiate(Particles, transform.position, Quaternion.identity);
    }

    private void Patroling()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        // Debug.Log("patrol");

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
            Debug.Log("FOI");
            walkPointSet = false;
            wallcol2.wallcol = false;
        }
    }
    private void SearchWalkPoint()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
       // Debug.Log("voidSearchWalkpoint");
        //nao parece ser nada desse void

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
           // Debug.Log("ifSearchWalkpoint");
        }
    }

    private void ChasePlayer()
    {
        Ani.SetBool("shooting2", true);
       // Ani.SetBool("Walk2", true);
        //Ani.SetBool("shooting2", true);

        agent.SetDestination(player.transform.position);

        
        // transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        Ani.SetBool("shooting2", true);

       // agent.SetDestination(player.transform.position / 2);

        //a treta é com a rotação, verificar o q esta mechendo com a rotação
        //possivel conflito entre "LookAt & SetDestination"
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!alreadyattacked)
        {
            Rigidbody rb = Instantiate(Projectil, bulletpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            rb.AddForce(transform.up * -0.5f, ForceMode.Impulse);


            alreadyattacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        //Ani.SetBool("shooting2", false);
    }

    private void ResetAttack()
    {
        //Ani.SetBool("shooting2", false);
        alreadyattacked = false;
    }
}

