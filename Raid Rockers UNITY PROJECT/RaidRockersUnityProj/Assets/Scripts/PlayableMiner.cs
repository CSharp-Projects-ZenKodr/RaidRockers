using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableMiner : SelectableObject {
    #region Variables
    /// <summary>
    /// Return true if one or more miners are selected, or false if not.
    /// </summary>
    public static bool aMinerIsSelected = false;

    /// <summary>
    /// The NavMeshAgent attached to this playable miner.
    /// </summary>
    private NavMeshAgent attachedAgent;
    #endregion

    private void Start()
    {
        attachedAgent = GetComponent<NavMeshAgent>();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        aMinerIsSelected = true;
        attachedAgent.isStopped = true;
        selectAfterMouseDown = false;
    }

    //Instead of immediately selecting another object, we want to move to where we click
    private void Update()
    {
        MinerMovement();
    }

    void MinerMovement ()
    {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            //Moving the miner to a point on the ground that I clicked.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool didRayHit = Physics.Raycast(ray, out hit);

            if (didRayHit)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    //Allow the miner to move again
                    attachedAgent.isStopped = false;
                    //Set it's destination to where I clicked
                    attachedAgent.SetDestination(hit.point);
                    //Deselect the miner
                    DeselectPreviousObject();
                    selectAfterMouseDown = true;
                    //Tell the cursor the miner was deselected
                    //TODO: When the player clicks, the check does play, but the cursor overrides it and goes default
                    cursorAnimator.SetTrigger("Check");
                    cursorAnimator.ResetTrigger("Move");
                    aMinerIsSelected = false;
                }
            }
        }
    }
}