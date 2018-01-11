using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;            // What to chase?
    public float updateRate = 2f;       // How many times each second we will update our path

    private Seeker seeker;              // Caching
    private Rigidbody2D rb;
    
    public Path path;                   //The calculated path
    public float speed = 1000f;         //The AI’s speed per second
    public ForceMode2D fMode;
    public bool pathIsEnded = false;

    public float spriteSize = 0.2f;
    
    public float nextWPDistance = 3;          // The max distance from the AI to a waypoint for it to continue to the next waypoint
    private int currentWP = 0;                // The waypoint we are currently moving towards
    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        StartCoroutine(UpdatePath());
    }
    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
        }
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }
        else
        {
            // Start a new path to the target position, return the result to the OnPathComplete method
            seeker.StartPath(transform.position, target.position, OnPathComplete);

            // Look at target 
            if (target.position.x > transform.position.x)
            {
                //face right
                transform.localScale = new Vector3(-spriteSize, spriteSize, spriteSize);
            }
            else if (target.position.x < transform.position.x)
            {
                //face left
                transform.localScale = new Vector3(spriteSize, spriteSize, spriteSize);
            }

            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }    
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWP = 0;
        }
    }
    void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null)
            return;
        if (currentWP >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;
            
            // When reach the end we gave it players new position.
            StartCoroutine(SearchForPlayer());
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWP] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce(dir, fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWP]);
        if (dist < nextWPDistance)
        {
            currentWP++;
            return;
        }
    }
}
