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
    /// The string that is attached to the debug statements to let me know the cursor is in debug mode.
    /// </summary>
    private const string dbm = "Cursor debug: ";

    /// <summary>
    /// The animator attached to this cursor.
    /// </summary>
    private Animator cursorAnimator;
    #endregion

    private void Awake()
    {
        Cursor.visible = debugMode;
        cursorAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        //Have sprite follow the mouse cursor.
        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(cursorPos);
        transform.position = Input.mousePosition;

        //Have the mouse visual react dynamically to hovered object when a miner is selected.
        DynamicMouseOnMinerSelected();
    }
    
    private void DynamicMouseOnMinerSelected()
    {
        SelectableObject hoverObj = SelectableObject.currentHoveredObject;

        if (hoverObj != null && PlayableMiner.aMinerIsSelected)
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