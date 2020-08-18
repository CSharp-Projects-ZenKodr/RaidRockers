using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The color of the object when hovered over.
    /// </summary>
    public static Color hoverColor = new Color(0.624f, 0.894f, 0.98f, 1f);
    /// <summary>
    /// The color of the object when selected.
    /// </summary>
    public static Color selectedColor = new Color(0.212f, 0.145f, 1f, 1f);
    /// <summary>
    /// A lost of the objects that are is currently selected.
    /// </summary>
    //Todo: Probably going to need to refactor because of the change to a list for this.
    //Note: Yup
    public static List<SelectableObject> currentSelectedObjects = new List<SelectableObject>();
    /// <summary>
    /// The object that is currently being hovered over.
    /// </summary>
    public static SelectableObject currentHoveredObject = null;
    /// <summary>
    /// Return true if we should select something after clicking, or false if not.
    /// </summary>
    public static bool selectAfterMouseUp = true;

    /// <summary>
    /// Return true if object is selected, or false if not.
    /// </summary>
    public bool selected { get; set; }
    /// <summary>
    /// The name of this object that will show up in the UI.
    /// </summary>
    public string objectName;
    /// <summary>
    /// The material that is on this object.
    /// </summary>
    public Material attachedMaterial { get; private set; }
    /// <summary>
    /// The initial color of the object.
    /// </summary>
    public Color initialColor { get; private set; }
    /// <summary>
    /// The player's cursor.
    /// </summary>
    public PlayerCursor cursorScript { get; private set; }
    /// <summary>
    /// The animator attached to the player animator.
    /// </summary>
    public Animator cursorAnimator { get; private set; }

    /// <summary>
    /// The renderer attached to this object.
    /// </summary>
    private Renderer attachedRenderer;
    /// <summary>
    /// The GameObject that holds the cursor data.
    /// </summary>
    private GameObject cursorObject;
    #endregion

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        selected = false;

        //Get player cursor
        cursorObject = GameObject.FindGameObjectWithTag("Cursor");
        cursorScript = cursorObject.GetComponent<PlayerCursor>();
        cursorAnimator = cursorObject.GetComponent<Animator>();
        //Get Material
        attachedRenderer = GetComponent<Renderer>();
        attachedMaterial = attachedRenderer.material;
        //Store initial color
        initialColor = attachedMaterial.color;
    }

    public virtual void OnMouseEnter()
    {
        if (!selected && selectAfterMouseUp)
        {
            attachedMaterial.color += hoverColor;
        }

        currentHoveredObject = this;
        cursorScript.StartCoroutine(cursorScript.ShowTag(objectName));
    }
    
    public virtual void OnMouseUp()
    {
        if (cursorScript.isMutliSelecting)
        {
            return;
        }

        if (selectAfterMouseUp)
        {
            //We currently have something selected, and want to select something else that's not this.
            if (currentSelectedObjects.Count > 0  && currentSelectedObjects.First() != this)
            {
                //Reset previously selected object
                DeselectPreviousObject();
            }

            if (!selected)
            {
                //Make this the currently selected object
                currentSelectedObjects.Add(this);
                attachedMaterial.color = initialColor + selectedColor;
                selected = true;
            } 
        }
    }

    /// <summary>
    /// Deselects the object that was previous selected.
    /// </summary>
    public void DeselectPreviousObject()
    {
        currentSelectedObjects.First().attachedMaterial.color = currentSelectedObjects.First().initialColor;
        currentSelectedObjects.First().selected = false;
    }

    public virtual void OnMouseExit()
    {
        if (!selected)
        {
            attachedMaterial.color = initialColor;
        }

        cursorScript.StopAllCoroutines();
        cursorScript.tagBox.SetActive(false);

        currentHoveredObject = null;
    }
}