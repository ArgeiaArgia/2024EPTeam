using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ToolkitParents : SerializedMonoBehaviour
{
    protected UIDocument UIDocument;
    protected VisualElement root;
    protected virtual void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
    }

    protected virtual void OnEnable()
    {
        root = UIDocument.rootVisualElement;
    }

    public virtual void EnableUI()
    {
        
    }
    protected virtual void DisableUI()
    {
        
    }
}
