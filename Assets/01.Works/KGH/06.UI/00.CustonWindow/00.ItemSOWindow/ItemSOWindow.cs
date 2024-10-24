using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSOWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private ItemSO _currentItem;

    private ItemInspector _itemInspector;
    
    [MenuItem("CustomWindow/ItemSOWindow")]
    public static void ShowWindow()
    {
        ItemSOWindow wnd = GetWindow<ItemSOWindow>();
        wnd.titleContent = new GUIContent("ItemSOWindow");
        wnd.minSize = new Vector2(400, 500);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement content = m_VisualTreeAsset.Instantiate();
        content.style.flexGrow = 1;
        root.Add(content);
        
        InitializeWindow(content);
        CreateItemList();
    }
    private void InitializeWindow(VisualElement content)
    {
        _itemInspector = new ItemInspector(content, this);
    }

    private void CreateItemList()
    {
    }
}

[Serializable]
public class DescList
{
    public List<string> descList;
}