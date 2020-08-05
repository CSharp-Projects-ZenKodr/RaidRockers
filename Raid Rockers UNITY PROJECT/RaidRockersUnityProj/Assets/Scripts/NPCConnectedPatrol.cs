using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NPCConnectedPatrol : MonoBehaviour {
    #region Variables
    /// <summary>
    /// Dictates whether the agent waits on each node.
    /// </summary>
    [SerializeField]
    bool patrolWaiting;
    /// <summary>
    /// The total time we wait at each node.
    /// </summary>
    [SerializeField]
    float totalWaitTime = 3f;
    /// <summary>
    /// The probability of switching direction.
    /// </summary>
    [SerializeField]
    float switchProbability = 0.2f;

    /// <summary>
    /// The Nav Mesh Agent componenet attached to this object.
    /// </summary>
    NavMeshAgent navMeshAgent;
    /// <summary>
    /// The current connected waypoint the NPC is on.
    /// </summary>
    ConnectedWaypoint currentWaypoint;
    /// <summary>
    /// The previous connected waypoint the NPC was at.
    /// </summary>
    ConnectedWaypoint previousWaypoint;
    /// <summary>
    /// Returns true if NPC is traveling, or false if not.
    /// </summary>
    bool traveling;
    /// <summary>
    /// Retturns true if NPC is waiting, or false if not.
    /// </summary>
    bool waiting;
    /// <summary>
    /// The timer that tells the NPC when to wait.
    /// </summary>
    float waitTimer;
    /// <summary>
    /// The waypoints that were visited.
    /// </summary>
    int waypointsVisited;
    #endregion

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + name, gameObject);
        }
        else
        {
            if (currentWaypoint == null)
            {
                //Set it at random
                //Grab all waypoint objects in scene.
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        //i.e. we found a waypoint.
                        if (startingWaypoint != null)
                        {
                            currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to find any waypoints for use in the scene");
                }
            }

            SetDestination();
        }
    }

    private void Update()
    {
        //Check if we're close to the destination
        if (traveling && navMeshAgent.remainingDistance <= 1.0f)
        {
            traveling = false;
            waypointsVisited++;

            //If we're going to wait, then wait
            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0;
            }
            else
            {
                SetDestination();
            }

            //Instead if we're waiting
            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= totalWaitTime)
                {
                    waiting = false;
                    
                    SetDestination();
                }
            }
        }
    }

    private void SetDestination()
    {
        if (waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = currentWaypoint.transform.position;
        navMeshAgent.SetDestination(targetVector);
        traveling = true;
    }
}