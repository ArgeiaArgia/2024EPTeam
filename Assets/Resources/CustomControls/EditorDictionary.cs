using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorDictionary : VisualElement
{
    public new class UxmlFactory : UxmlFactory<EditorDictionary, UxmlTraits>
    {
    }

    private Foldout _dictionaryFoldout;
    private IntegerField _countField;
    private ScrollView _dictionaryScrollView;
    private Button _addButton;
    private VisualElement _dictionaryContainer;

    private Type _keyType = typeof(StatType);

    public Type KeyType
    {
        get => _keyType;
        set
        {
            _keyType = value;
            ResetDictionary();
        }
    }

    private Type _valueType = typeof(string);

    public Type ValueType
    {
        get => _valueType;
        set
        {
            _valueType = value;
            ResetDictionary();
        }
    }

    private string _dictionaryName;

    public string DictionaryName
    {
        get => _dictionaryName;
        set
        {
            _dictionaryName = value;
            _dictionaryFoldout.text = value;
        }
    }

    private bool isKeyItemSo;
    public bool IsKeyItemSo
    {
        get => isKeyItemSo;
        set
        {
            isKeyItemSo = value;
            KeyType = value ? typeof(ItemSO) : typeof(StatType);
        }
    }

    private IList _keys;
    private IList _values;
    private readonly List<VisualElement> _itemElements = new List<VisualElement>();
    public Action<IDictionary> OnDictionaryChanged;

    public EditorDictionary()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/EditorDictionary");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/EditorDictionary");
        styleSheets.Add(styleSheet);

        _keys = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_keyType));
        _values = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_valueType));

        _dictionaryFoldout = this.Q<Foldout>("DictionaryFoldout");
        _dictionaryScrollView = this.Q<ScrollView>("DictionaryScrollView");
        _addButton = this.Q<Button>("AddButton");
        _dictionaryContainer = this.Q<VisualElement>("DictionaryContainer");

        _countField = new IntegerField();
        _dictionaryFoldout.Q<Toggle>().Q<VisualElement>().Add(_countField);
        _countField.label = "";
        _countField.value = 0;
        _countField.AddToClassList("count-field");
        _countField.RegisterValueChangedCallback(UpdateDictionaryCount);

        _addButton.clicked += () =>
        {
            AddElement();
            _countField.SetValueWithoutNotify(_countField.value + 1);
        };
    }


    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_dictionaryName = new()
            { name = "dictionary-name", defaultValue = "Dictionary" };

        UxmlBoolAttributeDescription m_isKeyItemSo = new()
            { name = "is-key-item-so", defaultValue = false };
        //
        // readonly UxmlTypeAttributeDescription<Type> m_keyType = new()
        //     { name = "key-type", defaultValue = typeof(StatType) };
        //
        // readonly UxmlTypeAttributeDescription<Type> m_valueType = new()
        //     { name = "value-type", defaultValue = typeof(int) };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as EditorDictionary;

            ate.DictionaryName = m_dictionaryName.GetValueFromBag(bag, cc);
            ate.IsKeyItemSo = m_isKeyItemSo.GetValueFromBag(bag, cc);
            // ate.KeyType = m_keyType.GetValueFromBag(bag, cc);
            // ate.ValueType = m_valueType.GetValueFromBag(bag, cc);
        }
    }

    private void ResetDictionary()
    {
        _countField.SetValueWithoutNotify(0);
        _keys.Clear();
        _values.Clear();
        for (var i = 0; i < _itemElements.Count; i++)
        {
            var item = _itemElements[i];
            _itemElements.Remove(item);
            item.RemoveFromHierarchy();
        }

        _keys = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_keyType));
        _values = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_valueType));

        OnDictionaryChanged?.Invoke(GetDictionary());
    }

    private void UpdateDictionaryCount(ChangeEvent<int> evt)
    {
        var value = evt.newValue;
        if (value < 0)
        {
            ResetDictionary();
        }
        else
        {
            for (var i = value; i < _keys.Count; i++)
            {
                RemoveElement(i);
            }

            for (var i = _keys.Count; i < value; i++)
            {
                AddElement();
            }
        }
    }

    private void RemoveElement(VisualElement item)
    {
        var i = _itemElements.IndexOf(item);
        RemoveElement(i);
    }

    private void RemoveElement(int i)
    {
        _countField.SetValueWithoutNotify(_countField.value - 1);
        var item = _itemElements[i];
        _itemElements.Remove(item);
        item.RemoveFromHierarchy();
        _keys.RemoveAt(i);
        _values.RemoveAt(i);

        for (var j = i; j < _itemElements.Count; j++)
        {
            _itemElements[j].Q<Label>().text = j.ToString();
        }

        OnDictionaryChanged?.Invoke(GetDictionary());
    }

    private void AddElement()
    {
        var i = _keys.Count;
        var item = new VisualElement();
        _dictionaryContainer.Add(item);
        item.AddToClassList("object-item");
        var index = new Label() { text = i.ToString() };
        item.Add(index);
        var keyAndValue = new VisualElement();
        keyAndValue.AddToClassList("object-item");
        item.Add(keyAndValue);
        var deleteButton = new Button(() => RemoveElement(item)) { text = "X" };
        item.Add(deleteButton);

        if (_keyType.IsEnum)
        {
            var enumLength = Enum.GetValues(_keyType).Length;
            var a = -1;
            for (int j = 0; j < enumLength; j++)
            {
                var isKeyExist = false;
                foreach (var k in _keys)
                {
                    if (Enum.GetValues(_keyType).GetValue(j).Equals(k))
                    {
                        isKeyExist = true;
                        break;
                    }
                }
                if (!isKeyExist)
                {
                    a = j;
                    break;
                }
            }
            if(a == -1)
            {
                Debug.LogError("No more key available");
                item.RemoveFromHierarchy();
                _countField.SetValueWithoutNotify(_countField.value - 1);
                return;
            }

            var defaultKey = Enum.GetValues(_keyType).GetValue(a) as Enum;
            var keyField = new EnumField("", defaultKey);
            keyAndValue.Add(keyField);
            keyField.AddToClassList("key-value-field");

            _keys.Add(defaultKey);

            keyField.RegisterValueChangedCallback(evt =>
            {
                _keys[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else if (_keyType == typeof(string))
        {
            var keyField = new TextField("");
            keyAndValue.Add(keyField);
            keyField.AddToClassList("key-value-field");

            _keys.Add("");

            keyField.RegisterValueChangedCallback(evt =>
            {
                _keys[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else if (_keyType == typeof(int))
        {
            var keyField = new IntegerField("");
            keyAndValue.Add(keyField);
            keyField.AddToClassList("key-value-field");

            _keys.Add(0);

            keyField.RegisterValueChangedCallback(evt =>
            {
                _keys[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else
        {
            var keyField = new ObjectField("") { objectType = _keyType };
            keyAndValue.Add(keyField);
            keyField.AddToClassList("key-value-field");

            _keys.Add(null);

            keyField.RegisterValueChangedCallback(evt =>
            {
                _keys[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }

        if (_valueType.IsEnum)
        {
            var defaultValue = Enum.GetValues(_valueType).GetValue(0) as Enum;
            var valueField = new EnumField("", defaultValue);
            keyAndValue.Add(valueField);
            valueField.AddToClassList("key-value-field");

            _values.Add(defaultValue);

            valueField.RegisterValueChangedCallback(evt =>
            {
                _values[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else if (_valueType == typeof(string))
        {
            var valueField = new TextField("");
            keyAndValue.Add(valueField);
            valueField.AddToClassList("key-value-field");

            _values.Add("");

            valueField.RegisterValueChangedCallback(evt =>
            {
                _values[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else if (_valueType == typeof(int))
        {
            var valueField = new IntegerField("");
            keyAndValue.Add(valueField);
            valueField.AddToClassList("key-value-field");

            _values.Add(0);

            valueField.RegisterValueChangedCallback(evt =>
            {
                _values[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }
        else
        {
            var valueField = new ObjectField("") { objectType = _valueType };
            keyAndValue.Add(valueField);
            valueField.AddToClassList("key-value-field");

            _values.Add(null);

            valueField.RegisterValueChangedCallback(evt =>
            {
                _values[int.Parse(index.text)] = evt.newValue;
                OnDictionaryChanged?.Invoke(GetDictionary());
            });
        }

        _itemElements.Add(item);

        OnDictionaryChanged?.Invoke(GetDictionary());
    }

    public IDictionary GetDictionary()
    {
        var returnDictionary =
            Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(_keyType, _valueType)) as IDictionary;
        for (var i = 0; i < _keys.Count; i++)
        {
            var key = Convert.ChangeType(_keys[i], _keyType);
            var value = Convert.ChangeType(_values[i], _valueType);
            returnDictionary?.Add(key, value);
        }

        return returnDictionary;
    }

    public void AddDictionaryItem<T1, T2>(T1 key, T2 value)
    {
        if (typeof(T1) != _keyType || typeof(T2) != _valueType)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        AddElement();

        if (_keys.Count >= 1) _keys[^1] = key;
        if (_values.Count >= 1) _values[^1] = value;

        _countField.SetValueWithoutNotify(_keys.Count);

        if (_keyType.IsEnum)
        {
            var enumKey = key as Enum;
            _itemElements[_keys.Count - 1].Q<EnumField>().SetValueWithoutNotify(key as Enum);
        }
        else if (_keyType == typeof(string))
        {
            _itemElements[_keys.Count - 1].Q<TextField>().SetValueWithoutNotify(key as string);
        }
        else if (_keyType == typeof(int))
        {
            _itemElements[_keys.Count - 1].Q<IntegerField>().SetValueWithoutNotify((int)(object)key);
        }
        else
        {
            _itemElements[_keys.Count - 1].Q<ObjectField>().SetValueWithoutNotify(key as UnityEngine.Object);
        }

        if (_valueType.IsEnum)
        {
            _itemElements[_keys.Count - 1].Q<EnumField>().SetValueWithoutNotify(value as Enum);
        }
        else if (_valueType == typeof(string))
        {
            _itemElements[_keys.Count - 1].Q<TextField>().SetValueWithoutNotify(value as string);
        }
        else if (_valueType == typeof(int))
        {
            _itemElements[_keys.Count - 1].Q<IntegerField>().SetValueWithoutNotify((int)(object)value);
        }
        else
        {
            _itemElements[_keys.Count - 1].Q<ObjectField>().SetValueWithoutNotify(value as UnityEngine.Object);
        }
    }

    public void ClearDictionary()
    {
        for (int i = _itemElements.Count - 1; i > -1; i--)
        {
            RemoveElement(i);
        }

        _keys.Clear();
        _values.Clear();
    }
}