using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSoWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CustomWindows/ItemSOWindow")]
    public static void ShowWindow()
    {
        ItemSoWindow wnd = GetWindow<ItemSoWindow>();
        wnd.titleContent = new GUIContent("ItemSOWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement content = m_VisualTreeAsset.Instantiate();
        root.Add(content);
        //
        // ObjectField objectField = content.Q<ObjectField>();
        // objectField.objectType = typeof(ItemSO);
        // objectField.refere
    }
}
