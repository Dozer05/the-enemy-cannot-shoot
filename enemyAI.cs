using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform Player;
    public GameObject EnemyProjectile;
    public float launchVelocity = 700f;
    float nextFire = 0;
    public float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        agent.destination = Player.position;
        if (distance <= 10f)
        {
            agent.isStopped = true;
            Attack();
        }
        else
        {
            agent.isStopped = false;
            agent.destination = Player.position;
        }
        FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Attack()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(EnemyProjectile, transform.position, transform.rotation);

            Quaternion _lookRotation =
            Quaternion.LookRotation((Player.position - bullet.transform.position).normalized);

            //over time
            transform.rotation =
                Quaternion.Slerp(bullet.transform.rotation, _lookRotation, Time.deltaTime * 0.8f);

            //instant
            bullet.transform.rotation = _lookRotation;

            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * launchVelocity);
            Destroy(bullet, 2f);
        }
    }
}
