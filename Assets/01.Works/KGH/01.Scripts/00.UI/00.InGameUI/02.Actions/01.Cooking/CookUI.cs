using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CookUI
{
    private InventoryManager _inventoryManager;
    private InGameUI _inGameUI;
    private List<ItemSO> _foodList;

    private TemplateContainer _cookList;
    private VisualElement _foodScrollView;

    private FoodType _currentFoodType;
    public CookUI(VisualElement root, InGameUI inGameUI, InventoryManager inventoryManager)
    {
        _inGameUI = inGameUI;
        _foodList = inventoryManager.ItemListSO.ItemList.FindAll(item => item.itemType == ItemType.Food);
        _inventoryManager = inventoryManager;

        _cookList = root.Q<TemplateContainer>("CookUI");
        _foodScrollView = _cookList.Q<VisualElement>("unity-content-container");
    }

    public void InitializeCookUI(FoodType foodType)
    {
        _currentFoodType = foodType;
        InitializeCookUI();
    }

    void InitializeCookUI()
    {
        _foodScrollView.Clear();
        for (var i = 0; i <= (int)_currentFoodType; i++)
        {
            var foods = _foodList.FindAll(x => x.foodType == (FoodType)i);
            foreach (var food in foods)
            {
                var foodElement = new CraftElement()
                {
                    CurrentItem = food
                };
                if (!_inventoryManager.CheckIfMakeable(food, out var lackItems))
                {
                    foodElement.Q<Button>("CraftItem").AddToClassList("lack");
                    foreach (var itemSo in lackItems)
                    {
                        foodElement.Q<VisualElement>(className:"required-icon").AddToClassList("lack");
                        foodElement.Q<Label>(className:"required-text").AddToClassList("lack");
                    }

                    foodElement.Q<Button>("CraftButton").pickingMode = PickingMode.Ignore;
                }

                foodElement.OnCreateItem += (e)=>
                {
                    _inGameUI.OnFoodSelected.Invoke(e);
                    HideCookUI();
                };
                _foodScrollView.Add(foodElement);
            }
        }
    }
    public void ShowCookUI()
    {
        InitializeCookUI();
        _cookList.RemoveFromClassList("hide");
    }
    public void HideCookUI()
    {
        _cookList.AddToClassList("hide");
    }
}