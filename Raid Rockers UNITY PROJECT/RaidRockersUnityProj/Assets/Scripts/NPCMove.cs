using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
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
    /// The list of all patrol nodes to visit.
    /// </summary>
    [SerializeField]
    List<WayPoint> patrolPoints;

    /// <summary>
    /// The Nav Mesh Agent componenet attached to this object.
    /// </summary>
    NavMeshAgent navMeshAgent;
    /// <summary>
    /// The current patrol index our NPC is heading towards.
    /// </summary>
    int currPatrolIndex;
    /// <summary>
    /// Returns true if NPC is traveling, or false if not.
    /// </summary>
    bool traveling;
    /// <summary>
    /// Retturns true if NPC is waiting, or false if not.
    /// </summary>
    bool waiting;
    /// <summary>
    /// Returns ture if NPC is moving forward in patrol, or false if not.
    /// </summary>
    bool patrolForward;
    /// <summary>
    /// The timer that tells the NPC when to wait.
    /// </summary>
    float waitTimer;
    #endregion

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("The Nav Mesh Agent component is not attached to " + name, gameObject);
        }
        else
        {
            //if (patrolPoints != null && patrolPoints.Count != 0)
            //{
            //    currPatrolIndex = 0;
            //    SetDestination();
            //}
            //else
            //{
            //    Debug.Log("Insufficiant amount of patrol points for movement.", gameObject);
            //}

            SetDestination();
        }
    }

    //private void Update()
    //{
    //    //Check if we're close to the destination
    //    if (traveling && navMeshAgent.remainingDistance <= 1.0f)
    //    {
    //        traveling = false;

    //        //If we're going to wait, then wait
    //        if (patrolWaiting)
    //        {
    //            waiting = true;
    //            waitTimer = 0;
    //        }
    //        else
    //        {
    //            ChangePatrolPoint();
    //            SetDestination();
    //        }

    //        //Instead if we're waiting
    //        if (waiting)
    //        {
    //            waitTimer += Time.deltaTime;
    //            if (waitTimer >= totalWaitTime)
    //            {
    //                waiting = false;

    //                ChangePatrolPoint();
    //                SetDestination();
    //            }
    //        }
    //    }
    //}

    ///// <summary>
    ///// Selects a ne wpatrol point in the available list, but
    ///// also with a small probability allows for us to move forwards or backwards.
    ///// </summary>
    //private void ChangePatrolPoint()
    //{
    //    if (UnityEngine.Random.Range(0f, 1f) <= switchProbability)
    //    {
    //        patrolForward = !patrolForward;
    //    }

    //    if (patrolForward)
    //    {
    //        currPatrolIndex = (currPatrolIndex + 1) % patrolPoints.Count;
    //    }
    //    else
    //    {
    //        //The -- before something quickly decrements it.
    //        if (--currPatrolIndex < 0)
    //        {
    //            currPatrolIndex = patrolPoints.Count - 1;
    //        }
    //    }
    //}

    private void SetDestination()
    {
        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[0].transform.position;
            navMeshAgent.SetDestination(targetVector);
            //traveling = true;
        }
    }
}