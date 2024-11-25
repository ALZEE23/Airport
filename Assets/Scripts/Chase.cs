using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public GameObject cam1;
    public GameObject fire;
    public GameObject target;
    private NavMeshAgent agent;
    private bool stop;
    public bool showPath;
    public bool showAhead;
    public bool wait;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            agent.speed += 0.1f;
        }
        float distanceToTarget = (transform.position - target.transform.position).magnitude;

        if(wait==false)
        {
            StartCoroutine(go());
            IEnumerator go()
            {
                yield return new WaitForSeconds(6f);
                cam1.SetActive(false);
                wait = true;
            }
        }

        if(wait==true)
        {
        if (distanceToTarget < 2 && !stop)
        {
            stop = true;
            agent.SetDestination(transform.position);
            agent.speed = 0f;
            fire.SetActive(false);

            var win = FindObjectOfType<Continue>();
            win.WinCondition();
        }
        else
        {
            // Set destination untuk mengejar target
            agent.SetDestination(target.transform.position);

            // Jika agent bergerak, putar menghadap arah gerakan
            if (agent.velocity.sqrMagnitude > 0.01f) // Memastikan ada gerakan
            {
                // Mendapatkan arah berdasarkan velocity agent
                Vector2 direction = agent.velocity.normalized;

                // Menghitung rotasi yang diinginkan
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Set rotasi di sumbu Z (untuk 2D)
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        }
        

    }

    private void OnDrawGizmos()
    {
        Navigate.DrawGizmos(agent, showPath, showAhead);
    }
}
