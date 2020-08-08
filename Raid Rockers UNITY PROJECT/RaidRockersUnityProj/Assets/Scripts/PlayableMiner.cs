using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableMiner : SelectableObject
{
    #region Variables
    /// <summary>
    /// The NavMeshAgent attached to this playable miner.
    /// </summary>
    NavMeshAgent attachedAgent;
    #endregion

    private void Start ()
    {
        attachedAgent = GetComponent<NavMeshAgent>();
    }

    //Instead of immediately selecting another object, we want to move to where we click
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentSelectedObject != this)
            {
                Debug.Log("Go");
                attachedAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}