using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class InGameUI : ToolkitParents
{
    [FormerlySerializedAs("_inputReader")] [SerializeField] private InputReader inputReader;
    [FormerlySerializedAs("_inventoryManager")] [SerializeField] private InventoryManager inventoryManager;
    [FormerlySerializedAs("_statManager")] [SerializeField] private StatManager statManager;
    [FormerlySerializedAs("_itemListTemplate")] [SerializeField] private VisualTreeAsset itemListTemplate;
    [FormerlySerializedAs("_craftListTemplate")] [SerializeField] private VisualTreeAsset craftListTemplate;
    [OdinSerialize] private Dictionary<StatType, StatIcon> _statIcons;
    [OdinSerialize] private Dictionary<AbilityType, Sprite> _abilityIcons;

    #region ItemInteractions

    private TemplateContainer _itemInteractions;
    private VisualElement _itemInteractContainer;
    private bool _isMouseOverInteract;

    #endregion

    private Dictionary<StatType, StatUI> _statUIs;
    private Dictionary<AbilityType, AbilityUI> _abilityUIs;

    public UnityEvent<AbilityType, int> OnChangeAbilityValue;
    public event Action<bool> OnInteracting;

    private bool _isInteracting;

    protected override void Awake()
    {
        base.Awake();
        _statUIs = new Dictionary<StatType, StatUI>();
        _abilityUIs = new Dictionary<AbilityType, AbilityUI>();

        inputReader.OnMovePressEvent += OnMovePress;
        statManager.OnStatChanged += ChangeStatValue;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        _statUIs.Clear();
        var statElements = root.Q<VisualElement>("StatList").Query<StatElement>().ToList();
        for (var i = 0; i < statElements.Count; i++)
        {
            var statElement = statElements[i];
            var statType = (StatType)i;
            var statUI = new StatUI(statElement, _statIcons[statType]);
            _statUIs.Add(statType, statUI);
        }

        var abilityElements = root.Q<VisualElement>("AbilityList").Query<StatElement>().ToList();
        for (var i = 0; i < abilityElements.Count; i++)
        {
            var abilityElement = abilityElements[i];
            var abilityType = (AbilityType)i;
            var abilityUI = new AbilityUI(abilityElement);
            _abilityUIs.Add(abilityType, abilityUI);

            abilityUI.OnChangeStatValue += value => OnChangeAbilityValue?.Invoke(abilityType, value);
        }

        var inventory = new Inventory(root, inventoryManager, itemListTemplate, craftListTemplate, this);

        _itemInteractions = root.Q<TemplateContainer>("ItemInteractions");
        _itemInteractions.RegisterCallback<MouseOverEvent>(_ => _isMouseOverInteract = true);
        _itemInteractions.RegisterCallback<MouseOutEvent>(_ => _isMouseOverInteract = false);
        _itemInteractContainer = _itemInteractions.Q<VisualElement>("Container");
    }

    private void ChangeStatValue(StatType statType, int value) => _statUIs[statType].ChangeStatUI(value);
    public void AddAbilityValue(AbilityType abilityType, int value) => _abilityUIs[abilityType].AddAbility(value);

    public void ShowInteractions(List<InteractEvent> events)
    {
        if (events == null)
        {
            OnInteracting?.Invoke(false);
            _itemInteractions.style.display = DisplayStyle.None;
            return;
        }

        _isInteracting = true;
        OnInteracting?.Invoke(true);
        _itemInteractions.style.display = DisplayStyle.Flex;

        _itemInteractContainer.Clear();
        foreach (var interactEvent in events)
        {
            var interactButton = new Button { text = interactEvent.EventName };
            interactButton.AddToClassList("interact-button");
            interactButton.clicked += () =>
            {
                interactEvent.OnInteract.Invoke();
                ShowInteractions(null);
            };
            _itemInteractContainer.Add(interactButton);
        }

        var mousePos = Mouse.current.position.ReadValue();
        var localMousePos = root.WorldToLocal(new Vector2(mousePos.x, Screen.height - mousePos.y));
        _itemInteractions.style.left = localMousePos.x + 25;
        _itemInteractions.style.top = localMousePos.y + 25;
    }

    public void ShowInteractions(List<InteractEvent> events, Vector2 pos)
    {
        if (events == null)
        {
            OnInteracting?.Invoke(false);
            _itemInteractions.style.display = DisplayStyle.None;
            return;
        }

        _isInteracting = true;
        OnInteracting?.Invoke(true);
        _itemInteractions.style.display = DisplayStyle.Flex;

        _itemInteractContainer.Clear();
        foreach (var interactEvent in events)
        {
            var interactButton = new Button { text = interactEvent.EventName };
            interactButton.AddToClassList("interact-button");
            interactButton.clicked += () =>
            {
                interactEvent.OnInteract.Invoke();
                ShowInteractions(null);
            };
            _itemInteractContainer.Add(interactButton);
        }

        var localMousePos = root.WorldToLocal(new Vector2(pos.x, Screen.height - pos.y));
        _itemInteractions.style.left = localMousePos.x + 25;
        _itemInteractions.style.top = localMousePos.y + 25;
    }
    
    private void OnMovePress(Vector2 obj)
    {
        if (_isMouseOverInteract || !_isInteracting) return;
        ShowInteractions(null);
    }

    public void CoroutineHelper(IEnumerator coroutine) => StartCoroutine(coroutine);
}

public enum StatType
{
    Hunger,
    Thirst,
    Tired,
    Bored,
    Health,
}

public enum AbilityType
{
    Fishing,
    Cooking,
    Repairing
}