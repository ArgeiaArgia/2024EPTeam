using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<StatElement, UxmlTraits>
    {
    }

    private VisualElement _icon;
    private Label _titleLabel;
    private ProgressBar _progressBar;
    private VisualElement _lineContainer;

    private int _iconIndex;

    public int IconIndex
    {
        get => _iconIndex;
        set
        {
            _iconIndex = value;
            var icon = new StyleBackground(Resources.LoadAll<Sprite>($"Sprites/StatIcon/Emoji")[value]);
            _icon.style.backgroundImage = icon;
        }
    }


    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _titleLabel.text = value;
        }
    }

    private bool _isShowValue;

    bool IsShowValue
    {
        get => _isShowValue;
        set
        {
            _isShowValue = value;
            _progressBar.title = value ? $"{Value}/{MaxValue}" : "";
        }
    }

    public int Value
    {
        get => Mathf.RoundToInt(_progressBar.value);
        set
        {
            _progressBar.value = value;
            ChangeValueText();
        }
    }

    public int MaxValue
    {
        get => Mathf.RoundToInt(_progressBar.highValue);
        set { _progressBar.highValue = value; }
    }

    public int MinValue
    {
        get => Mathf.RoundToInt(_progressBar.lowValue);
        set => _progressBar.lowValue = value;
    }

    private bool _isShowLine;

    public bool IsShowLine
    {
        get => _isShowLine;
        set
        {
            _isShowLine = value;
            _lineContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    private int _lineCount;

    public int LineCount
    {
        get => _lineCount;
        set
        {
            _lineCount = value;
            _lineContainer.Clear();
            for (int i = 0; i < value; i++)
            {
                var line = new VisualElement();
                line.AddToClassList("progress-line");
                _lineContainer.Add(line);
            }

            var empty = new VisualElement();
            empty.style.flexGrow = 1;
            _lineContainer.Add(empty);
        }
    }

    public StatElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/StatElement");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/StatElement");
        styleSheets.Add(styleSheet);

        _icon = this.Q<VisualElement>("Icon");
        _titleLabel = this.Q<Label>("Name");
        _progressBar = this.Q<ProgressBar>("ProgressBar");
        _lineContainer = new VisualElement();
        _lineContainer.AddToClassList("line-container");
        _progressBar.Q<VisualElement>(className: "unity-progress-bar__background").Add(_lineContainer);

        _lineContainer.PlaceBehind(_progressBar.Q<VisualElement>(className: "unity-progress-bar__title-container"));
    }

    private void ChangeValueText()
    {
        if (_isShowValue)
            _progressBar.title = $"{Value}/{MaxValue}";
        else
            _progressBar.title = "";
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_iconIndex = new()
            { name = "icon-index", defaultValue = 0 };

        UxmlStringAttributeDescription m_title = new()
            { name = "title", defaultValue = "Stat Element" };

        UxmlBoolAttributeDescription m_isShowValue = new()
            { name = "is-show-value", defaultValue = true };

        UxmlIntAttributeDescription m_value = new()
            { name = "value", defaultValue = 25 };

        UxmlIntAttributeDescription m_maxValue = new()
            { name = "max-value", defaultValue = 100 };

        UxmlIntAttributeDescription m_minValue = new()
            { name = "min-value", defaultValue = 0 };

        UxmlBoolAttributeDescription m_isShowLine = new()
            { name = "is-show-line", defaultValue = false };

        UxmlIntAttributeDescription m_lineCount = new()
            { name = "line-count", defaultValue = 5 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as StatElement;
            ate.IconIndex = m_iconIndex.GetValueFromBag(bag, cc);
            ate.Title = m_title.GetValueFromBag(bag, cc);
            ate.IsShowValue = m_isShowValue.GetValueFromBag(bag, cc);
            ate.Value = m_value.GetValueFromBag(bag, cc);
            ate.MaxValue = m_maxValue.GetValueFromBag(bag, cc);
            ate.MinValue = m_minValue.GetValueFromBag(bag, cc);
            ate.IsShowLine = m_isShowLine.GetValueFromBag(bag, cc);
            ate.LineCount = m_lineCount.GetValueFromBag(bag, cc);
        }
    }
}