using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSOWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    
    
    [MenuItem("CustomWindow/ItemSOWindow")]
    public static void ShowWindow()
    {
        ItemSOWindow wnd = GetWindow<ItemSOWindow>();
        wnd.titleContent = new GUIContent("ItemSOWindow");
        wnd.minSize = new Vector2(400, 500);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement content = m_VisualTreeAsset.Instantiate();
        content.style.flexGrow = 1;
        root.Add(content);
        
        InitializeWindow();
        CreateItemList();
    }
    private void InitializeWindow()
    {
        
    }
    private void CreateItemList()
    {
    }

}
