using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FishMiniGameUI : ToolkitParents
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private ItemListSO itemList;

    [Header("fishing setting")] [SerializeField]
    private List<Sprite> fishSprites;

    [SerializeField] private List<Sprite> trashSprites;
    [SerializeField] private int tileCount;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private int arrowRepeat;

    [OdinSerialize] private Dictionary<ItemSO, int> _inventoryItems;

    public float ArrowSpeed => arrowSpeed;
    public int ArrowRepeat => arrowRepeat;

    private VisualElement _tileContainer;
    private VisualElement _tiles;

    private List<VisualElement> _tileList = new List<VisualElement>();
    private Dictionary<VisualElement, int> _fishTiles = new Dictionary<VisualElement, int>();
    private Dictionary<VisualElement, int> _trashTiles = new Dictionary<VisualElement, int>();

    private VisualElement _arrow;
    private int _arrowMoveCount;
    private float _arrowPos;

    private bool _isEnabled;

    private List<ItemSO> _fishItems = new List<ItemSO>();
    private List<ItemSO> _trashItems = new List<ItemSO>();
    private float _fishItemPercentage;
    private float _trashItemPercentage;

    private Dictionary<FishTileType, int> _percentage = new Dictionary<FishTileType, int>
    {
        { FishTileType.FishTile, 0 },
        { FishTileType.TrashTile, 0 },
        { FishTileType.EmptyTile, 0 }
    };

    [FormerlySerializedAs("OnFishEnd")] public UnityEvent<ItemSO, int> OnItemFishEnd;
    public UnityEvent<InventoryItem, int> OnInventoryFishEnd;

    protected override void Awake()
    {
        base.Awake();
        _fishItems = itemList.ItemList.FindAll(x => x.itemType == ItemType.Ingredient);
        _trashItems = itemList.ItemList.FindAll(x => x.itemType is ItemType.Trash or ItemType.Tool);
        foreach (var item in _fishItems)
        {
            _fishItemPercentage += item.percentageOfCatch;
        }

        foreach (var item in _trashItems)
        {
            _trashItemPercentage += item.percentageOfCatch;
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        _tileContainer = root.Q<VisualElement>("TileContainer");
        _tiles = root.Q<VisualElement>("Tiles");
        _arrow = root.Q<VisualElement>("Arrow");
    }

    public override void EnableUI()
    {
        base.EnableUI();
        _tileContainer.RemoveFromClassList("hide");
        inputReader.OnFishEvent += CatchFish;

        _percentage[FishTileType.FishTile] = 1;
        _percentage[FishTileType.TrashTile] = 1;
        _percentage[FishTileType.EmptyTile] = 1;

        _isEnabled = true;
        _arrowMoveCount = 0;
        InitializeTiles();
    }

    private void Update()
    {
        if (!_isEnabled) return;
        ArrowMovement();
    }

    protected override void DisableUI()
    {
        base.DisableUI();
        _isEnabled = false;
        _arrowMoveCount = 0;
        _arrowPos = -50;
        _arrow.style.left = new StyleLength(Length.Percent(_arrowPos));
        _tileList.Clear();
        _fishTiles.Clear();
        _trashTiles.Clear();
        _tiles.Clear();

        inputReader.OnFishEvent -= CatchFish;
        _tileContainer.AddToClassList("hide");
    }

    private void CatchFish()
    {
        if (!_isEnabled) return;
        foreach (var tile in _tileList.Where(tile => tile.worldBound.Overlaps(_arrow.worldBound)))
        {
            if (tile.ClassListContains("fish"))
            {
                Debug.Log("I caught fish");
                _percentage[FishTileType.FishTile] += _fishTiles[tile] * 5;
            }
            else if (tile.ClassListContains("trash"))
            {
                Debug.Log("I caught trash");
                _percentage[FishTileType.TrashTile] += _trashTiles[tile] * 5;
            }
            else if (tile.ClassListContains("empty"))
            {
                Debug.Log("I caught empty");
                _percentage[FishTileType.EmptyTile] += tileCount * 5 / 2;
            }

            StartCoroutine(WaitAndInitializeTiles());

            break;
        }
    }

    private IEnumerator WaitAndInitializeTiles()
    {
        _isEnabled = false;
        yield return new WaitForSeconds(1f);
        _isEnabled = true;
        _arrowMoveCount++;
        InitializeTiles();
    }

    private void ArrowMovement()
    {
        if (_arrowMoveCount % 2 == 1)
            _arrowPos -= arrowSpeed * Time.deltaTime;
        else
            _arrowPos += arrowSpeed * Time.deltaTime;
        _arrow.style.left = new StyleLength(Length.Percent(_arrowPos));

        switch (_arrowPos)
        {
            case >= 49.5f when _arrowMoveCount % 2 == 0:
                _percentage[FishTileType.EmptyTile] += tileCount * 5;
                _arrowMoveCount++;
                InitializeTiles();
                break;
            case <= -49.5f when _arrowMoveCount % 2 == 1:
                _percentage[FishTileType.EmptyTile] += tileCount * 5;
                _arrowMoveCount++;
                InitializeTiles();
                break;
        }

        if (_arrowMoveCount < arrowRepeat) return;
        var item = GetRandomItem();
        if (item.itemType == ItemType.Tool && item.toolType == ToolType.Inventory)
        {
            var inventoryItem = new InventoryItem(item, 1, "갑판");
            foreach (var itemIn in _inventoryItems)
            {
                var randomValue = Random.Range(0, itemIn.Value);
                if (randomValue > 0)
                {
                    var itemInInventory = new InventoryItem(itemIn.Key, randomValue, inventoryItem.name);
                    inventoryItem.itemsIn.Add(itemInInventory);
                }
            }
            OnInventoryFishEnd?.Invoke(inventoryItem, 1);
        }
        else
            OnItemFishEnd?.Invoke(item, Random.Range(1, 3));
        _isEnabled = false;
        DisableUI();
    }

    private void InitializeTiles()
    {
        if (_arrowMoveCount % 2 == 1)
            _arrowPos = 50;
        else
            _arrowPos = -50;
        _arrow.style.left = new StyleLength(Length.Percent(_arrowPos));
        _tiles.Clear();

        _tileList.Clear();
        _fishTiles.Clear();
        _trashTiles.Clear();

        var remainingEmptyTile = tileCount / 2;
        var remainingFishTile = Random.Range(tileCount / 4, tileCount / 2);
        var remainingTrashTile = tileCount - remainingEmptyTile - remainingFishTile;

        var lastTileType = FishTileType.None;
        for (var i = 0; i < tileCount; i++)
        {
            if (remainingTrashTile == remainingEmptyTile && remainingEmptyTile == remainingFishTile &&
                remainingFishTile == 0)
                break;
            var currentTile = (FishTileType)Random.Range(0, 3);

            var attempts = 0;
            while ((currentTile == lastTileType && attempts < 10) ||
                   currentTile == FishTileType.FishTile && remainingFishTile == 0 ||
                   currentTile == FishTileType.TrashTile && remainingTrashTile == 0 ||
                   currentTile == FishTileType.EmptyTile && remainingEmptyTile == 0)
            {
                currentTile = (FishTileType)Random.Range(0, 3);
                attempts++;
            }


            var tile = new VisualElement();
            tile.AddToClassList("tile");

            var tileSize = 1;
            switch (currentTile)
            {
                case FishTileType.FishTile:
                    if (remainingFishTile > 0)
                    {
                        if (remainingTrashTile == 0 && remainingEmptyTile == 0)
                            tileSize = remainingFishTile;
                        else
                            tileSize = Random.Range(1, remainingFishTile);
                        remainingFishTile -= tileSize;
                        tile.AddToClassList("fish");
                        _fishTiles.Add(tile, tileSize);
                        var sprite = fishSprites[Random.Range(0, fishSprites.Count)];
                        var icon = new VisualElement();
                        icon.AddToClassList("icon");
                        icon.style.backgroundImage = new StyleBackground(sprite);
                        tile.Add(icon);
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                    break;
                case FishTileType.TrashTile:
                    if (remainingTrashTile > 0)
                    {
                        if (remainingFishTile == 0 && remainingEmptyTile == 0)
                            tileSize = remainingTrashTile;
                        else
                            tileSize = Random.Range(1, remainingTrashTile);
                        remainingTrashTile -= tileSize;
                        tile.AddToClassList("trash");
                        _trashTiles.Add(tile, tileSize);
                        var sprite = trashSprites[Random.Range(0, trashSprites.Count)];
                        var icon = new VisualElement();
                        icon.AddToClassList("icon");
                        icon.style.backgroundImage = new StyleBackground(sprite);
                        tile.Add(icon);
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                    break;
                case FishTileType.EmptyTile:
                    if (remainingEmptyTile > 0)
                    {
                        if (remainingFishTile == 0 && remainingTrashTile == 0)
                            tileSize = remainingEmptyTile;
                        else
                            tileSize = Random.Range(1, remainingEmptyTile);
                        remainingEmptyTile -= tileSize;
                        tile.AddToClassList("empty");
                    }
                    else
                    {
                        i--;
                        continue;
                    }

                    break;
                case FishTileType.None:
                    i--;
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _tiles.Add(tile);
            _tileList.Add(tile);
            tile.style.flexGrow = tileSize;
            lastTileType = currentTile;
        }
    }

    private ItemSO GetRandomItem()
    {
        var allPercentage = _percentage[FishTileType.FishTile] + _percentage[FishTileType.TrashTile] +
                            _percentage[FishTileType.EmptyTile];
        var randomValue = Random.Range(0, allPercentage);

        Debug.Log(
            $"Fish : {_percentage[FishTileType.FishTile]} Trash : {_percentage[FishTileType.TrashTile]} Empty : {_percentage[FishTileType.EmptyTile]}" +
            $"All : {allPercentage} Random : {randomValue}");

        if (randomValue < _percentage[FishTileType.FishTile])
        {
            Debug.Log("This is fish");
            var percentage = 0f;
            var randomPercentage = Random.Range(0, _fishItemPercentage);
            foreach (var fish in _fishItems)
            {
                percentage += fish.percentageOfCatch;
                if (randomPercentage < percentage)
                {
                    return fish;
                }
            }

            return null;
        }
        else if (randomValue < _percentage[FishTileType.FishTile] + _percentage[FishTileType.TrashTile])
        {
            Debug.Log("This is trash");
            var percentage = 0f;
            var randomPercentage = Random.Range(0, _trashItemPercentage);
            foreach (var trash in _trashItems)
            {
                percentage += trash.percentageOfCatch;
                if (randomPercentage < percentage)
                {
                    Debug.Log(trash.itemName);
                    return trash;
                }
            }

            return null;
        }
        else
        {
            Debug.Log("This is empty");
            return null;
        }
    }

    public void HandleFishLevelUp(AbilityType abilityType, int value)
    {
        if (abilityType != AbilityType.Fishing) return;
        arrowSpeed += 10;
        arrowRepeat = value / 2 + 1;
    }
}

public enum FishTileType
{
    FishTile,
    TrashTile,
    EmptyTile,
    None
}