using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class ItemInspector
{
    private ItemSOWindow _itemSOWindow;
    private ItemSO _currentItem;
    private ScrollView _itemInspector;

    private readonly Button _confirmButton;
    private VisualElement _iconPreview;
    private VisualElement _spritePreview;
    private readonly ObjectField _iconField;
    private readonly ObjectField _spriteField;
    private readonly TextField _itemName;
    private readonly Button _changeNameButton;
    private EnumField _itemType;
    private Slider _percentSlider;
    private Slider _weightSlider;
    private TextField _descField;
    private readonly VisualElement _additionalContent;
    private readonly EditorList _materialList;
    private readonly EnumField _toolType;
    private readonly SliderInt _slotSlider;
    private readonly EditorDictionary _effectList;
    private readonly EnumField _foodType;

    public event Action<ItemSO, string> OnNameChange;
    public event Action<ItemSO, ItemType> OnTypeChange;
    public event Action<ItemSO, Sprite> OnIconChange;

    public ItemInspector(VisualElement content, ItemSOWindow itemSOWindow)
    {
        _itemSOWindow = itemSOWindow;
        _currentItem = null;

        _itemInspector = content.Q<ScrollView>("ItemInspector");
        _itemInspector.style.display = DisplayStyle.None;

        _iconPreview = content.Q<VisualElement>("IconImage");
        _spritePreview = content.Q<VisualElement>("SpriteImage");
        _iconField = content.Q<ObjectField>("IconField");
        _spriteField = content.Q<ObjectField>("SpriteField");
        _iconField.RegisterValueChangedCallback((e) => HandleChangePreview(e, _iconField, _iconPreview));
        _spriteField.RegisterValueChangedCallback((e) => HandleChangePreview(e, _spriteField, _spritePreview));

        _itemName = content.Q<TextField>("NameField");
        _changeNameButton = content.Q<Button>("ChangeNameButton");
        _changeNameButton.clicked += HandleChangeName;

        _itemType = content.Q<EnumField>("ItemType");
        _itemType.RegisterCallback<ChangeEvent<Enum>>((e) => HandleChangeType(e.newValue));

        _percentSlider = content.Q<Slider>("Percentage");
        _percentSlider.RegisterValueChangedCallback((e) => HandleChangePercentage(e.newValue));

        _weightSlider = content.Q<Slider>("Weight");
        _weightSlider.RegisterValueChangedCallback((e) => HandleChangeWeight(e.newValue));
        
        _descField = content.Q<TextField>("Description");
        _descField.RegisterValueChangedCallback((e) => HandleChangeDescription(e.newValue));

        _confirmButton = content.Q<Button>("ConfirmButton");
        _confirmButton.clicked += ()=>EditorUtility.SetDirty(_currentItem);
        
        _additionalContent = content.Q<VisualElement>("Additional");
        _materialList = content.Q<EditorList>("MaterialList");
        _toolType = content.Q<EnumField>("ToolType");
        _slotSlider = content.Q<SliderInt>("SlotSlider");
        _effectList = content.Q<EditorDictionary>("StatEffectDictionary");
        _foodType = content.Q<EnumField>("FoodType");
        
        _materialList.OnListChanged += HandleMaterialListChanged;
        _toolType.RegisterValueChangedCallback((e) => HandleChangeToolType(e.newValue));
        _slotSlider.RegisterValueChangedCallback((e) => HandleChangeSlotCount(e.newValue));
        _effectList.OnDictionaryChanged += HandleEffectDictionaryChanged;
        _foodType.RegisterCallback<ChangeEvent<Enum>>((e) => HandleChangeFoodType(e.newValue));
    }

    private void HandleChangeWeight(float evtNewValue)
    {
        if (_currentItem == null) return;
        _currentItem.weight = evtNewValue;
    }

    private void HandleChangeSlotCount(float evtNewValue)
    {
        if (_currentItem == null || _currentItem.toolType != ToolType.Inventory) return;
        _currentItem.slotCount = (int)evtNewValue;
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

        if (field == _iconField)
        {
            _currentItem.itemIcon = newSprite;
            OnIconChange?.Invoke(_currentItem, newSprite);
        }
        else if (field == _spriteField) _currentItem.itemSprite = newSprite;
    }

    private void HandleChangeName()
    {
        if (_currentItem == null) return;
        if (string.IsNullOrEmpty(_itemName.value.Trim())) return;

        string newName = _itemName.value;
        _currentItem.itemName = newName;
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
        
        ShowItemType(type);
    }

    private void HandleChangePercentage(float evtNewValue)
    {
        if (_currentItem == null) return;
        _currentItem.percentageOfCatch = evtNewValue;
    }

    private void HandleChangeDescription(string evtNewValue)
    {
        if (_currentItem == null) return;
        _currentItem.description = evtNewValue;
    }

    private void HandleMaterialListChanged(IList obj)
    {
        if (_currentItem == null || (_currentItem.itemType != ItemType.Food && _currentItem.itemType != ItemType.Tool))
            return;
        _currentItem.materialList = obj as List<ItemSO>;
    }

    private void HandleChangeToolType(Enum evtNewValue)
    {
        if (_currentItem == null || _currentItem.itemType != ItemType.Tool ||
            evtNewValue is not ToolType toolType) return;
        _currentItem.toolType = toolType;
        ShowToolType(toolType);
    }

    private void HandleEffectDictionaryChanged(IDictionary obj)
    {
        if (_currentItem == null ||
            !(_currentItem.itemType == ItemType.Ingredient || _currentItem.itemType == ItemType.Food) ||
            obj is not Dictionary<StatType, int> dictionary) return;
        _currentItem.StatEffect = dictionary;
    }

    private void HandleChangeFoodType(Enum evtNewValue)
    {
        if (_currentItem == null || _currentItem.itemType != ItemType.Food ||
            evtNewValue is not FoodType foodType) return;
        _currentItem.foodType = foodType;
    }

    public void ChangeItem(ItemSO item)
    {
        if (_currentItem != null && (_currentItem.itemType == ItemType.Ingredient || _currentItem.itemType == ItemType.Food))
        {
            _currentItem.StatEffect = (Dictionary<StatType, int>)_effectList.GetDictionary();
        }

        _currentItem = item;

        if (_currentItem == null)
        {
            _itemInspector.style.display = DisplayStyle.None;
            return;
        }

        _itemInspector.style.display = DisplayStyle.Flex;

        _iconField.value = item.itemIcon;
        _spriteField.value = item.itemSprite;
        _itemName.SetValueWithoutNotify(item.itemName);
        _itemType.SetValueWithoutNotify(item.itemType);
        _percentSlider.SetValueWithoutNotify(item.percentageOfCatch);
        _weightSlider.SetValueWithoutNotify(item.weight);
        _toolType.SetValueWithoutNotify(item.toolType);
        _slotSlider.SetValueWithoutNotify(item.slotCount);
        _descField.SetValueWithoutNotify(item.description);
        _foodType.SetValueWithoutNotify(item.foodType);
        
        ShowItemType(item.itemType);
        ShowToolType(item.toolType);
        
        switch (_currentItem.toolType)
        {
            case ToolType.Inventory:
                _slotSlider.style.display = DisplayStyle.Flex;
                break;
            default:
                _slotSlider.style.display = DisplayStyle.None;
                break;
        }
        _materialList.ClearList();
        _effectList.ClearDictionary();
        foreach (var mat in item.materialList)
        {
            _materialList.AddList(mat);
        }

        foreach (var eff in item.StatEffect)
        {
            _effectList.AddDictionaryItem(eff.Key, eff.Value);
        }
    }

    void ShowItemType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Trash:
                _additionalContent.style.display = DisplayStyle.None;
                break;
            case ItemType.Tool:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materialList.style.display = DisplayStyle.Flex;
                _toolType.style.display = DisplayStyle.Flex;
                _effectList.style.display = DisplayStyle.None;
                _foodType.style.display = DisplayStyle.None;
                break;
            case ItemType.Ingredient:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materialList.style.display = DisplayStyle.None;
                _toolType.style.display = DisplayStyle.None;
                _effectList.style.display = DisplayStyle.Flex;
                _foodType.style.display = DisplayStyle.None;
                break;
            case ItemType.Food:
                _additionalContent.style.display = DisplayStyle.Flex;
                _materialList.style.display = DisplayStyle.Flex;
                _toolType.style.display = DisplayStyle.None;
                _effectList.style.display = DisplayStyle.Flex;
                _foodType.style.display = DisplayStyle.Flex;
                break;
        }
    }

    void ShowToolType(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Inventory:
                _slotSlider.style.display = DisplayStyle.Flex;
                break;
            default:
                _slotSlider.style.display = DisplayStyle.None;
                break;
        }
    }
}