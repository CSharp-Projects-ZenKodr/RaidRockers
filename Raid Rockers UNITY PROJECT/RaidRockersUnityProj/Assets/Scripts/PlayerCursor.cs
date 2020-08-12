using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Return true to show the cursor during gameplay, or false if not.
    /// </summary>
    [Tooltip("Return true to show the cursor during gameplay, or false if not.  Debugging purposes only.")]
    public bool showCursor = false;
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