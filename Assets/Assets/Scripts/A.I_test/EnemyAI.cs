using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Projectil;
    public GameObject thisFace;
    public GameObject patriclesystem;
    public Transform player;
    private MeshRenderer renderer;
    

    [Range(0, 8)] public float lerpTime;
    public Color color;
    private Color corIncial = new Color32(156, 248, 206, 255);
    bool Lerpbool = false;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkpoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyattacked;


    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    float hits = 0;


    private void Awake()
    {
        player = GameObject.Find("Noah").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Lerpbool == true)
        {
            LerpRed();
        }
        if (Lerpbool == false)
        {
            LerpBlue();
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    private void Patroling()
    {
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
    }
    private void SearchWalkPoint()
    {
       
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //Debug.Log("voidSearchWalkpoint");
        //nao parece ser nada desse void

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
           // Debug.Log("ifSearchWalkpoint");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Golpe")
        {
            transform.localScale -= new Vector3(0.002f, 0.002f, 0.002f);

            Lerpbool = true;
            hits++;
        }

        if (hits > 10)
        {
            Destroy(thisFace);
            Instantiate(patriclesystem, transform.position, Quaternion.identity);
        }
    }

    void LerpBoolFalse()
    {
        Lerpbool = false;
    }
    void LerpRed()
    {
        renderer.material.color = Color.Lerp(renderer.material.color, color, lerpTime * Time.deltaTime);
        Invoke("LerpBoolFalse", 0.3f);
    }
    void LerpBlue()
    {
        renderer.material.color = Color.Lerp(renderer.material.color, corIncial, lerpTime * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);

       // transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);

        agent.SetDestination(player.transform.position / 2);

        //a treta é com a rotação, verificar o q esta mechendo com a rotação
        //possivel conflito entre "LookAt & SetDestination"
        transform.LookAt(player);

        if (!alreadyattacked)
        {
            Rigidbody rb = Instantiate(Projectil, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 4f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.4f, ForceMode.Impulse);


            alreadyattacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyattacked = false;
    }
}
