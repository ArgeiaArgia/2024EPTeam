using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class ItemInspector
{
    private ItemSOWindow _itemSOWindow;
    private ItemSO _currentItem;

    private string _descPath;

    private ScrollView _itemInspector;

    private Button _confirmButton;

    #region Preview

    private VisualElement _iconPreview;
    private VisualElement _spritePreview;
    private ObjectField _iconField;
    private ObjectField _spriteField;

    #endregion

    #region Name

    private TextField _itemName;
    private Button _changeNameButton;

    #endregion

    private EnumField _itemType;
    private Slider _percentSlider;
    private TextField _descField;

    #region Additional

    private VisualElement _additionalContent;
    private EditorList _materailList;
    private EnumField _toolType;
    private IMGUIContainer _effectList;
    private EnumField _foodType;

    #endregion

    public event Action<ItemSO, string> OnNameChange;
    public event Action<ItemSO, ItemType> OnTypeChange;
    public event Action<ItemSO> OnConfirm;

    public ItemInspector(VisualElement content, ItemSOWindow itemSOWindow)
    {
        
        _itemSOWindow = itemSOWindow;
        _currentItem = null;

        _descPath = Path.Combine(Application.dataPath, "ItemDescriptions.json");

        _itemInspector = content.Q<ScrollView>("ItemInspector");
        _itemInspector.style.display = DisplayStyle.None;

        _iconPreview = content.Q<VisualElement>("IconImage");
        _spritePreview = content.Q<VisualElement>("SpriteImage");
        _iconField = content.Q<ObjectField>("IconField");
        _spriteField = content.Q<ObjectField>("SpriteField");
        _iconField.RegisterCallback<ChangeEvent<Object>>((e) => HandleChangePreview(e, _iconField, _iconPreview));
        _spriteField.RegisterCallback<ChangeEvent<Object>>((e) => HandleChangePreview(e, _spriteField, _spritePreview));

        _itemName = content.Q<TextField>("NameField");
        _changeNameButton = content.Q<Button>("ChangeNameButton");
        _changeNameButton.clicked += HandleChangeName;

        _itemType = content.Q<EnumField>("ItemType");
        _itemType.RegisterCallback<ChangeEvent<Enum>>((e) => HandleChangeType(e.newValue));

        _percentSlider = content.Q<Slider>("Percentage");
        _percentSlider.RegisterValueChangedCallback((e) => HandleChangePercentage(e.newValue));

        _descField = content.Q<TextField>("Description");
        _descField.RegisterValueChangedCallback((e) => HandleChangeDescription(e.newValue));

        _additionalContent = content.Q<VisualElement>("Additional");
        _materailList = content.Q<EditorList>("MaterialList");
        _toolType = content.Q<EnumField>("ToolType");
        _effectList = content.Q<IMGUIContainer>("StatEffectDictionary");
        _foodType = content.Q<EnumField>("FoodType");
        
        _materailList.OnListChanged += HandleMaterialListChanged;
        _toolType.RegisterValueChangedCallback((e)=>HandleChangeToolType(e.newValue));
        _effectList.onGUIHandler += HandleEffectGUI;
        _foodType.RegisterCallback<ChangeEvent<Enum>>((e) => HandleChangeFoodType(e.newValue));
        
        _confirmButton = content.Q<Button>("ConfirmButton");
        _confirmButton.style.display = DisplayStyle.None;
        _confirmButton.clicked += () => OnConfirm?.Invoke(_currentItem);
    }

    

    private void HandleEffectGUI()
    {
    }

    private void HandleChangePreview(ChangeEvent<Object> evt, ObjectField field, VisualElement preview)
    {
        if (_currentItem == null) return;
        var newSprite = evt.newValue as Sprite;
        if (newSprite != null)
        {
            var texture = new StyleBackground(newSprite);
            preview.style.backgroundImage = texture;
        }
        else
        {
            preview.style.backgroundImage = null;
        }

        if (field == _iconField) _currentItem.itemIcon = newSprite;
        else if (field == _spriteField) _currentItem.itemSprite = newSprite;

        EditorUtility.SetDirty(_currentItem);
    }
    private void HandleChangeName()
    {
        if (_currentItem == null) return;
        if (string.IsNullOrEmpty(_itemName.value.Trim())) return;

        string newName = _itemName.value;
        OnNameChange?.Invoke(_currentItem, newName);
    }
    private void HandleChangeType(Enum evtNewValue)
    {
        if (_currentItem == null || evtNewValue is not ItemType type) return;
        _currentItem.itemType = type;

        OnTypeChange?.Invoke(_currentItem, type);

        _currentItem.materialList.Clear();
        _currentItem.StatEffect.Clear();
        _currentItem.toolType = ToolType.FishingRod;
        _currentItem.foodType = FoodType.FirstLevelFood;
        switch (type)
        {
            case ItemType.Material:
                _additionalContent.style.display = DisplayStyle.None;
                break;
            case ItemType.Tool:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materailList.style.display = DisplayStyle.Flex;
                _toolType.style.display = DisplayStyle.Flex;
                _effectList.style.display = DisplayStyle.None;
                _foodType.style.display = DisplayStyle.None;
                break;
            case ItemType.Fish:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materailList.style.display = DisplayStyle.None;
                _toolType.style.display = DisplayStyle.None;
                _effectList.style.display = DisplayStyle.Flex;
                _foodType.style.display = DisplayStyle.None;
                break;
            case ItemType.Food:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materailList.style.display = DisplayStyle.Flex;
                _toolType.style.display = DisplayStyle.None;
                _effectList.style.display = DisplayStyle.Flex;
                _foodType.style.display = DisplayStyle.Flex;
                break;
        }
    }
    private void HandleChangePercentage(float evtNewValue)
    {
        if (_currentItem == null) return;
        _currentItem.percentageOfCatch = evtNewValue;
        EditorUtility.SetDirty(_currentItem);
    }
    private void HandleChangeDescription(string evtNewValue)
    {
        if (_currentItem == null) return;
        try
        {
            var descList = JsonUtility.FromJson<DescList>(_descPath);
            descList.descList[_currentItem.itemNumber] = evtNewValue;
            File.WriteAllText(_descPath, JsonUtility.ToJson(descList, true));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    private void HandleMaterialListChanged(IList obj)
    {
        if (_currentItem == null) return;
        _currentItem.materialList = obj as List<ItemSO>;
        EditorUtility.SetDirty(_currentItem);
    }
    private void HandleChangeToolType(Enum evtNewValue)
    {
        if (_currentItem == null || _currentItem.itemType != ItemType.Tool || evtNewValue is not ToolType) return;
        _currentItem.toolType = (ToolType) evtNewValue;
        EditorUtility.SetDirty(_currentItem);
    }
    
    private void HandleChangeFoodType(Enum evtNewValue)
    {
        if(_currentItem == null || _currentItem.itemType != ItemType.Food || evtNewValue is not FoodType) return;
        _currentItem.foodType = (FoodType) evtNewValue;
        EditorUtility.SetDirty(_currentItem);
    }

}