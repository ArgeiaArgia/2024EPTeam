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
    [Header("Loading Setting")] [SerializeField]
    private int _loadingTime = 0;

    [SerializeField] private int _reducedTime = 0;

    [Header("Setting")] [FormerlySerializedAs("_inputReader")] [SerializeField]
    private InputReader inputReader;

    [FormerlySerializedAs("_inventoryManager")] [SerializeField]
    private InventoryManager inventoryManager;

    [FormerlySerializedAs("_statManager")] [SerializeField]
    private StatManager statManager;

    [FormerlySerializedAs("_itemListTemplate")] [SerializeField]
    private VisualTreeAsset itemListTemplate;

    [FormerlySerializedAs("_craftListTemplate")] [SerializeField]
    private VisualTreeAsset craftListTemplate;

    [OdinSerialize] private Dictionary<StatType, StatIcon> _statIcons;
    [OdinSerialize] private Dictionary<AbilityType, Sprite> _abilityIcons;

    #region ItemInteractions

    private TemplateContainer _itemInteractions;
    private VisualElement _itemInteractContainer;
    private bool _isMouseOverInteract;

    #endregion

    private CookUI _cookUI;

    private TabElement _tabElement;
    private VisualElement _container;

    private Dictionary<StatType, StatUI> _statUIs;
    private Dictionary<AbilityType, AbilityUI> _abilityUIs;
    private VisualElement _loadingUI;
    private CustomProgressBar _loadingBar;

    private Label _fishLabel;
    private Label _sleepLabel;

    public UnityEvent OnLoadingEnded;
    public UnityEvent<AbilityType, int> OnChangeAbilityValue;
    public UnityEvent<ItemSO> OnFoodSelected;
    public event Action<bool> OnInteracting;

    private bool _isInteracting;
    private bool _isEnabled;
    private bool _isMaking;
    private Camera mainCam;

    protected override void Awake()
    {
        base.Awake();
        _statUIs = new Dictionary<StatType, StatUI>();
        _abilityUIs = new Dictionary<AbilityType, AbilityUI>();

        inputReader.OnMovePressEvent += OnMovePress;
        statManager.OnStatChanged += ChangeStatValue;

        mainCam = Camera.main;
        inputReader.OnEscapeEvent += HandleEscapeEvent;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        _isEnabled = true;

        _container = root.Q<VisualElement>("Container");

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

            abilityUI.OnChangeStatValue += value => HandleAbilityChange(abilityType, value);
        }

        var inventory = new Inventory(root, inventoryManager, itemListTemplate, craftListTemplate, this);

        _itemInteractions = root.Q<TemplateContainer>("ItemInteractions");
        _itemInteractions.RegisterCallback<MouseOverEvent>(_ => _isMouseOverInteract = true);
        _itemInteractions.RegisterCallback<MouseOutEvent>(_ => _isMouseOverInteract = false);
        _itemInteractContainer = _itemInteractions.Q<VisualElement>("Container");

        _cookUI = new CookUI(root, this, inventoryManager);
        _cookUI.InitializeCookUI(FoodType.FirstLevelFood);

        _tabElement = root.Q<TabElement>("TabElement");

        _loadingUI = root.Q<VisualElement>("LoadingUI");
        _loadingBar = _loadingUI.Q<CustomProgressBar>();

        _loadingBar.HighValue = _loadingTime;

        _fishLabel = root.Q<Label>("FishLabel");
        _sleepLabel = root.Q<Label>("SleepLabel");
    }

    private void HandleEscapeEvent()
    {
        ShowInteractions(null);
        StopCoroutine(LoadingWaiting());
        _isMaking = false;
        _loadingUI.style.display = DisplayStyle.None;
        _cookUI.HideCookUI();
    }

    private void ChangeStatValue(StatType statType, int value)
    {
        _statUIs[statType].ChangeStatUI(value);
    }

    public void AddAbilityValue(AbilityType abilityType, int value)
    {
        _abilityUIs[abilityType].AddAbility(value);
    }

    public void AddFishAbilityCount(int count)
    {
        _abilityUIs[AbilityType.Fishing].AddAbility(count);
    }

    public void AddCookAbilityCount(int count)
    {
        _abilityUIs[AbilityType.Cooking].AddAbility(count);
    }

    public void AddRepairAbilityCount(int count)
    {
        _abilityUIs[AbilityType.Repairing].AddAbility(count);
    }

    public void ShowInteractions(List<InteractEvent> events)
    {
        if (!_isEnabled) return;
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
                ShowInteractions((List<InteractEvent>)null);
            };
            _itemInteractContainer.Add(interactButton);
        }

        var mousePos = Mouse.current.position.ReadValue();
        var localMousePos = root.WorldToLocal(new Vector2(mousePos.x, Screen.height - mousePos.y));
        _itemInteractions.style.left = localMousePos.x + 25;
        _itemInteractions.style.top = localMousePos.y + 25;
    }

    public void ShowInteractions(List<ItemInteractEvent> events, ItemSO itemSo)
    {
        if (!_isEnabled) return;
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
                interactEvent.OnInteract.Invoke(itemSo);
                ShowInteractions((List<InteractEvent>)null);
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
        if (!_isEnabled) return;
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

    public void InitializeCookUI(FoodType foodType) => _cookUI.InitializeCookUI(foodType);

    public void SetUIEnable(bool enable)
    {
        _isEnabled = enable;
        if (enable)
            _container.RemoveFromClassList("hide");
        else
            _container.AddToClassList("hide");
    }

    public void ShowCookUI() => _cookUI.ShowCookUI();
    public void HideCookUI() => _cookUI.HideCookUI();

    public void ShowLoadingUI(Vector2 pos)
    {
        _isMaking = true;
        var worldPos = mainCam.WorldToScreenPoint(pos);
        var localPos = root.WorldToLocal(new Vector2(worldPos.x, Screen.height - worldPos.y));
        _loadingUI.style.left = localPos.x - 125;
        _loadingUI.style.top = localPos.y - 25;

        _loadingUI.style.display = DisplayStyle.Flex;
        _loadingBar.Value = 0;
        StartCoroutine(LoadingWaiting());
    }

    private IEnumerator LoadingWaiting()
    {
        while (_loadingBar.Value < _loadingBar.HighValue)
        {
            _loadingBar.Value += 1;
            yield return new WaitForSeconds(1);
        }

        if (!_isMaking) yield break;
        OnLoadingEnded?.Invoke();
        _loadingUI.style.display = DisplayStyle.None;
        _tabElement.TabButtonClick(_tabElement.TabButtons[0]);
    }

    private void HandleAbilityChange(AbilityType abilityType, int value)
    {
        switch (abilityType)
        {
            case AbilityType.Cooking:
                Debug.Log("Cooking");
                _cookUI.InitializeCookUI((FoodType)value);
                break;
            case AbilityType.Repairing:
                _loadingBar.HighValue = _loadingTime - _reducedTime * value;
                _loadingBar.Value = 0;
                break;
        }

        OnChangeAbilityValue?.Invoke(abilityType, value);
    }

    public void ShowFishLabel() => _fishLabel.RemoveFromClassList("hide");
    public void HideFishLabel() => _fishLabel.AddToClassList("hide");
    public void ShowSleepLabel() => _sleepLabel.RemoveFromClassList("hide");
    public void HideSleepLabel() => _sleepLabel.AddToClassList("hide");

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