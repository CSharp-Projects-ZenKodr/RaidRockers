using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Return true to show the cursor during gameplay, or false if not.
    /// </summary>
    [Tooltip("Return true to show the cursor during gameplay, or false if not.  Debugging purposes only.")]
    public bool showCursor = false;
    //[Space(8)]
    #endregion

    private void Start()
    {
        Cursor.visible = showCursor;
    }

    void Update()
    {
        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(cursorPos);
        transform.position = Input.mousePosition;
    }
}