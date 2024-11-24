using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CookMiniGameUI : ToolkitParents
{
    [SerializeField] private InputReader inputReader;

    [Header("Note Settings")] [SerializeField]
    private float noteDistance;

    [SerializeField] private string[] noteClassNames;
    [SerializeField] private float[] noteSpeeds;

    private float _currentNoteSpeed;
    private int _currentNoteCount;
    private int _correctNoteCount;

    private VisualElement _container;
    private VisualElement _noteContainer;
    private VisualElement _noteBounds;
    private List<VisualElement> _notes = new List<VisualElement>();

    private float _notePosition;

    private bool _isCooking;

    private ItemSO _cookingItem;

    public UnityEvent<ItemSO> OnCookingEnd;

    [SerializeField] private ItemSO testItem;

    protected override void OnEnable()
    {
        base.OnEnable();
        _container = root.Q<VisualElement>("NoteContainer");
        _noteContainer = _container.Q<VisualElement>("Notes");
        _noteBounds = _container.Q<VisualElement>("NoteBound");
    }

    private void Start()
    {
        EnableUI(testItem);
    }

    public void EnableUI(ItemSO item)
    {
        base.EnableUI();
        _container.RemoveFromClassList("hide");
        _cookingItem = item;
        if (item == null || item.itemType != ItemType.Food) return;
        _currentNoteSpeed = noteSpeeds[(int)item.foodType];
        foreach (var material in item.materialList)
        {
            _currentNoteCount += material.Value;
        }

        inputReader.OnNotesEvent += CheckNote;
        _isCooking = true;

        InitializeNotes();
    }

    protected override void DisableUI()
    {
        base.DisableUI();
        inputReader.OnNotesEvent -= CheckNote;
        _notePosition = 0;
        _notes.ForEach(x => x.RemoveFromHierarchy());
        _notes.Clear();
        _noteContainer.Clear();
        _isCooking = false;
        _container.AddToClassList("hide");
    }

    private void CheckNote(Vector2Int direction)
    {
        var note = _notes.Find(x => _noteBounds.worldBound.Overlaps(x.worldBound) && GetDirection(x) == direction);
        if (note == null) return;
        _correctNoteCount++;
        note.AddToClassList("hide");
    }

    private static Vector2Int GetDirection(VisualElement note)
    {
        if(note.ClassListContains("hide")) return Vector2Int.zero;
        if (note.ClassListContains("up")) return Vector2Int.up;
        if (note.ClassListContains("down")) return Vector2Int.down;
        if (note.ClassListContains("left")) return Vector2Int.left;
        if (note.ClassListContains("right")) return Vector2Int.right;
        return Vector2Int.zero;
    }

    private void Update()
    {
        if (!_isCooking) return;

        _notePosition += _currentNoteSpeed * Time.deltaTime;

        if (_notes.Count == 0)
        {
            _isCooking = false;
            OnCookingEnd?.Invoke(_cookingItem);
            DisableUI();
            return;
        }

        foreach (var note in _notes)
        {
            note.style.left = new StyleLength(Length.Percent(_notePosition - noteDistance * _notes.IndexOf(note)));
        }
    }

    private void InitializeNotes()
    {
        _correctNoteCount = 0;
        for (int i = 0; i < _currentNoteCount; i++)
        {
            var note = new VisualElement();
            note.AddToClassList("note");
            _noteContainer.Add(note);
            _notes.Add(note);
            var noteIndex = Random.Range(0, 4);
            note.AddToClassList(noteClassNames[noteIndex]);
        }
    }
}