using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ItemElement, UxmlTraits>
    {
    }

    private VisualElement _icon;
    private Label _nameLabel;
    private Label _categoryLabel;

    private Sprite _currentIcon;

    public Sprite CurrentIcon
    {
        get => _currentIcon;
        set
        {
            _currentIcon = value;
            _icon.style.backgroundImage = new StyleBackground(_currentIcon);
        }
    }

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            if (_value > 1)
                _nameLabel.text = $"{_name}({_value})";
            else
                _nameLabel.text = _name;
        }
    }

    private int _value;

    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            if (_value > 1)
                _nameLabel.text = $"{_name}({_value})";
            else
                _nameLabel.text = _name;
        }
    }

    private string _category;

    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            _categoryLabel.text = _category;
        }
    }

    public ItemElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/ItemElement");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/ItemElement");
        styleSheets.Add(styleSheet);

        _icon = this.Q<VisualElement>("Icon");
        _nameLabel = this.Q<Label>("NameLabel");
        _categoryLabel = this.Q<Label>("CategoryLabel");
    }
}