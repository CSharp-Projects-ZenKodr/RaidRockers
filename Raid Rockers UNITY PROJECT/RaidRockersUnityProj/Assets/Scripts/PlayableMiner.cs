using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableMiner : SelectableObject {
    #region Variables
    /// <summary>
    /// The NavMeshAgent attached to this playable miner.
    /// </summary>
    NavMeshAgent attachedAgent;
    #endregion

    private void Start()
    {
        attachedAgent = GetComponent<NavMeshAgent>();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        selectAfterMouseDown = false;
    }

    //Instead of immediately selecting another object, we want to move to where we click
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool didRayHit = Physics.Raycast(ray, out hit);

            if (didRayHit)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    Debug.Log("Go");
                    attachedAgent.SetDestination(hit.point);
                    DeselectPreviousObject();
                    selectAfterMouseDown = true; ;
                }
            }
        }
    }
}