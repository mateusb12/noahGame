using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float life = 4;
    [SerializeField] GameObject bala;
    [SerializeField] GameObject balaexp;

    public int timerr;

    public Vector3 pos = new Vector3(0, 0, 0);
    private void Awake()
    {
        Destroy(bala, life);
       // Destroy(balaexp, life);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("destroyer", 0.6f);
       // Instantiate(balaexp, collision.transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

        timerr++;

    }
    void destroyer()
    {
        Destroy(bala);
    }
}
