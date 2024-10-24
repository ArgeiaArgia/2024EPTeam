using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorDictionary : VisualElement
{
    public new class UxmlFactory : UxmlFactory<EditorDictionary, UxmlTraits>{}
    
    private Foldout _dictionaryFoldout;
    private IntegerField _countField;
    private ScrollView _dictionaryScrollView;
    private Button _addButton;

    private Type _keyType;
    public Type KeyType
    {
        get => _keyType;
        set
        {
            _keyType = value;
            ResetDictionary();
        }
    }

    private Type _valueType;
    public Type ValueType
    {
        get => _valueType;
        set
        {
            _valueType = value;
            ResetDictionary();
        }
    }
    
    string _dictionaryName;
    public string DictionaryName
    {
        get => _dictionaryName;
        set
        {
            _dictionaryName = value;
            _dictionaryFoldout.text = value;
        }
    }

    public IDictionary _dictionary;
    private List<VisualElement> _itemElements = new List<VisualElement>();
    
    public EditorDictionary()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/EditorDictionary");
        visualTree.CloneTree(this);
        
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/EditorDictionary");
        styleSheets.Add(styleSheet);
        
        _dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(_keyType, _valueType));
        
        _dictionaryFoldout = this.Q<Foldout>("DictionaryFoldout");
        _dictionaryScrollView = this.Q<ScrollView>("DictionaryScrollView");
        _addButton = this.Q<Button>("AddButton");

        _countField = new IntegerField();
        _dictionaryFoldout.Q<Toggle>().Q<VisualElement>().Add(_countField);
        _countField.label = "";
        _countField.value = 0;
        _countField.AddToClassList("count-field");
        _countField.RegisterValueChangedCallback(UpdateDictionaryCount);

    }


    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlTypeAttributeDescription<string> m_keyType = new () { name = "key-type" , defaultValue = typeof(string)};
        UxmlTypeAttributeDescription<string> m_valueType = new () { name = "value-type" , defaultValue = typeof(string)};
        
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as EditorDictionary;
            
            ate.KeyType = m_keyType.GetValueFromBag(bag, cc);
            ate.ValueType = m_valueType.GetValueFromBag(bag, cc);
        }
    }

    private void ResetDictionary()
    {
        _countField.SetValueWithoutNotify(0);
    }
    private void UpdateDictionaryCount(ChangeEvent<int> evt)
    {
        var value = evt.newValue;
        if(value < 0)
        {
            ResetDictionary();
        }
        else
        {
            for (int i = value; i < _dictionary.Count; i++)
            {
                RemoveElement(i);
            }
            for (int i = _dictionary.Count; i < value; i++)
            {
                AddElement();
            }
        }
    }
    private void RemoveElement(int i)
    {
        
    }
    private void AddElement()
    {
        var item = new VisualElement();
        var index = new Label();
        var keyAndValue = new VisualElement();
        var deleteButton = new Button();
        //object, enum, string, int
        if (_keyType.IsEnum)
        {
        }
        
        _dictionaryFoldout.Add(item);
        item.AddToClassList(".object-item");
        
    }
}
