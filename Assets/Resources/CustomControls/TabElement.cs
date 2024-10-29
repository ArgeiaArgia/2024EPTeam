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

    private VisualElement _tabButtonContainer;
    private VisualElement _tabContentsContainer;

    //이름, 배경, 버튼 수
    private string _tabNames;

    public string TabNames
    {
        get => _tabNames;
        set
        {
            _tabNames = value;
            var names = _tabNames.Split(',');
            _tabNameList.AddRange(names);
            for (int i = 0; i < Mathf.Min(names.Length, _tabButtonList.Count); i++)
            {
                _tabButtonList[i].text = names[i];
            }
        }
    }

    private int _tabCount;

    public int TabCount
    {
        get => _tabCount;
        set
        {
            for (int i = _tabCount; i < value; i++)
            {
                AddTab();
            }

            for (int i = _tabCount - 1; i >= value; i--)
            {
                RemoveTab(i);
            }

            if (_tabCount > 0)
            {
                TabButtonClick(_tabButtonList[0]);
            }

            _tabCount = value;
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
    }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_tabNames = new UxmlStringAttributeDescription
            { name = "tab-names", defaultValue = "Tab1,Tab2,Tab3" };

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
        if (_tabButtonList.Count < _tabNameList.Count)
            tabButton.text = _tabNameList[_tabButtonList.Count - 1];
        else
            tabButton.text = $"Tab{_tabButtonList.Count + 1}";
        tabButton.AddToClassList("tab-button");
        _tabButtonContainer.Add(tabButton);

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