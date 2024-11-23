using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomProgressBar : VisualElement
{
    public new class UxmlFactory : UxmlFactory<CustomProgressBar, UxmlTraits> { }
    
    private ProgressBar _progressBar;
    private readonly VisualElement _lineContainer;

    private int _lowValue;
    public int LowValue
    {
        get => _lowValue;
        set
        {
            _lowValue = value;
            _progressBar.lowValue = value;
        }
    }

    private int _highValue;
    public int HighValue
    {
        get => _highValue;
        set
        {
            _highValue = value;
            _progressBar.highValue = value;
        }
    }

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _progressBar.value = value;
        }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _progressBar.title = value;
        }
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
    
    public CustomProgressBar()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/CustomProgressBar");
        visualTree.CloneTree(this);
        
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/CustomProgressBar");
        styleSheets.Add(styleSheet);

        _progressBar = this.Q<ProgressBar>();
        
        _lineContainer = new VisualElement();
        _lineContainer.AddToClassList("line-container");
        _progressBar.Q<VisualElement>(className: "unity-progress-bar__background").Add(_lineContainer);

        _lineContainer.PlaceBehind(_progressBar.Q<VisualElement>(className: "unity-progress-bar__title-container"));
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_lowValue = new UxmlIntAttributeDescription { name = "low-value" };
        UxmlIntAttributeDescription m_highValue = new UxmlIntAttributeDescription { name = "high-value" };
        UxmlIntAttributeDescription m_value = new UxmlIntAttributeDescription { name = "value" };
        UxmlStringAttributeDescription m_title = new UxmlStringAttributeDescription { name = "title" };
        UxmlBoolAttributeDescription m_isShowLine = new UxmlBoolAttributeDescription { name = "is-show-line" };
        UxmlIntAttributeDescription m_lineCount = new UxmlIntAttributeDescription { name = "line-count" };
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as CustomProgressBar;
            ate.LowValue = m_lowValue.GetValueFromBag(bag, cc);
            ate.HighValue = m_highValue.GetValueFromBag(bag, cc);
            ate.Value = m_value.GetValueFromBag(bag, cc);
            ate.Title = m_title.GetValueFromBag(bag, cc);
            ate.IsShowLine = m_isShowLine.GetValueFromBag(bag, cc);
            ate.LineCount = m_lineCount.GetValueFromBag(bag, cc);
        }
    }
}
