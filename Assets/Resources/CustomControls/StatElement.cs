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

    private string _iconName;

    public string IconName
    {
        get => _iconName;
        set
        {
            _iconName = value;
            if (string.IsNullOrWhiteSpace(value))
            {
                _icon.style.backgroundImage = null;
            }
            else
            {
                var icon = new StyleBackground(Resources.Load<Sprite>($"Sprites/{_iconName}"));
                _icon.style.backgroundImage = icon;
            }
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

    public StatElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/StatElement");
        visualTree.CloneTree(this);

        var styleSheet = Resources.Load<StyleSheet>("CustomControls/StatElement");
        styleSheets.Add(styleSheet);

        _icon = this.Q<VisualElement>("Icon");
        _titleLabel = this.Q<Label>("Name");
        _progressBar = this.Q<ProgressBar>("ProgressBar");
    }

    private void ChangeValueText()
    {
        _progressBar.title = $"{Value}/{MaxValue}";
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_iconName = new()
            { name = "icon-name", defaultValue = "" };

        UxmlStringAttributeDescription m_title = new()
            { name = "title", defaultValue = "Stat Element" };

        UxmlIntAttributeDescription m_value = new()
            { name = "value", defaultValue = 25 };

        UxmlIntAttributeDescription m_maxValue = new()
            { name = "max-value", defaultValue = 100 };

        UxmlIntAttributeDescription m_minValue = new()
            { name = "min-value", defaultValue = 0 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as StatElement;
            ate.IconName = m_iconName.GetValueFromBag(bag, cc);
            ate.Title = m_title.GetValueFromBag(bag, cc);
            ate.Value = m_value.GetValueFromBag(bag, cc);
            ate.MaxValue = m_maxValue.GetValueFromBag(bag, cc);
            ate.MinValue = m_minValue.GetValueFromBag(bag, cc);
        }
    }
}