using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CookUI
{
    private InGameUI _inGameUI;
    private List<ItemSO> _foodList;

    private TemplateContainer _cookList;
    private VisualElement _foodScrollView;

    public CookUI(VisualElement root, InGameUI inGameUI, InventoryManager inventoryManager)
    {
        _inGameUI = inGameUI;
        _foodList = inventoryManager.ItemListSO.ItemList.FindAll(item => item.itemType == ItemType.Food);

        _cookList = root.Q<TemplateContainer>("CookUI");
        _foodScrollView = _cookList.Q<VisualElement>("unity-content-container");
    }

    public void InitializeCookUI(FoodType foodType)
    {
        _foodScrollView.Clear();
        for (var i = 0; i <= (int)foodType; i++)
        {
            var foods = _foodList.FindAll(x => x.foodType == (FoodType)i);
            foreach (var food in foods)
            {
                var foodElement = new CraftElement()
                {
                    CurrentItem = food
                };
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
        _cookList.RemoveFromClassList("hide");
    }
    public void HideCookUI()
    {
        _cookList.AddToClassList("hide");
    }
}