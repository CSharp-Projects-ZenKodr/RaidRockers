using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The name of this object that will show up in the UI.
    /// </summary>
    public string objectName;
    /// <summary>
    /// The color of the object when hovered over.
    /// </summary>
    public static Color hoverColor = new Color(0.624f, 0.894f, 0.98f, 1f);
    /// <summary>
    /// The color of the object when selected.
    /// </summary>
    public static Color selectedColor = new Color(0.212f, 0.145f, 1f, 1f);
    /// <summary>
    /// The object that is currently selected.
    /// </summary>
    public static SelectableObject currentSelectedObject = null;
    /// <summary>
    /// Return true if we should select something after clicking, or false if not.
    /// </summary>
    public static bool selectAfterMouseDown = true;
    
    /// <summary>
    /// The material that is on this object.
    /// </summary>
    public Material attachedMaterial { get; private set; }
    /// <summary>
    /// The initial color of the object.
    /// </summary>
    public Color initialColor { get; private set; }
    /// <summary>
    /// Return true if object is selected, or false if not.
    /// </summary>
    public bool selected { get; private set; }

    /// <summary>
    /// The renderer attached to this object.
    /// </summary>
    private Renderer attachedRenderer;
    #endregion

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        selected = false;

        //Get Material
        attachedRenderer = GetComponent<Renderer>();
        attachedMaterial = attachedRenderer.material;
        //Store initial color
        initialColor = attachedMaterial.color;
    }

    public virtual void OnMouseEnter()
    {
        if (!selected && selectAfterMouseDown)
        {
            attachedMaterial.color += hoverColor; 
        }
    }

    public virtual void OnMouseDown()
    {
        if (selectAfterMouseDown)
        {
            //We currently have something selected, and want to select something else that's not this.
            if (currentSelectedObject != null && currentSelectedObject != this)
            {
                //Reset previously selected object
                DeselectPreviousObject();
            }

            if (!selected)
            {
                //Make this the currently selected object
                currentSelectedObject = this;
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
        currentSelectedObject.attachedMaterial.color = currentSelectedObject.initialColor;
        currentSelectedObject.selected = false;
    }

    public virtual void OnMouseExit()
    {
        if (!selected)
        {
            attachedMaterial.color = initialColor;
        }
    }
}