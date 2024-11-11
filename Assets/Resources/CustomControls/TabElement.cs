using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TabElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<TabElement, UxmlTraits>
    {
    }

    private readonly VisualElement _tabButtonContainer;
    private readonly VisualElement _leftTabButtonContainer;
    private readonly VisualElement _bottomTabButtonContainer;
    private VisualElement _tabContentsContainer;

    //이름, 배경, 버튼 수
    private string _tabNames;

    public string TabNames
    {
        get => _tabNames;
        set
        {
            _tabNames = value;
            _tabNameList.Clear();
            var names = _tabNames.Split(',');
            _tabNameList.AddRange(names);
            for (int i = 0; i < Mathf.Min(names.Length, _tabButtonList.Count); i++)
            {
                _tabButtonList[i].text = names[i];
            }
        }
    }

    private TabPositionType _tabPositionType;

    public TabPositionType TabPositionType
    {
        get => _tabPositionType;
        set
        {
            _tabPositionType = value;
            switch (_tabPositionType)
            {
                case TabPositionType.Right:
                    for (var i = _leftTabButtonContainer.childCount - 1; i > -1; i--)
                    {
                        var btn = _leftTabButtonContainer.ElementAt(i);
                        _tabButtonContainer.Add(btn);
                        btn.RemoveFromClassList("left");
                        btn.RemoveFromClassList("bottom");
                    }

                    break;
                case TabPositionType.Left:
                    for (var i = _tabButtonContainer.childCount - 1; i > -1; i--)
                    {
                        var btn = _tabButtonContainer.ElementAt(i);
                        _leftTabButtonContainer.Add(btn);
                        btn.AddToClassList("left");
                        btn.RemoveFromClassList("bottom");
                    }

                    break;
                case TabPositionType.Bottom:
                    for (var i = _tabButtonContainer.childCount - 1; i > -1; i--)
                    {
                        var btn = _tabButtonContainer.ElementAt(i);
                        _bottomTabButtonContainer.Add(btn);
                        btn.RemoveFromClassList("left");
                        btn.AddToClassList("bottom");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private int _tabCount;

    public int TabCount
    {
        get => _tabCount;
        set
        {
            if (value < _tabCount)
            {
                for (int i = _tabCount - 1; i >= value; i--)
                {
                    RemoveTab(i);
                }
            }
            else if (value > _tabCount)
            {
                for (int i = _tabCount; i < value; i++)
                {
                    AddTab();
                }
            }
            
            _tabCount = value;

            // Select the first tab if available
            if (_tabCount > 0)
            {
                TabButtonClick(_tabButtonList[0]);
            }
        }
    }

    private readonly List<string> _tabNameList = new List<string>();
    private readonly List<Button> _tabButtonList = new List<Button>();
    public override VisualElement contentContainer => _tabContentsContainer;

    public TabElement()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("CustomControls/TabElement");
        visualTree.CloneTree(this);
        var styleSheet = Resources.Load<StyleSheet>("CustomControls/TabElement");
        styleSheets.Add(styleSheet);

        _tabButtonContainer = this.Q<VisualElement>("TabButtons");
        _tabContentsContainer = this.Q<VisualElement>("TabContents");
        _leftTabButtonContainer = this.Q<VisualElement>("LeftTabButtons");
        _bottomTabButtonContainer = this.Q<VisualElement>("BottomTabButtons");
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_tabNames = new UxmlStringAttributeDescription
            { name = "tab-names", defaultValue = "Tab1,Tab2,Tab3" };

        UxmlEnumAttributeDescription<TabPositionType> m_tabPositionType =
            new UxmlEnumAttributeDescription<TabPositionType>
                { name = "tab-position-type", defaultValue = TabPositionType.Right };

        UxmlIntAttributeDescription m_tabCount = new UxmlIntAttributeDescription
            { name = "tab-count", defaultValue = 3 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            TabElement ate = ve as TabElement;
            ate.TabNames = m_tabNames.GetValueFromBag(bag, cc);
            ate.TabPositionType = m_tabPositionType.GetValueFromBag(bag, cc);
            ate.TabCount = Mathf.Clamp(m_tabCount.GetValueFromBag(bag, cc), 0, 10);
            ate._tabContentsContainer = ate.contentContainer;

            if (ate.TabCount > 0)
            {
                ate.TabButtonClick(ate._tabButtonList[0]);
            }
        }
    }

    private void AddTab()
    {
        var tabButton = new Button();
        _tabButtonList.Add(tabButton);
        if (_tabButtonList.Count <= _tabNameList.Count)
            tabButton.text = _tabNameList[_tabButtonList.Count - 1];
        else
            tabButton.text = $"Tab{_tabButtonList.Count}";
        tabButton.AddToClassList("tab-button");

        switch (_tabPositionType)
        {
            case TabPositionType.Right:
                _tabButtonContainer.Add(tabButton);
                break;
            case TabPositionType.Left:
                _leftTabButtonContainer.Add(tabButton);
                tabButton.AddToClassList("left");
                break;
            case TabPositionType.Bottom:
                _bottomTabButtonContainer.Add(tabButton);
                tabButton.AddToClassList("bottom");
                break;
        }

        tabButton.clickable.clicked += () => { TabButtonClick(tabButton); };
    }

    private void TabButtonClick(Button tab)
    {
        foreach (var contents in _tabContentsContainer.Children())
        {
            contents.style.display = DisplayStyle.None;
        }

        foreach (var button in _tabButtonList)
        {
            if (button == tab)
                button.AddToClassList("clicked");
            else
                button.RemoveFromClassList("clicked");
        }

        try
        {
            var contents = _tabContentsContainer.Children().ElementAt(_tabButtonList.IndexOf(tab));
            contents.style.display = DisplayStyle.Flex;
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }
    }

    private void RemoveTab(int index)
    {
        if (index < 0 || index >= _tabButtonList.Count)
            return;
        var btn = _tabButtonList[index];
        _tabButtonList.Remove(btn);
        btn.RemoveFromHierarchy();

        try
        {
            var contents = _tabContentsContainer.Children().ElementAt(index);
            contents.RemoveFromHierarchy();
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("TabContents is null!! Add more container to it");
            return;
        }
    }
}

public enum TabPositionType
{
    Right,
    Left,
    Bottom
}