using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableMiner : SelectableObject
{

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

        //Stop Miner in place
        attachedAgent.isStopped = true;

        selectAfterMouseDown = false;

        cursorAnimator.SetTrigger("Check");
    }

    //Instead of immediately selecting another object, we want to move to where we click
    private void Update()
    {
        MinerMovement();
    }

    void MinerMovement()
    {
        //Todo: make sure the place we're clicking and moving to is valid (not a wall, on the NavMesh)
        if (Input.GetMouseButtonDown(0) && selected && currentHoveredObject != null && currentHoveredObject.objectName == "GROUND")
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
                    aMinerIsSelected = false;
                    //Tell the cursor the miner was deselected
                    cursorAnimator.SetTrigger("Check");
                    cursorAnimator.ResetTrigger("Move");
                }
            }
            else
            {
                //TODO: When the player clicks, the cross does play, but the cursor overrides it and goes default
                Debug.Log("Don't move");
                //Deselect the miner
                DeselectPreviousObject();
                selectAfterMouseDown = true;
                aMinerIsSelected = false;
                //Animate cursor
                cursorAnimator.ResetTrigger("Move");
                cursorAnimator.SetTrigger("Cross");
            }
        }
    }
}