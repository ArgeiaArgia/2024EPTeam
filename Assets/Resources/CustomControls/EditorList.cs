using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorList : VisualElement
{
    public new class UxmlFactory : UxmlFactory<EditorList, UxmlTraits>
    {
    }

    private Foldout _listFoldout;
    private IntegerField _countField;
    private ScrollView _listScrollView;

    private string _listName;

    public string ListName
    {
        get => _listName;
        set
        {
            _listName = value;
            _listFoldout.text = value;
        }
    }

    private Type _listType = typeof(ItemSO);

    public Type ListType
    {
        get => _listType;
        set
        {
            _listType = value;
            _countField.value = 0;
            _list.Clear();
            foreach (var obj in _objectFields)
            {
                obj.RemoveFromHierarchy();
            }

            _objectFields.Clear();
        }
    }

    private IList _list;

    List<VisualElement> _objectItems = new List<VisualElement>();
    List<ObjectField> _objectFields = new List<ObjectField>();
    List<Button> _deleteButtons = new List<Button>();

    private Button _addButton;

    public event Action<IList> OnListChanged;

    public EditorList()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/EditorList");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/EditorList");
        styleSheets.Add(styleSheet);

        _list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new[] { _listType }));

        _listFoldout = this.Q<Foldout>("List");
        _listScrollView = _listFoldout.Q<ScrollView>();

        _countField = new IntegerField();
        _listFoldout.Q<Toggle>().Q<VisualElement>().Add(_countField);
        _countField.label = "";
        _countField.value = 0;
        _countField.AddToClassList("count-field");
        _countField.RegisterValueChangedCallback(UpdateListCount);

        _addButton = this.Q<Button>("AddButton");
        _addButton.clickable.clicked += () => _countField.value++;
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlTypeAttributeDescription<ItemSO> m_listType = new()
            { name = "list-type", defaultValue = typeof(List<>) };

        UxmlStringAttributeDescription m_listName = new()
            { name = "list-name", defaultValue = "List" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as EditorList;

            ate.ListType = m_listType.GetValueFromBag(bag, cc);
            ate.ListName = m_listName.GetValueFromBag(bag, cc);
        }
    }

    private void UpdateListCount(int count)
    {
        if (count < 0)
        {
            _countField.value = 0;
            foreach (var obj in _objectFields)
            {
                obj.RemoveFromHierarchy();
            }
            foreach (var btn in _deleteButtons)
            {
                btn.RemoveFromHierarchy();
            }
            foreach (var item in _objectItems)
            {
                item.RemoveFromHierarchy();
            }
            _objectFields.Clear();
            _deleteButtons.Clear();
            _objectItems.Clear();
            
            _list.Clear();
        }

        for (var i = count; i < _list.Count; i++)
        {
            _list.RemoveAt(i);
            var field = _objectFields[i];
            var deleteButton = _deleteButtons[i];
            var item = _objectItems[i];
        
            _objectFields.RemoveAt(i);
            _deleteButtons.RemoveAt(i);
            _objectItems.RemoveAt(i);
        
            field.RemoveFromHierarchy();
            deleteButton.RemoveFromHierarchy();
            item.RemoveFromHierarchy();
        }

        for (var i = _list.Count; i < count; i++)
        {
            _list.Add(default);
            var objectField = new ObjectField();
            objectField.objectType = _listType;
            objectField.label = i.ToString();
            objectField.RegisterValueChangedCallback(evt =>
            {
                _list[i -1 ] = evt.newValue;
            });
            _objectFields.Add(objectField);
            
            VisualElement objectItem = new VisualElement();
            objectItem.AddToClassList("object-item");
            _objectItems.Add(objectItem);
            
            Button deleteButton = new Button();
            deleteButton.text = "X";
            _deleteButtons.Add(deleteButton);
            
            _listScrollView.Add(objectItem);
            objectItem.Add(objectField);
            objectItem.Add(deleteButton);

            deleteButton.clickable.clicked += ()=>HandleDeleteButton(int.Parse(objectField.label));
        }
        
        OnListChanged?.Invoke(_list);
    }

    private void HandleDeleteButton(int i)
    {
        _countField.SetValueWithoutNotify(_countField.value - 1);
        
        Debug.Log(_list[i]);
        _list.RemoveAt(i);
        var field = _objectFields[i];
        var deleteButton = _deleteButtons[i];
        var item = _objectItems[i];
        
        _objectFields.RemoveAt(i);
        _deleteButtons.RemoveAt(i);
        _objectItems.RemoveAt(i);
        
        field.RemoveFromHierarchy();
        deleteButton.RemoveFromHierarchy();
        item.RemoveFromHierarchy();
        
        for (var j = i; j < _objectFields.Count; j++)
        {
            _objectFields[j].label = j.ToString();
        }
    }

    private void UpdateListCount(ChangeEvent<int> evt) => UpdateListCount(evt.newValue);

    public List<T> GetList<T>()
    {
        if (_listType is not T)
        {
            throw new InvalidCastException();
        }
        return (List<T>)_list;
    }
    
    public void AddList(object item)
    {
        if(item.GetType() != _listType)
            throw new InvalidCastException();
        _list.Add(item);
        _countField.SetValueWithoutNotify(_list.Count);
    }
}