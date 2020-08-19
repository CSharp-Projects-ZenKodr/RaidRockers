using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Return true to have the cursor's debug properties show, or false if not.
    /// </summary>
    [Tooltip("Return true to have the cursor's debug properties show, or false if not.")]
    public bool debugMode = false;
    /// <summary>
    /// The amount of seconds that pass until the cursor tag shows.
    /// </summary>
    [Space(8)]
    [Header("Cursor Tag")]
    public float secondsTilTagShows;
    /// <summary>
    /// The image that represents the cursor tag.
    /// </summary>
    public GameObject tagBox;
    /// <summary>
    /// The text component that makes up the tag.
    /// </summary>
    public Text tagText;
    /// <summary>
    /// The transform that holds our rect for multi-select.
    /// </summary>
    [Space(8)]
    [Header("Multi-selection")]
    public RectTransform multiSelectRect;

    /// <summary>
    /// Return true if the player is attempting to multi-select, or false if not.
    /// </summary>
    public bool isMutliSelecting { get; private set; }

    /// <summary>
    /// The string that is attached to the debug statements to let me know the cursor is in debug mode.
    /// </summary>
    private const string dbm = "Cursor debug: ";

    /// <summary>
    /// The animator attached to this cursor.
    /// </summary>
    private Animator cursorAnimator;
    /// <summary>
    /// The starting position of the cursor when we go for a multi-select.
    /// </summary>
    private Vector2 cursorSelectStartPos;
    /// <summary>
    /// The miners that are in the scene.
    /// </summary>
    private List<PlayableMiner> minersInScene;
    #endregion

    private void Awake()
    {
        Cursor.visible = debugMode;
        cursorAnimator = GetComponent<Animator>();
        //Make sure the cursor is above all other UI elements.
        transform.SetAsLastSibling();
        //Get all the miners in the scene
        minersInScene = FindObjectsOfType<PlayableMiner>().ToList();
    }

    void Update()
    {
        //Have sprite follow the mouse cursor.
        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(cursorPos);
        transform.position = Input.mousePosition;

        //Multi-selection
        MultiSelection();

        //Have the mouse visual react dynamically to hovered object when a miner is selected.
        DynamicMouseOnMinerSelected();
    }

    #region Multi-Selecting
    /// <summary>
    /// The function that will handle our mutli-selection.
    /// </summary>
    private void MultiSelection()
    {
        //mouse down
        if (Input.GetMouseButtonDown(0))
        {
            //Set the start position of mouse
            cursorSelectStartPos = Input.mousePosition;
        }

        //mouse up
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        //mouse held down
        if (Input.GetMouseButton(0))
        {
            //Update the selection box
            UpdateMultiSelectRect(Input.mousePosition);
        }

        isMutliSelecting = DetermineWhetherPlayerIsMultiSelecting();
    }

    /// <summary>
    /// Determines whether the player is multi-selecting or not.
    /// </summary>
    /// <returns>
    /// Is the player multi-selecting?
    /// </returns>
    private bool DetermineWhetherPlayerIsMultiSelecting()
    {
        bool output = false;

        if (multiSelectRect.sizeDelta.x > float.Epsilon && multiSelectRect.sizeDelta.y > float.Epsilon)
        {
            output = true;
        }

        return output;
    }

    /// <summary>
    /// Updates the multi-selection rect visual
    /// </summary>
    /// <param name="currentMousePosition">
    /// The current position of the mouse cursor
    /// </param>
    void UpdateMultiSelectRect(Vector2 currentMousePosition)
    {
        //Note: works only with canvases that are of constant pixel size
        if (!multiSelectRect.gameObject.activeInHierarchy)
        {
            multiSelectRect.gameObject.SetActive(true);
        }

        float width = currentMousePosition.x - cursorSelectStartPos.x;
        float height = currentMousePosition.y - cursorSelectStartPos.y;

        multiSelectRect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        multiSelectRect.anchoredPosition = cursorSelectStartPos + new Vector2(width / 2, height / 2);
    }
    
    /// <summary>
    /// What happens when we release the selection box.
    /// </summary>
    void ReleaseSelectionBox()
    {
        //Disable visual
        multiSelectRect.gameObject.SetActive(false);

        //Get bounds (min/max) of drawn box
        //Bottom left of selection box.
        Vector2 min = multiSelectRect.anchoredPosition - (multiSelectRect.sizeDelta / 2);
        //Top right of selection box.
        Vector2 max = multiSelectRect.anchoredPosition + (multiSelectRect.sizeDelta / 2);

        //See which units we overlapped by turning their world positions into screen coords
        //Todo: Switch from example to actual implementation
        //Example
        foreach (PlayableMiner miner in minersInScene)
        {
            //Get the position of the miner, and turn it into screen space
            Vector3 minerScreenPos = Camera.main.WorldToScreenPoint(miner.transform.position);

            //is the x position of miner greater than min, but less than max
            if (minerScreenPos.x > min.x && minerScreenPos.x < max.x && minerScreenPos.y > min.y && minerScreenPos.y < max.y)
            {
                //Todo: select miner properly
                miner.SelectMiner(true);
            }
        }

        cursorAnimator.SetTrigger("Check");
    }
    #endregion

    /// <summary>
    /// Changes up the cursor visual based on what it's hovered over when a miner is selected.
    /// </summary>
    private void DynamicMouseOnMinerSelected()
    {
        SelectableObject hoverObj = SelectableObject.currentHoveredObject;

        if (hoverObj == null)
        {
            cursorAnimator.ResetTrigger("Move");
            cursorAnimator.SetTrigger("Default");
            return;
        }

        //Once a Miner is selected
        if (PlayableMiner.aMinerIsSelected)
        {
            switch (hoverObj.objectName)
            {
                case "GROUND":
                    cursorAnimator.SetTrigger("Move");
                    if (debugMode) Debug.Log(dbm + "1 - Over Ground", gameObject);
                    break;
                case "MINER":
                    cursorAnimator.ResetTrigger("Move");
                    cursorAnimator.SetTrigger("Default");
                    if (debugMode) Debug.Log(dbm + "2 - Over Miner", gameObject);
                    break;
                default:
                    cursorAnimator.ResetTrigger("Move");
                    cursorAnimator.SetTrigger("Default");
                    if (debugMode) Debug.Log(dbm + "3 - Not accounted for", gameObject);
                    break;
            } 
        }
    }

    /// <summary>
    /// Shows the cursor tag.
    /// </summary>
    /// <param name="tagValue">
    /// The value of what the tag will say.
    /// </param>
    /// <returns></returns>
    public IEnumerator ShowTag(string tagValue)
    {
        yield return new WaitForSeconds(secondsTilTagShows);
        tagText.text = tagValue;
        tagBox.SetActive(true);
    }
}