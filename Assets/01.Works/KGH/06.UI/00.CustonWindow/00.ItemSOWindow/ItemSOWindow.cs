using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSOWindow : EditorWindow
{
    public string SOPath = "00.ForEveryone/02.SOs/03.ItemSOs";
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private VisualTreeAsset m_itemElement = default;

    private ItemSO _currentItem;
    private ItemInspector _itemInspector;
    private ItemListView _itemListView;

    [MenuItem("CustomWindow/ItemSOWindow")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<ItemSOWindow>();
        wnd.titleContent = new GUIContent("ItemSOWindow");
        wnd.minSize = new Vector2(400, 500);
    }

    private void OnEnable() { }

    public void CreateGUI()
    {
        var root = rootVisualElement;
        VisualElement content = m_VisualTreeAsset.Instantiate();
        content.style.flexGrow = 1;
        root.Add(content);
        InitializeWindow(content);
    }

    private void InitializeWindow(VisualElement content)
    {
        _itemInspector = new ItemInspector(content, this);
        _itemListView = new ItemListView(content, this, m_itemElement);
        _itemInspector.OnNameChange += ChangeItemName;
        _itemInspector.OnTypeChange += ChangeItemType;
        _itemInspector.OnIconChange += (item, sprite) => _itemListView.ChangeItemIcon(item, sprite);
        _itemListView.OnItemSelect += SelectItem;

        foreach (var item in GetItemList())
        {
            _itemListView.SetUpItem(item);
        }
    }

    private void SelectItem(ItemSO item)
    {
        _currentItem = item;
        _itemInspector.ChangeItem(item);
    }

    private void ChangeItemType(ItemSO item, ItemType type)
    {
        _itemListView.MoveType(item);
    }

    private void ChangeItemName(ItemSO targetItem, string name)
    {
        if (GetItemList().Any(item => item.name == name))
        {
            Debug.LogError("Already Exist");
            return;
        }

        var path = AssetDatabase.GetAssetPath(targetItem);
        AssetDatabase.RenameAsset(path, name);
        targetItem.itemName = name;
        targetItem.name = name;
        _itemListView.ChangeItemName(targetItem, name);
        UnityEditor.EditorUtility.SetDirty(targetItem);
    }

    public List<ItemSO> GetItemList()
    {
        var dir = new DirectoryInfo($"Assets/{SOPath}");
        var info = dir.GetFiles("*.asset");
        return info.Select(fileInfo =>
        {
            var asset = AssetDatabase.LoadAssetAtPath<ItemSO>($"Assets/{SOPath}/{fileInfo.Name}");
            return asset;
        }).ToList();
    }
}