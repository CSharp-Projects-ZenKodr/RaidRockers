using System;
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
        InteractWithAClick();
    }

    /// <summary>
    /// The function that handles what happens when the player clicks something in the environment when a miner is selected.
    /// </summary>
    void InteractWithAClick()
    {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            if (currentHoveredObject == null)
            {
                Debug.Log(name + " will do nothing.", gameObject);
                DeselectMiner();
                //Animate cursor
                cursorAnimator.ResetTrigger("Move");
                cursorAnimator.SetTrigger("Cross");
                return;
            }

            switch(currentHoveredObject.objectName)
            {
                case "GROUND":
                    MinerMoveCommand();
                    break;
                case "MINER":
                    //Do nothing, maybe play check anim
                    cursorAnimator.ResetTrigger("Move");
                    cursorAnimator.SetTrigger("Check");
                    break;
                default:
                    Debug.Log(name + " will do nothing.", gameObject);
                    DeselectMiner();
                    //Animate cursor
                    cursorAnimator.ResetTrigger("Move");
                    cursorAnimator.SetTrigger("Cross");
                    break;
            }
        }
    }

    /// <summary>
    /// The command that tells the miner to move where player clicked
    /// </summary>
    private void MinerMoveCommand()
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
    }

    /// <summary>
    /// Deselects the miner.
    /// </summary>
    void DeselectMiner()
    {
        //Deselect the miner
        DeselectPreviousObject();
        selectAfterMouseDown = true;
        aMinerIsSelected = false;
    }
}